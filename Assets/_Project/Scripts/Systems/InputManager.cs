using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace bpdev
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private float MinimumZoomLevel;
        [SerializeField] private float MaximumZoomLevel;

        public Vector2 MPosScreen {get; private set;}
        public Vector3 MPosWorld {get; private set;}
        public Vector3Int MPosCell {get; private set;}

        public Vector2 PlayerMoveDir {get; private set;}

        public Vector3 PlayerPos {get; private set;}
        public Vector3Int PlayerCellPos {get; private set;}
        public Vector3Int PlayerCellPosLastFrame {get; private set;}

        [Header("Inputs")]
        private InputAction a_leftClick;
        private InputAction a_rightClick;
        private InputAction a_mousePosition;
        private InputAction a_hotbarNumbers;
        private InputAction a_scrollWheel;

        private InputAction a_cameraPan;
        private InputAction a_cameraZoomIn;
        private InputAction a_cameraZoomOut;

        private InputAction a_debug;

        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera cvCam; //
        [SerializeField] private Transform player; //

        [Header("Events")]
        [SerializeField] private GameEvent_Int e_OnHotbarSelection; 
        [SerializeField] private GameEvent_Int e_OnDebugButtonPressed; 
        [SerializeField] private GameEvent_Int e_OnRotateCursorClockwise; 
        [SerializeField] private GameEvent_Int e_OnRotateCursorAntiClockwise; 


        protected override void Awake()
        {
            base.Awake();

            PlayerInput input = new PlayerInput();
            input.Enable();

            // MOUSE
            a_leftClick = input.Player.LeftClick;
            a_leftClick.performed += ctx => OnLeftClick();

            a_rightClick = input.Player.RightClick;
            a_rightClick.performed += ctx => OnRightClick();

            a_mousePosition = input.Player.MousePosition;

            a_scrollWheel = input.Player.ScrollWheel;
            a_scrollWheel.performed += ctx => OnScrollWheel(ctx);

            // HOTBAR
            a_hotbarNumbers = input.Player.HotbarNumbers;
            a_hotbarNumbers.performed += ctx => OnPressNumber(ctx);

            // CAMERA
            a_cameraPan = input.Camera.Pan;

            a_cameraZoomIn = input.Camera.ZoomIn;
            a_cameraZoomIn.performed += ctx => OnCameraZoomIn();

            a_cameraZoomOut = input.Camera.ZoomOut;
            a_cameraZoomOut.performed += ctx => OnCameraZoomOut();

            // DEBUG DISPLAY
            a_debug = input.Player.Debug;
            a_debug.performed += ctx => OnDebugPressed();
        }

        private void Update()
        {
            CalcMousePositions();
            CalcPlayerMovement();
            CalcPlayerPositions();
        }

        private void CalcPlayerMovement()
        {
            PlayerMoveDir = a_cameraPan.ReadValue<Vector2>();
        }

        private void CalcPlayerPositions()
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerMovement>().transform.parent;
            }

            PlayerCellPosLastFrame = PlayerCellPos;
            PlayerPos = player.transform.position;
            PlayerCellPos = TilemapManager.Instance.WorldToCell(PlayerPos);
        }

        private void CalcMousePositions()
        {
            MPosScreen = a_mousePosition.ReadValue<Vector2>();
            MPosWorld = Camera.main.ScreenToWorldPoint(MPosScreen);
            // mousePosWorld.z = 0f;
            MPosCell = TilemapManager.Instance.WorldToCell(MPosWorld);
        }

        // BUTTON CALLBACKS

        private void OnLeftClick()
        {
            if (!TilemapManager.Instance.InsideBounds(MPosCell)) { return; }

            GameObject _building = BuildingProxy.Instance.GetBuildingAt(MPosCell);

            if (_building != null)
            {
                _building.GetComponent<TSystemRotator>()?.RotateClockwise();
            }
            else
            {
                BuildingType _selectedBuildingType = UIHotbar.Instance.GetSelected().buildingType;

                if (_selectedBuildingType == BuildingType.UNASSIGNED) { return; }

                BuildingProxy.Instance.InstantiateBuildingAt(_selectedBuildingType, MPosCell);
            }
        }

        private void OnRightClick() 
        {
            if (!TilemapManager.Instance.InsideBounds(MPosCell)) { return; }

            BuildingProxy.Instance.DestroyBuildingAt(MPosCell);
        }

        private void OnPressNumber(InputAction.CallbackContext ctx)
        {
            int num;
            bool valid = int.TryParse(ctx.control.name, out num);

            if (valid)
            {
                // + 9 % 10 scales values down by 1, so pressing 1 selects index 0 (but won't go into negatives)
                e_OnHotbarSelection.Raise((num + 9) % 10);
            }
        }

        private void OnDebugPressed()
        {
            e_OnDebugButtonPressed.Raise(-1);
        }

        private void OnCameraZoomOut()
        {
            if (cvCam == null)
            {
                cvCam = Camera.main.transform.parent.GetComponent<CinemachineVirtualCamera>();
            }

            cvCam.m_Lens.OrthographicSize = Mathf.Clamp(cvCam.m_Lens.OrthographicSize + 1, MinimumZoomLevel, MaximumZoomLevel);
        }

        private void OnCameraZoomIn()
        {
            if (cvCam == null)
            {
                cvCam = Camera.main.transform.parent.GetComponent<CinemachineVirtualCamera>();
            }

            cvCam.m_Lens.OrthographicSize = Mathf.Clamp(cvCam.m_Lens.OrthographicSize - 1, MinimumZoomLevel, MaximumZoomLevel);
        }
    
        private void OnScrollWheel(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValue<float>() > 0f)
            {
                e_OnRotateCursorAntiClockwise.Raise(-1);
            }

            if (ctx.ReadValue<float>() < 0f)
            {
                e_OnRotateCursorClockwise.Raise(-1);
            }
        }
    }
}
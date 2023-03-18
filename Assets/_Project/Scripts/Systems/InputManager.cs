using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Cinemachine;

namespace bpdev
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private float MinimumZoomLevel;
        [SerializeField] private float MaximumZoomLevel;

        private float cameraZoom;

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

        private InputAction a_camera_pan;
        private InputAction a_camera_zoom;

        private InputAction a_debug;

        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera cvCam; //

        [SerializeField] private Transform player; //

        [Header("Events")]
        [SerializeField] private GameEvent_Int e_OnHotbarSelection; 
        [SerializeField] private GameEvent_Int e_OnDebugButtonPressed; 


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

            // HOTBAR
            a_hotbarNumbers = input.Player.HotbarNumbers;
            a_hotbarNumbers.performed += ctx => OnPressNumber(ctx);

            // CAMERA
            a_camera_pan = input.Camera.Pan;

            a_camera_zoom = input.Camera.Zoom;
            a_camera_zoom.performed += ctx => OnCameraZoom();

            // DEBUG DISPLAY
            a_debug = input.Player.Debug;
            a_debug.performed += ctx => OnDebugPressed();
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().transform.parent;
            cvCam = Camera.main.transform.parent.GetComponent<CinemachineVirtualCamera>();

            // CalcPlayerPositions();
        }

        private void Update()
        {
            CalcMousePositions();
            CalcPlayerMovement();
            CalcPlayerPositions();
        }

        private void OnCameraZoom()
        {
            cameraZoom = a_camera_zoom.ReadValue<float>();
            if (cameraZoom == 0f) { return; }

            if (cameraZoom > 0)
            {
                cvCam.m_Lens.OrthographicSize = Mathf.Clamp(cvCam.m_Lens.OrthographicSize + 1, MinimumZoomLevel, MaximumZoomLevel);
            }
            else
            {
                cvCam.m_Lens.OrthographicSize = Mathf.Clamp(cvCam.m_Lens.OrthographicSize - 1, MinimumZoomLevel, MaximumZoomLevel);
            }
        }

        private void CalcPlayerMovement()
        {
            PlayerMoveDir = a_camera_pan.ReadValue<Vector2>();
        }

        private void CalcPlayerPositions()
        {
            PlayerCellPosLastFrame = PlayerCellPos;
            PlayerCellPos = TilemapManager.Instance.WorldToCell(player.transform.position);
        }

        private void CalcMousePositions()
        {
            MPosScreen = a_mousePosition.ReadValue<Vector2>();
            MPosWorld = Camera.main.ScreenToWorldPoint(MPosScreen);
            // mousePosWorld.z = 0f;
            MPosCell = TilemapManager.Instance.WorldToCell(MPosWorld);
        }

        // BUTTONS

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
                BuildingType _selectedBuildingType = UIHotbarManager.Instance.GetSelected().buildingType;

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
    }
}
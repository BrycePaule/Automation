using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public float CameraPanningSpeed;
    public float MinimumZoomLevel;
    public float MaximumZoomLevel;

    private Vector2 mousePosScreen;
    private Vector3 mousePosWorld;
    private Vector3Int mousePosCell;

    private Vector2 moveDir;
    private float cameraZoom;

    [Header("Inputs")]
    private InputAction a_leftClick;
    private InputAction a_rightClick;
    private InputAction a_mousePosition;
    private InputAction a_hotbarNumbers;

    private InputAction a_camera_pan;
    private InputAction a_camera_zoom;

    private InputAction a_debug;

    [Header("References")]
    [SerializeField] private TileCursor tileCursor;
    [SerializeField] private UIDebugDisplay DebugDisplay; 

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineVirtualCamera cvCam;

    [SerializeField] private Transform player;

    // UI
    [SerializeField] private UIHotbarManager hotbarManager;

    private Vector3Int playerCellPos;
    private Vector3Int playerCellPosLastFrame;


    private void Awake()
    {
        PlayerInput input = new PlayerInput();
        input.Enable();

        a_leftClick = input.Player.LeftClick;
        a_leftClick.performed += ctx => OnLeftClick();

        a_rightClick = input.Player.RightClick;
        a_rightClick.performed += ctx => OnRightClick();

        a_hotbarNumbers = input.Player.HotbarNumbers;
        a_hotbarNumbers.performed += ctx => OnPressNumber(ctx);

        a_mousePosition = input.Player.MousePosition;

        a_camera_pan = input.Camera.Pan;

        a_camera_zoom = input.Camera.Zoom;
        a_camera_zoom.performed += ctx => OnCameraZoom();

        a_debug = input.Player.Debug;
        a_debug.performed += ctx => OnDebugPressed();
    }

    private void Start()
    {
        CalcPlayerPositions();
        RefreshTilemap();
    }

    private void Update()
    {
        CalcMousePositions();
        CalcPlayerPositions();

        RefreshTilemap();
        RefreshTileCursor();
        RefreshDebugMenu();
    }

    private void RefreshDebugMenu()
    {
        DebugDisplay.SetMPosScreen(mousePosScreen);
        DebugDisplay.SetMPosWorld(mousePosWorld);
        DebugDisplay.SetMPosCell(mousePosCell);

        DebugDisplay.SetPlayerPosWorld(player.position);
        DebugDisplay.SetPlayerPosCell(playerCellPos);
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

    private void CalcPlayerPositions()
    {
        moveDir = a_camera_pan.ReadValue<Vector2>();

        if (moveDir == Vector2.zero) { return; }

        player.position += ((Vector3) moveDir) * CameraPanningSpeed * Time.deltaTime;

        playerCellPosLastFrame = playerCellPos;
        playerCellPos = TilemapManager.Instance.WorldToCell(player.transform.position);
    }

    private void CalcMousePositions()
    {
        // Screen-space
        mousePosScreen = a_mousePosition.ReadValue<Vector2>();

        // World-space
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mousePosWorld.z = 0f;

        // Cell-space
        mousePosCell = TilemapManager.Instance.WorldToCell(mousePosWorld);
    }

    private void RefreshTileCursor()
    {
        if (TilemapManager.Instance.InsideBounds(mousePosCell))
        {
            tileCursor.Enable();
            tileCursor.UpdatePosition(mousePosCell);
        }
        else
        {
            tileCursor.Disable();
        }
    }

    private void RefreshTilemap()
    {
        if (playerCellPosLastFrame == null) { return; }

        if (playerCellPos != playerCellPosLastFrame)
        {
            TilemapManager.Instance.RefreshTiles(playerCellPos);
        }
    }

    // BUTTONS

    private void OnLeftClick()
    {
        if (!TilemapManager.Instance.InsideBounds(mousePosCell)) { return; }

        GameObject _building = BuildingProxy.Instance.GetBuildingAt(mousePosCell);

        if (_building != null)
        {
            _building.GetComponent<TSystemRotator>()?.RotateClockwise();
        }
        else
        {
            BuildingType _selectedBuildingType = hotbarManager.GetSelected().buildingType;

            if (_selectedBuildingType == BuildingType.UNASSIGNED) { return; }

            BuildingProxy.Instance.InstantiateBuildingAt(_selectedBuildingType, mousePosCell);
        }
    }

    private void OnRightClick() 
    {
        if (!TilemapManager.Instance.InsideBounds(mousePosCell)) { return; }

        BuildingProxy.Instance.DestroyBuildingAt(mousePosCell);
    }

    private void OnPressNumber(InputAction.CallbackContext ctx)
    {
        int num;
        bool valid = int.TryParse(ctx.control.name, out num);

        if (valid)
        {
            // + 9 % 10 scales values down by 1, so pressing 1 selects index 0 (but won't go into negatives)
            hotbarManager.SelectSlot((num + 9) % 10);
        }
    }

    private void OnDebugPressed()
    {
        DebugDisplay.ToggleDisplay();
    }
}

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

    private Vector2 moveDir;
    private float cameraZoom;

    [Header("Inputs")]
    private InputAction a_leftClick;
    private InputAction a_rightClick;
    private InputAction a_mousePosition;
    private InputAction a_hotbarNumbers;

    private InputAction a_camera_pan;
    private InputAction a_camera_zoom;

    [Header("References")]
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TSystemManager tSysManager;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private TileCursor tileCursor;

    [SerializeField] private PrefabLibrary prefabLibrary;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineVirtualCamera cvCam;

    [SerializeField] private Transform player;

    // UI
    [SerializeField] private UIHotbarManager hotbarManager;



    private void Awake()
    {
        PlayerInput input = new PlayerInput();
        input.Enable();

        a_leftClick = input.Player.LeftClick;
        a_leftClick.performed += ctx => OnLeftClick();

        a_rightClick = input.Player.RightClick;
        a_rightClick.performed += ctx => OnRightClick();

        a_hotbarNumbers = input.Player.HotbarNumbers;
        a_hotbarNumbers.performed += ctx => OnNumber(ctx);

        a_mousePosition = input.Player.MousePosition;

        a_camera_pan = input.Camera.Pan;

        a_camera_zoom = input.Camera.Zoom;
        a_camera_zoom.performed += ctx => OnCameraZoom();
    }

    private void Start()
    {
        player.position = new Vector3(mapGenerator.MapSize / 2, mapGenerator.MapSize / 2, 0f);
    }

    private void Update()
    {
        UpdateMousePos();
        HandleTileCursor();
        UpdatePlayerPosition();
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

    private void UpdatePlayerPosition()
    {
        moveDir = a_camera_pan.ReadValue<Vector2>();

        if (moveDir == Vector2.zero) { return; }

        player.position += ((Vector3) moveDir) * CameraPanningSpeed * Time.deltaTime;
    }

    private void UpdateMousePos()
    {
        mousePosScreen = a_mousePosition.ReadValue<Vector2>();
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mousePosWorld.z = 0f;
    }

    private void HandleTileCursor()
    {
        if (tilemapManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            tileCursor.Visible = true;
            tileCursor.UpdatePosition(tilemapManager.TileAnchorFromWorldPos(mousePosWorld));
        }
        else
        {
            tileCursor.Visible = false;
        }
    }

    // BUTTONS

    private void OnLeftClick()
    {
        RaycastHit2D _hit = Physics2D.Raycast(mousePosWorld, Vector2.zero);

        if (_hit)
        {
            _hit.transform.gameObject.GetComponent<TSystemRotator>()?.RotateClockwise();
        }
        else
        {
            PrefabType selectedBuilding = hotbarManager.GetSelected().building;
            if (selectedBuilding == PrefabType.NOT_SELECTED) { return; }

            GameObject gObj = prefabLibrary.GetPrefabOfType(selectedBuilding);

            bool needsTilemap = gObj.TryGetComponent<ITilemapConnected>(out _);
            bool needsPrefabLibrary = gObj.TryGetComponent<IPrefabLibraryConnected>(out _);

            if (needsPrefabLibrary)
            {
                tSysManager.PlaceTSystemObjectAtWorldPos(gObj, mousePosWorld, tilemap, prefabLibrary);
            }
            else if (needsTilemap)
            {
                tSysManager.PlaceTSystemObjectAtWorldPos(gObj, mousePosWorld, tilemap);
            }
            else
            {
                tSysManager.PlaceTSystemObjectAtWorldPos(gObj, mousePosWorld);
            }
        }
    }

    private void OnRightClick() 
    {
        RaycastHit2D _hit = Physics2D.Raycast(mousePosWorld, Vector2.zero);
        if (_hit)
        {
            tSysManager.DestroyTSystemObjectAtWorldPos(mousePosWorld);
        }
    }

    // private void On1()
    // {
    //     Component _connector = tSysManager.GetTSystemObjectAtWorldPos(mousePosWorld);
    //     if (_connector == null) { return; }

    //     ITSystemReceivable _receiver = _connector.GetComponent<ITSystemReceivable>();
    //     if (_receiver == null) { return; }

    //     // TODO: delegate this somewhere else later
    //     Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    //     Item _item = Instantiate(prefabLibrary.GetPrefabOfType(PrefabType.Item), Vector3.zero, Quaternion.identity).GetComponent<Item>();
    //     _item.gameObject.GetComponent<SpriteRenderer>().color = randomColor;

    //     if (!_receiver.CanReceive(_item.ItemType)) 
    //     {
    //         Destroy(_item.gameObject);
    //         return;
    //     }

    //     _receiver.Give(_item);
    // }

    // private void On2()
    // {
    //     RaycastHit2D _hit = Physics2D.Raycast(mousePosWorld, Vector2.zero);
    //     if (_hit) { return; }

    //     PrefabType buildingToPlace = PrefabType.Drill;
    //     tSysManager.PlaceTSystemObjectAtWorldPos(prefabLibrary.GetPrefabOfType(buildingToPlace), mousePosWorld, tilemap, prefabLibrary);
    // }

    // private void On3()
    // {
    //     RaycastHit2D _hit = Physics2D.Raycast(mousePosWorld, Vector2.zero);
    //     if (_hit) { return; }

    //     PrefabType buildingToPlace = PrefabType.Sink;
    //     tSysManager.PlaceTSystemObjectAtWorldPos(prefabLibrary.GetPrefabOfType(buildingToPlace), mousePosWorld);
    // }

    private void OnNumber(InputAction.CallbackContext ctx)
    {
        int num;
        bool valid = int.TryParse(ctx.control.name, out num);

        if (valid)
        {
            // + 9 % 10 scales values down by 1, so pressing 1 selects index 0 (but won't go into negatives)
            hotbarManager.SelectSlot((num + 9) % 10);
        }
    }
}

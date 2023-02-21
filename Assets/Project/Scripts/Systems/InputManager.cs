using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector2 mousePosScreen;
    private Vector3 mousePosWorld;

    [Header("Inputs")]
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePosition;
    private InputAction key_Q;
    private InputAction key_W;
    private InputAction key_E;

    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TSystemManager TSysManager;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private TileCursor tileCursor;
    [SerializeField] private PrefabLibrary prefabLibrary;

    private void Awake()
    {
        PlayerInput _playerInput = new PlayerInput();
        _playerInput.Enable();


        leftClick = _playerInput.Player.LeftClick;
        leftClick.performed += ctx => OnLeftClick();

        rightClick = _playerInput.Player.RightClick;
        rightClick.performed += ctx => OnRightClick();

        key_Q = _playerInput.Player.Q;
        key_Q.performed += ctx => OnQ();

        key_W = _playerInput.Player.W;
        key_W.performed += ctx => OnW();

        key_E = _playerInput.Player.E;
        key_E.performed += ctx => OnE();

        mousePosition = _playerInput.Player.MousePosition;
    }

    private void Update()
    {
        mousePosScreen = mousePosition.ReadValue<Vector2>();

        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        mousePosWorld.z = 0f;

        HandleTileCursor();
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
        RaycastHit2D _hit = Physics2D.Raycast(mousePosWorld, Vector2.up);

        if (_hit)
        {
            _hit.transform.gameObject.GetComponent<TSystemRotator>()?.RotateClockwise();
        }

        if (tilemapManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            TSysManager.CreateConnectableAtWorldPos(prefabLibrary.GetPrefabOfType(PrefabType.Conveyor), mousePosWorld);
        }
    }

    private void OnRightClick() => TSysManager.DestroyConnectableAtWorldPos(mousePosWorld);

    private void OnQ()
    {
        TSystemConnector _connectable = TSysManager.GetConnectableAtWorldPos(mousePosWorld);
        if (!_connectable) { return; }
        if (!_connectable.GetComponent<TSystemQueueReceiver>().CanReceiveItem()) { return; }
        
        // TODO: delegate this somewhere else later
        Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        Item item = Instantiate(prefabLibrary.GetPrefabOfType(PrefabType.Item), Vector3.zero, Quaternion.identity).GetComponent<Item>();
        item.gameObject.GetComponent<SpriteRenderer>().color = randomColor;

        _connectable.GetComponent<TSystemQueueReceiver>().PlaceItem(item);
    }

    private void OnW()
    {
        if (tilemapManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            TSysManager.CreateConnectableAtWorldPos(prefabLibrary.GetPrefabOfType(PrefabType.Spawner), mousePosWorld);
        }
    }

    private void OnE()
    {
        if (tilemapManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            TSysManager.CreateConnectableAtWorldPos(prefabLibrary.GetPrefabOfType(PrefabType.Sink), mousePosWorld);
        }
    }
}

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

    [Header("References")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private ConveyorManager conveyorManager;
    [SerializeField] private TileCursor tileCursor;

    [SerializeField] private GameObject itemPrefab;

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
        if (conveyorManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            tileCursor.Visible = true;
            tileCursor.UpdatePosition(conveyorManager.TileAnchorFromWorldPos(mousePosWorld));
        }
        else
        {
            tileCursor.Visible = false;
        }
    }

    // BUTTONS

    private void OnLeftClick()
    {
        if (conveyorManager.IsLayerAtWorldPos(Layers.Conveyer, mousePosWorld))
        {
            conveyorManager.GetConveyorAtWorldPos(mousePosWorld)?.RotateClockwise();
        }

        if (conveyorManager.IsLayerAtWorldPos(Layers.Tilemap, mousePosWorld))
        {
            conveyorManager.CreateConveyorAtWorldPos(mousePosWorld);
        }
    }

    private void OnRightClick() => conveyorManager.DestroyConveyorAtWorldPos(mousePosWorld);

    private void OnQ()
    {
        Conveyor _conv = conveyorManager.GetConveyorAtWorldPos(mousePosWorld);
        if (!_conv) { return; }
        
        // TODO: delegate this somewhere else later
        Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        Item item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity).GetComponent<Item>();
        item.gameObject.GetComponent<SpriteRenderer>().color = randomColor;

        if (_conv.CanReceiveItem())
        {
            _conv.PlaceItem(item);
        }
    }
}

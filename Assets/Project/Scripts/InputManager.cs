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
    [SerializeField] private GameObject conveyerPrefab;
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
    }

    // BUTTONS

    private void OnLeftClick()
    {
        if (RayToConveyor(mousePosScreen))
        {
            GetConveyor().RotateClockwise();
        }

        if (RayToTilemap(mousePosScreen))
        {
            Vector3 offsetCorrection = new Vector3(.5f, .5f, 0);
            Vector3Int cell = _tilemap.WorldToCell(mousePosWorld);

            Conveyor conv = Instantiate(conveyerPrefab, _tilemap.CellToWorld(cell) + offsetCorrection, Quaternion.identity).GetComponent<Conveyor>();
            conv.Tilemap = _tilemap;
            RefreshConveyorsInBlock(cell);
        }
    }

    private void RefreshConveyorsInBlock(Vector3Int cell)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 offset = new Vector3(i, j, 0);
                Conveyor adjConv = GetConveyorAtWorldPos(cell + offset);

                if (adjConv)
                    adjConv.RefreshConnections();
            }
        }
    }

    private void OnRightClick()
    {
        Conveyor conv = GetConveyor();
        
        if (conv)
            Destroy(conv.gameObject);
    }

    private void OnQ()
    {
        Conveyor conv = GetConveyor();
        if (conv == null) { return; }
        if (!conv.SlotQueue.Peek().IsEmpty()) { return; }

        GameObject itemObj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        conv.PlaceItem(itemObj.GetComponent<Item>());

        Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        itemObj.GetComponent<SpriteRenderer>().color = randomColor;
    }

    // HELPERS

    private Conveyor GetConveyorAtWorldPos(Vector3 _pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(_pos, Vector2.up);

        if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
        {
            return hit.transform.GetComponent<Conveyor>();
        }
        
        return null;
    }

    private Conveyor GetConveyor()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosScreen);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);

        if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
        {
            return hit.transform.GetComponent<Conveyor>();
        }

        return null;
    }

    private bool RayToTilemap(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);

        return (hit && hit.transform.gameObject.layer == (int) Layers.Tilemap);
    }

    private bool RayToConveyor(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f);

        return (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer);
    }
}

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

    private void OnLeftClick()
    {
        Conveyor conv = GetConveyor();

        if (conv == null)
        {
            Vector3 offsetCorrection = new Vector3(.5f, .5f, 0);
            Vector3Int cell = _tilemap.WorldToCell(mousePosWorld);
            Instantiate(conveyerPrefab, _tilemap.CellToWorld(cell) + offsetCorrection, Quaternion.identity);
        } 
        else 
        {
            Destroy(conv.gameObject);
        }
    }

    private void OnRightClick()
    {
        Conveyor conv = GetConveyor();
        
        if (conv != null)
            conv.RotateClockwise();
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

    private void OnQ()
    {
        Conveyor conv = GetConveyor();
        if (conv == null) { return; }
        if (conv.Slots[0].IsNotEmpty()) { return; }

        GameObject itemObj = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        conv.PlaceItem(itemObj);
    }


}

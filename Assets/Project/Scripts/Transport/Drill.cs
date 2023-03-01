using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : MonoBehaviour, ITilemapConnected
{
    private CardinalDirection drillDirection;
    
    private bool running;
    private MyTile gem;

    private TSystemConnector connector;
    private Tilemap tilemap; 

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();   
    }

    private void Update()
    {
        RefreshDrillingTile();
    }

    private void RefreshDrillingTile()
    {
        if (tilemap == null) { return; }

        RefreshDrillDirection();

        Vector3Int _pos = tilemap.WorldToCell(transform.position + Utils.DirToVector(drillDirection));
        MyTile _facingTile = (MyTile) tilemap.GetTile(_pos);

        if (_facingTile.Mineable)
        {
            gem = _facingTile;
        }
    }

    private void RefreshDrillDirection()
    {
        int _facingAsInt = (int) connector.Facing;
        drillDirection = Utils.IntToCardinalDirection(_facingAsInt + 2);
    }

    public void SetTilemap(Tilemap _tilemap)
    {
        tilemap = _tilemap;
    }

    private void SwitchOn()
    {
        running = true;
    }

    private void SwitchOff()
    {
        running = false;
    }
}

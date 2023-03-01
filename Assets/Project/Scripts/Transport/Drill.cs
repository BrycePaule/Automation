using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : MonoBehaviour, ITilemapConnected
{
    private bool isRunning;
    private bool wasRunningLastFrame;

    private CardinalDirection drillDirection;
    private MyTile gem;
        
    private Tilemap tilemap; 
    private TSystemConnector connector;
    private ParticleSystem particles;

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();   
        particles = GetComponentInChildren<ParticleSystem>();   

        isRunning = false;
        wasRunningLastFrame = false;
    }

    private void FixedUpdate()
    {
        wasRunningLastFrame = isRunning;

        RefreshDrillDirection();
        RefreshDrillTile();

        StartOrStopRunning();
    }

    private void StartOrStopRunning()
    {
        if (gem != null && wasRunningLastFrame == false)
        {
            SwitchOn();
        }

        if (gem == null && isRunning == true)
        {
            SwitchOff();
        }
    }

    private void RefreshDrillTile()
    {
        if (tilemap == null) { return; }

        Vector3Int _pos = tilemap.WorldToCell(transform.position + Utils.DirToVector(drillDirection));
        MyTile _facingTile = (MyTile) tilemap.GetTile(_pos);

        if (_facingTile.Mineable)
        {
            gem = _facingTile;
        }
        else
        {
            gem = null;
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
        isRunning = true;
        particles.Play();
    }

    private void SwitchOff()
    {
        isRunning = false;
        particles.Stop(withChildren: false);
    }
}

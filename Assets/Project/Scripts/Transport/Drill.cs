using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Max drill speed = 10

// To calculate time to drill an item:
// ((MAX_DRILL_SPEED - DRILL_SPEED) * GEM_HARDNESS) / 10 = TIME_TO_DRILL

// e.g.
// ((10 - 1) * 1) / 10 = 0.9s
// (9 * 1) / 10 = 0.9s
// 9 / 10 = 0.9s

// Each point of hardness adds (scaled drill speed * gem hardness / 10) to time


public class Drill : MonoBehaviour, ITilemapConnected
{
    [Range(1, 10)]
    public int DrillSpeed;

    private bool isRunning;
    private bool wasRunningLastFrame;

    private CardinalDirection drillDirection;
    private MyTile gem;

    private float timer;
    private float timeToDrill;
        
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

    private void Update()
    {
        wasRunningLastFrame = isRunning;
        timer += Time.deltaTime;

        RefreshDrillDirection();
        RefreshDrillTile();

        StartOrStopRunning();

        if (isRunning && timer >= timeToDrill)
        {
            timer = 0f;
            DrillItem();
        }

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
        MyTile _Tile = (MyTile) tilemap.GetTile(_pos);

        if (_Tile.Drillable)
        {
            gem = _Tile;
            timeToDrill = ((10 - DrillSpeed) * gem.Hardness) / 10;
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

    private void DrillItem()
    {
        GameObject _gObj = ResourceProxy.Instance.InstantiateByType(gem.ResourceTypeReleased);

        if (connector.CanOffloadItem(_gObj.GetComponent<Resource>()))
        {
            Resource _resource = _gObj.GetComponent<Resource>();
            connector.GetConnectedReceiver().Give(_resource);
        }
    }
}

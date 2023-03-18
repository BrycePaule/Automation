using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Max drill speed = 10

// To calculate time to drill an item:
// ((MAX_DRILL_SPEED - DRILL_SPEED) * GEM_HARDNESS) / 10 = TIME_TO_DRILL

// e.g.
// ((10 - 1) * 1) / 10 = 0.9s
// (9 * 1) / 10 = 0.9s
// 9 / 10 = 0.9s

// Each point of hardness adds (scaled drill speed * gem hardness / 10) to time

namespace bpdev
{
    [RequireComponent(typeof(TSystemConnector))]
    public class Drill : MonoBehaviour
    {
        [Range(1, 10)]
        public int DrillSpeed;

        private bool isRunning;
        private bool wasRunningLastFrame;

        private CardinalDirection drillDirection;
        private TerrainTile gem;

        private float timer;
        private float timeToDrill;
            
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
            TerrainTile _Tile = TilemapManager.Instance.GetTile(connector.CellPos + Utils.DirToVector(drillDirection));

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
            if (connector.CanOffloadItem(gem.ResourceTypeReleased))
            {
                GameObject _gObj = ResourceProxy.Instance.InstantiateByType(gem.ResourceTypeReleased);
                connector.GetConnectedReceiver().Give(_gObj.GetComponent<Resource>());
            }
        }
    }
}
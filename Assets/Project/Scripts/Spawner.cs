using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConveyorConnectable))]
[RequireComponent(typeof(Rotatable))]
public class Spawner : MonoBehaviour
{

    public float SpawnCadence;
    public GameObject ItemToSpawn;

    private float timer;
    private ConveyorConnectable convConnectable;

    private void Awake()
    {
        convConnectable = GetComponent<ConveyorConnectable>();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= SpawnCadence)
        {
            timer -= SpawnCadence;
            Spawn();
        }
    }

    private void Spawn()
    {
        if (convConnectable.NextConveyor == null) { return; }
        if (!convConnectable.NextConveyor.CanReceiveItem()) { return; }
        
        convConnectable.NextConveyor.PlaceItem(ItemToSpawn.GetComponent<Item>());
    }
}

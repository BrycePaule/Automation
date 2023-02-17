using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IConveyorConnectable, IConveyorReceivable
{

    public float SpawnCadence;
    public GameObject ItemToSpawn;

    private float timer;
    public Conveyor NextConveyor;


    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= SpawnCadence)
        {
            timer -= SpawnCadence;

        }
    }

    private void SpawnItem()
    {
        if (NextConveyor == null) { return; }
        
        Item item = ItemToSpawn.GetComponent<Item>();
    }


    public void RefreshPushConnection()
    {
        throw new System.NotImplementedException();
    }

    public bool CanReceiveItem()
    {
        throw new System.NotImplementedException();
    }

    public void PlaceItem(Item _item)
    {
        throw new System.NotImplementedException();
    }
}

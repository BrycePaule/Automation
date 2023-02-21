using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemConnector))]
[RequireComponent(typeof(TSystemRotator))]
public class Spawner : MonoBehaviour
{

    public float SpawnCadence;
    public GameObject ItemToSpawn;

    private float timer;
    private TSystemConnector convConnectable;

    private void Awake()
    {
        convConnectable = GetComponent<TSystemConnector>();
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
        if (!((Component) convConnectable.NextConveyor).GetComponent<TSystemQueueReceiver>().CanReceiveItem()) { return; }

        Item _item = Instantiate(ItemToSpawn, transform.position, Quaternion.identity).GetComponent<Item>();
        ((Component) convConnectable.NextConveyor).GetComponent<TSystemQueueReceiver>().PlaceItem(_item);
    }
}

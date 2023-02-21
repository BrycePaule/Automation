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
    private TSystemConnector tSysConnector;

    private void Awake()
    {
        tSysConnector = GetComponent<TSystemConnector>();
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
        if (!tSysConnector.HasConnection) { return; }

        Component _connectedObj = (Component) tSysConnector.ConnectedTo;
        if (!_connectedObj.GetComponent<TSystemQueueReceiver>().CanReceiveItem()) { return; }

        Item _item = Instantiate(ItemToSpawn, transform.position, Quaternion.identity).GetComponent<Item>();
        _connectedObj.GetComponent<TSystemQueueReceiver>().PlaceItem(_item);
    }
}

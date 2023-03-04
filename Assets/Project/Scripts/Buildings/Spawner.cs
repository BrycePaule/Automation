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
    private TSystemConnector connector;

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();
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
        Resource _item = Instantiate(ItemToSpawn, transform.position, Quaternion.identity).GetComponent<Resource>();

        if (connector.CanOffloadItem(_item))
        {
            connector.GetConnectedReceiver().Give(_item);
        }
        else
        {
            Destroy(_item.gameObject);
        }
    }
}

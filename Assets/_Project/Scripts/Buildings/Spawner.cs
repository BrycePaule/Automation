using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [RequireComponent(typeof(TSystemConnector))]
    [RequireComponent(typeof(TSystemRotator))]
    public class Spawner : MonoBehaviour
    {
        public float SpawnCadence;
        public ResourceType ResourceType;

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
            GameObject _gObj = ResourceProxy.Instance.InstantiateByType(ResourceType);
            Resource _resource = _gObj.GetComponent<Resource>();

            if (connector.CanOffloadItem(_resource.resourceType))
            {
                connector.GetConnectedReceiver().Give(_resource);
            }
            else
            {
                Destroy(_resource.gameObject);
            }
        }
    }
}
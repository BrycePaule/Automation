using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [RequireComponent(typeof(ITSystemConnectable))]
    public class TSystemBeltReceiver : MonoBehaviour, ITSystemReceivable
    {
        public int MaxItems;
        public List<Resource> Resources;

        public float gapWidth;

        private TSystemConnector connector;
        private Vector3 resetPos;

        private void Awake()
        {
            connector = transform.GetComponent<TSystemConnector>();

            Resources = new List<Resource>();
            SetSpacing();
        }

        public bool CanReceive(ResourceType resourceType)
        {
            if (!ItemMatchesFilter(resourceType)) { return false; }
            if (IsFull()) { return false; }

            return true;
        }

        public void Give(Resource resource)
        {
            resource.transform.SetParent(transform);
            resource.transform.position = transform.position;
            resource.transform.localPosition += resetPos;
            resource.transform.localRotation = transform.localRotation;

            int _rotationMultiplier = (int) connector.Facing - (int) CardinalDirection.East;
            resource.transform.Rotate(new Vector3(0f, 0f, -90 * _rotationMultiplier));

            Resources.Add(resource);
        }

        public bool ItemMatchesFilter(ResourceType resourceType)
        {
            TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
            if (_filter == null) { return true; }
        
            return _filter.Check(resourceType);
        }

        // HELPERS

        private bool IsFull()
        {
            return Resources.Count >= MaxItems;
        }

        private Resource RearItem()
        {
            return Resources[Resources.Count - 1];
        }

        private void SetSpacing()
        {
            resetPos = new Vector3(-0.5f, 0f, 0f);
            gapWidth =  1f / (MaxItems + 1);
        }
        
    }
}
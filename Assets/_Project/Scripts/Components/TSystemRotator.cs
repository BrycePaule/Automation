using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [RequireComponent(typeof(TSystemConnector))]
    public class TSystemRotator : MonoBehaviour, ITSystemRotatable
    {
        private TSystemConnector connector;
        private Transform connectionMarker;

        private bool hasMarker;

        private void Awake()
        {
            connector = GetComponent<TSystemConnector>();
        }

        public void RotateClockwise(bool _reverse = false)
        {
            int _currDir = (int) connector.Facing;
            int _newDir = (_currDir + 1) % 4;
            connector.Facing = (CardinalDirection) _newDir;

            Vector3 _rotateVector = _reverse ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90);

            transform.Rotate(_rotateVector);
            if (hasMarker)
            {
                connectionMarker.Rotate(_rotateVector);
            }

            connector.RefreshTSysConnection();
        }

        public void RotateAntiClockwise()
        {
            RotateClockwise(_reverse: true);
        }
    }
}
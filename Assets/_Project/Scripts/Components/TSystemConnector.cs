using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class TSystemConnector : MonoBehaviour, ITSystemConnectable
    {
        public ITSystemConnectable ConnectedTo;
        public CardinalDirection Facing = CardinalDirection.East;
        public Transform ConnectionMarker;

        public bool HasValidConnection;

        public Vector3Int CellPos { get; private set; }

        // DEBUG ONLY
        [SerializeField] private string TSysConnectionName;

        private void FixedUpdate()
        {
            UpdateConnectionDebugFlag();
        }

        public void RefreshTSysConnection()
        {
            ConnectedTo = null;

            Vector3Int _posInFront = CellPos + Utils.DirToVector(Facing);

            if (TilemapManager.Instance.ContainsBuilding(_posInFront))
            {
                ConnectedTo = TilemapManager.Instance.GetBuilding(_posInFront).GetComponent<ITSystemConnectable>();
            }

            UpdateConnectionDebugFlag();
        }

        public bool CanOffloadItem(ResourceType resourceType)
        {
            if (!HasValidConnection) { return false; }

            ITSystemReceivable _nextReceiver = GetConnectedReceiver();
            if (_nextReceiver == null) { return false; }

            if (!_nextReceiver.CanReceive(resourceType)) { return false; }

            return true;
        }

        public ITSystemReceivable GetConnectedReceiver()
        {
            if (!HasValidConnection) { return null; }

            ITSystemReceivable _receiver = ((Component) ConnectedTo).GetComponent<ITSystemReceivable>();
            return _receiver;
        }


        // DEBUG ONLY
        private void UpdateConnectionDebugFlag()
        {
            if (ConnectedTo == null)
            { 
                HasValidConnection = false;
                TSysConnectionName = "N/A";
            }
            else
            {
                HasValidConnection = true;
                TSysConnectionName = ConnectedTo.ToString();
            }
        }

        public void SetCellPosition(Vector3Int _cellPos)
        {
            CellPos = _cellPos;
        }
    }
}
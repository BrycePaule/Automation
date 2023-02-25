using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemConnector : MonoBehaviour, ITSystemConnectable
{
    public ITSystemConnectable ConnectedTo;
    public CardinalDirection Facing = CardinalDirection.East;
    public Transform ConnectionMarker;

    public bool HasValidConnection;

    // DEBUG ONLY
    [SerializeField] private string TSysConnectionName;

    private void FixedUpdate()
    {
        UpdateConnectionDebugFlag();
    }

    public void RefreshTSysConnection()
    {
        ConnectedTo = null;

        RaycastHit2D _hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);

        if (_hit && _hit.transform.GetComponent<ITSystemConnectable>() != null)
        {
            ConnectedTo = _hit.transform.GetComponent<ITSystemConnectable>();
        }

        UpdateConnectionDebugFlag();
    }

    public bool CanOffloadItem(Item _item)
    {
        if (_item == null) { return false; }
        if (!HasValidConnection) { return false; }

        ITSystemReceivable _nextReceiver = GetConnectedReceiver();

        if (_nextReceiver == null) { return false; }
        if (!_nextReceiver.CanReceive(_item)) { return false; }

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
}

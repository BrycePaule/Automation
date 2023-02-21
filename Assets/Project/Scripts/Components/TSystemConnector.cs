using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemConnector : MonoBehaviour, ITSystemConnectable
{
    public ITSystemConnectable ConnectedTo;
    public CardinalDirection Facing = CardinalDirection.East;
    public bool HasConnection;

    // For debugging as interface is not serializable
    [SerializeField] private string NextConveyorName;

    private void Update()
    {
        UpdateConnectionDebugFlag();
    }

    public void RefreshPushConnection()
    {
        ConnectedTo = null;

        RaycastHit2D _hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);

        if (_hit.transform.GetComponent<ITSystemConnectable>() != null)
        {
            ConnectedTo = _hit.transform.GetComponent<ITSystemConnectable>();
        }

        UpdateConnectionDebugFlag();
    }

    public bool CanOffloadItem()
    {
        if (ConnectedTo == null) { return false; }

        ITSystemReceivable _nextReceiver = ((Component) ConnectedTo).GetComponent<ITSystemReceivable>();

        if (_nextReceiver == null) { return false; }
        if (!_nextReceiver.CanReceiveItem()) { return false; }

        return true;
    }

    private void UpdateConnectionDebugFlag()
    {
        if (ConnectedTo == null)
        { 
            HasConnection = false;
            NextConveyorName = "N/A";
        }
        else
        {
            HasConnection = true;
            NextConveyorName = ConnectedTo.ToString();
        }
    }
}

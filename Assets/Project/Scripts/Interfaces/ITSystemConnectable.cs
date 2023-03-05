using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemConnectable
{
    public void RefreshTSysConnection();
    public ITSystemReceivable GetConnectedReceiver();
    public void SetCellPosition(Vector3Int cellPos);

    // CHECKERS
    public bool CanOffloadItem(ResourceType resourceType);
}

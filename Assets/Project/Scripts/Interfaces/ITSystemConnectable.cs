using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemConnectable
{
    public void RefreshTSysConnection();
    public ITSystemReceivable GetConnectedReceiver();

    // CHECKERS
    public bool CanOffloadItem(Resource resource);
}

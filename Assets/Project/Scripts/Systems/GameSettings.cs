using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Settings/Game Settings",  fileName = "New GameSettings")]
public class GameSettings : ScriptableObject
{
    public MapManager MapManager;
}

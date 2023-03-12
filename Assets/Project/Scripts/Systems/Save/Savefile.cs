using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Savefile", fileName = "New Savefile")]
public class Savefile : ScriptableObject
{
	public Vector3 PlayerPos;
    public scr_MapAsset Map;
}
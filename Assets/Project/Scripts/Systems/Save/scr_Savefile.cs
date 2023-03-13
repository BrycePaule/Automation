using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Savefile", fileName = "New Savefile")]
public class scr_Savefile : ScriptableObject
{
	public Vector3Int PlayerCellPos;
    public scr_MapAsset MapAsset;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptables/Building Asset", fileName = "New Building Asset")]
public class scr_BuildingAsset : ScriptableObject
{
    [Header("Settings")]
    public int ID;
    public BuildingType BuildingType;

    [Header("Display")]
    public string DisplayName;
    public string Description;
    public Sprite WorldSprite;
    public Sprite InventoryIcon;

    [Header("Object")]
    public GameObject Prefab;
}

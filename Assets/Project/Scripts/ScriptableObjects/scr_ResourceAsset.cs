using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptables/Resource Asset", fileName = "New Resource Asset")]
public class scr_ResourceAsset : ScriptableObject
{
    public int ID;
    public string DisplayName;

    public Sprite Sprite;

    public ResourceType ResourceType;
}

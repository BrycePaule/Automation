using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Scriptables/Resource Library", fileName = "New Resource Library")]
public class ResourceLibrary : ScriptableObject
{
    public scr_ResourceAsset[] Resources;

    // LOOKUPS
    public scr_ResourceAsset GetByID(int id)
    {
        foreach (var resource in Resources)
        {
            if (resource.ID == id) { return resource; }
        }

        Debug.LogError("ResourceLibrary attempted to find resource of ID: " + id + ", no match.");
        return null;
    }

    public scr_ResourceAsset GetByType(ResourceType type)
    {
        foreach (var resource in Resources)
        {
            if (resource.ResourceType == type) { return resource; }
        }

        Debug.LogError("ResourceLibrary attempted to find resource of Type: " + type + ", no match.");
        return null;
    }
}

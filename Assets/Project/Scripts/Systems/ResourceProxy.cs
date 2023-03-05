using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProxy : Singleton<ResourceProxy>
{
    [SerializeField] private scr_ResourceLibrary library;
    [SerializeField] private GameObject baseItemPrefab;

    public scr_ResourceAsset GetByID(int id)
    {
        return library.GetByID(id);
    }

    public scr_ResourceAsset GetByType(ResourceType type)
    {
        return library.GetByType(type);
    }

    public GameObject InstantiateByType(ResourceType type)
    {
        if (type == ResourceType.UNASSIGNED) { Debug.LogError("ResourceProxy failed to Instatiate resource of Type: " + ResourceType.UNASSIGNED); }
        if (type == ResourceType.Any) { Debug.LogError("ResourceProxy failed to Instatiate resource of Type: " + ResourceType.Any); }

        GameObject resourceObject = Instantiate(baseItemPrefab, Vector3.zero, Quaternion.identity);
        resourceObject.GetComponent<Resource>().OverrideDefaultValues(type);

        return resourceObject;
    }
}

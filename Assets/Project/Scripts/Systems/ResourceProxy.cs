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
        scr_ResourceAsset asset = GetByType(type);
        GameObject resourceObject = Instantiate(baseItemPrefab, Vector3.zero, Quaternion.identity);

        resourceObject.GetComponent<Resource>().SetResource(type);

        return resourceObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProxy : Singleton<ResourceProxy>
{

    [SerializeField] private ResourceLibrary library;

    public scr_ResourceAsset GetByID(int id)
    {
        return library.GetByID(id);
    }

    public scr_ResourceAsset GetByType(ResourceType type)
    {
        return library.GetByType(type);
    }
}

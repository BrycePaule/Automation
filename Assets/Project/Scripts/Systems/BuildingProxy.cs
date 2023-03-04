using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProxy : Singleton<BuildingProxy>
{
    [SerializeField] private scr_BuildingLibrary library;

    public scr_BuildingAsset GetByID(int id)
    {
        return library.GetByID(id);
    }

    public scr_BuildingAsset GetByType(BuildingType type)
    {
        return library.GetByType(type);
    }

    public GameObject InstantiateByType(BuildingType type)
    {
        scr_BuildingAsset asset = GetByType(type);

        return Instantiate(asset.Prefab, Vector3.zero, Quaternion.identity);
    }
}

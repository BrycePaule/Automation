using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Scriptables/Building Library", fileName = "New Building Library")]
    public class scr_BuildingLibrary : ScriptableObject
    {
        public scr_BuildingAsset[] Buildings;

        // LOOKUPS
        public scr_BuildingAsset GetByID(int id)
        {
            foreach (var building in Buildings)
            {
                if (building.ID == id) { return building; }
            }

            Debug.LogError("BuildingLibrary attempted to find building of ID: " + id + ", no match.");
            return null;
        }

        public scr_BuildingAsset GetByType(BuildingType type)
        {
            foreach (var building in Buildings)
            {
                if (building.BuildingType == type) { return building; }
            }

            Debug.LogError("BuildingLibrary attempted to find building of Type: " + type + ", no match.");
            return null;
        }
    }
}
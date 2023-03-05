using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingGhost : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = transform.GetComponent<SpriteRenderer>();
    }

    public void OnBuildingSelected(BuildingType selected)
    {
        if (selected == BuildingType.UNASSIGNED)
        {
            sr.enabled = false;
            sr.sprite = null;
        }
        else
        {
            sr.enabled = true;
            sr.sprite = BuildingProxy.Instance.GetByType(selected).WorldSprite;
        }
    }
}

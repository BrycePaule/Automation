using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = transform.GetComponent<SpriteRenderer>();
    }

    public void SetResource(ResourceType type)
    {
        resourceType = type;

        scr_ResourceAsset asset = ResourceProxy.Instance.GetByType(type);

        transform.name = asset.DisplayName;
        sr.sprite = asset.Sprite;
    }
}

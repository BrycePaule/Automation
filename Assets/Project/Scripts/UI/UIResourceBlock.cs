using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResourceBlock : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;

    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    private int resourceCount;

    private void Start()
    {
        resourceCount = 0;

        RefreshResourceType();
        UpdateText();
    }

    public void OnResourcePickUp(ResourceType eventItemType)
    {
        if (eventItemType != resourceType) { return;}

        resourceCount += 1;
        UpdateText();
    }

    private void UpdateText()
    {
        countText.text = resourceCount.ToString();
    }

    private void RefreshResourceType()
    {
        if (resourceType == ResourceType.UNASSIGNED) { return; }

        icon.sprite = ResourceProxy.Instance.GetByType(resourceType).Sprite;
    }
}

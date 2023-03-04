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

    private void Awake()
    {
        RefreshType();

        resourceCount = 0;
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

    private void RefreshType()
    {
        icon.sprite = ResourceProxy.Instance.GetByType(resourceType).Sprite;
    }
}

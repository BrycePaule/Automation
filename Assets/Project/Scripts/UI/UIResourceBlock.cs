using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResourceBlock : MonoBehaviour
{

    [SerializeField] private ResourceType itemType;

    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemCountText;

    private int itemCount;

    private void Awake()
    {
        itemCount = 0;
        UpdateText();
    }

    public void OnResourcePickUp(ResourceType eventItemType)
    {
        if (eventItemType != itemType) { return;}

        itemCount += 1;
        UpdateText();
    }

    private void UpdateText()
    {
        itemCountText.text = itemCount.ToString();
    }
}

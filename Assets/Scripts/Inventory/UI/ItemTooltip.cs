using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;

    [SerializeField] private GameObject bottomPart;

    public void SetupTooltip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;

        if(itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            var price = itemDetails.itemPrice;
            if(slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }

            valueText.text = price.ToString();
            bottomPart.gameObject.SetActive(true);
        }
        else
        {
            bottomPart.gameObject.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "����",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            ItemType.HoeTool => "����",
            ItemType.ChopTool => "����",
            ItemType.BreakTool => "����",
            ItemType.ReapTool => "����",
            ItemType.WaterTool => "����",
            ItemType.CollectTool => "����",
            ItemType.ReapableScebery => "����",
            _ => "��"
        };
    }
}

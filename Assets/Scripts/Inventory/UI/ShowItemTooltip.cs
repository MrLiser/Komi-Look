using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mfarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        private SlotUI slotUI;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(slotUI.itemAmount != 0)
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);
                inventoryUI.itemTooltip.transform.position = transform.position + Vector3.up * 60;
            }
            else
            {
                inventoryUI.itemTooltip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        


    }
}


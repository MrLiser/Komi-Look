﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Mfarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image slotHightlight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;
        public ItemDetails itemDetails;
        public int itemAmount;


        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

  
        /// <summary>
        /// 更新格子UI信息
        /// </summary>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            itemAmount = amount;
            slotImage.sprite = item.itemIcon;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;

        }

        /// <summary>
        /// 将格子滞空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0)
                return;
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHightlight(slotIndex);

            if(slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
             if(itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;

            if(eventData.pointerCurrentRaycast.gameObject != null)
            {
                if(eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                {
                    return;
                }
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;

                if(slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                inventoryUI.UpdateSlotHightlight(-1);
            }
            else //扔在地上
            {
                if(itemDetails.canDropped)
                {
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
                }
               
            }
        }


    }
}


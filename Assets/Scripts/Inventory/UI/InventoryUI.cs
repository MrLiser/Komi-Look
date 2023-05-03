using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mfarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("拖拽物体")]
        public Image dragItem;
        [Header("玩家背包")]
        [SerializeField] private GameObject bagUI;

        private bool bagOpend;

        [SerializeField] private SlotUI[] playerSlots;

        [SerializeField] public ItemTooltip itemTooltip;

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforSceneUnLoadEvent += OnBeforSceneUnLoadEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforSceneUnLoadEvent -= OnBeforSceneUnLoadEvent;
        }

        private void Start()
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpend = bagUI.activeInHierarchy;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnBeforSceneUnLoadEvent()
        {
            UpdateSlotHightlight(-1);
        }
        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch(location)
            {
                case InventoryLocation.Player:
                    {
                        for (int i = 0; i < playerSlots.Length; i++)
                        {
                            if(list[i].itemAmount > 0)
                            {
                                var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                                playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                            }
                            else
                            {
                                playerSlots[i].UpdateEmptySlot();
                            }
                        }
                        break;
                    }
                case InventoryLocation.Box:
                    {
                        break;
                    }
            }
        }

        public void OpenBagUI()
        {
            bagOpend = !bagOpend;
            bagUI.gameObject.SetActive(bagOpend);
        }

        public void UpdateSlotHightlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHightlight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightlight.gameObject.SetActive(false);
                }
            }
        }
    }
}


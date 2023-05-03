using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mfarm.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();

            if(item != null)
            {
                if(item.itemDetails.canPickedup)
                {
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }
    }
}


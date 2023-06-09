using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mfarm.Inventory
{
    [System.Serializable]
    public class Item : MonoBehaviour
    {
        public int ItemID;

        private SpriteRenderer spriteRenderer;

        private BoxCollider2D coll;

        public ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if(ItemID != 0)
            {
                Init(ItemID);
            }
        }
        public void Init(int ID)
        {
            ItemID = ID;

            itemDetails = InventoryManager.Instance.GetItemDetails(ItemID);

            if(itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.x);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.bounds.center.y);
            }
        }
    }
}


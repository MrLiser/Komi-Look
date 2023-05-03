using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mfarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;

        private Transform itemParent;

        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforSceneUnLoadEvent += OnBeforSceneUnLoadEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforSceneUnLoadEvent -= OnBeforSceneUnLoadEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        }

        private void OnBeforSceneUnLoadEvent()
        {
            GetAllSceneItems();
        }

        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItem();
        }

        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.ItemID = ID;
        }

        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.ItemID,
                    position = new SerializableVector3(item.transform.position)

                };
                currentSceneItems.Add(sceneItem);
            }
            string key = SceneManager.GetActiveScene().name;
            if (sceneItemDict.ContainsKey(key))
            {
                sceneItemDict[key] = currentSceneItems;
            }
            else
            {
                sceneItemDict.Add(key, currentSceneItems);
            }
        }

        private void RecreateAllItem()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            string key = SceneManager.GetActiveScene().name;

            if (sceneItemDict.TryGetValue(key, out currentSceneItems))
            {
                if(currentSceneItems != null)
                {
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab,item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }

    }
}


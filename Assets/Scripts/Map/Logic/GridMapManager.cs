using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mfarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("µÿÕº–≈œ¢")]

        public List<MapData_SO> mapDataList;

        public Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

        private Grid currentGrid;

        private void OnEnable()
        {
            EventHandler.ExcuteActionAfterAnimation += OnExcuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        }

        private void OnDisable()
        {
            EventHandler.ExcuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        }

        private void Start()
        {
            foreach (var item in mapDataList)
            {
                InitTileDetailsDict(item);
            }
        }
        private void InitTileDetailsDict(MapData_SO mapData_SO)
        {
            foreach (var item in mapData_SO.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    girdX = item.tileCoordinate.x,
                    girdY = item.tileCoordinate.y
                };

                string key = tileDetails.girdX + "x" + tileDetails.girdY + "y" + mapData_SO.sceneName;

                if (GetTileDetails(key) != null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch (item.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = item.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = item.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.canPLaceFurniture = item.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.isNPCObstacle = item.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key) != null)
                    tileDetailsDict[key] = tileDetails;
                else
                    tileDetailsDict.Add(key, tileDetails);
            }
        }

        private TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            return null;
        }

        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;

            return GetTileDetails(key);
        }
       

        private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if(currentTile != null)
            {
                switch(itemDetails.itemType)
                {
                    case ItemType.Commodity:
                        EventHandler.CallInstantiateItemInScene(itemDetails.itemID, mouseWorldPos);
                        break;
                }
            }
        }
        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
        }
    } 
}
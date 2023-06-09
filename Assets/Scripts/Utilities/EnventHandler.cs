using System;
using System.Collections.Generic;
using UnityEngine;
public static class EventHandler 
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;

    public static void CallInstantiateItemInScene(int ID, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<int, int> GameMinuteEvent;

    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int, int, int, int, Season> GameDataEvent;

    public static void CallGameDataEvent(int hour, int day, int month, int year, Season season)
    {
        GameDataEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action<string, Vector3> TransitonEvent;

    public static void CallTransitonEvent(string sceneName, Vector3 targetPositon)
    {
        TransitonEvent?.Invoke(sceneName, targetPositon);
    }

    public static event Action BeforSceneUnLoadEvent;

    public static void CallBeforSceneUnLoadEvent()
    {
        BeforSceneUnLoadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadedEvent;

    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPositon;

    public static void CallMoveToPositon(Vector3 tartgetPosition)
    {
        MoveToPositon?.Invoke(tartgetPosition);
    }

    public static event Action<Vector3, ItemDetails> MouseClickEvent;

    public static void CallMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickEvent?.Invoke(pos, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> ExcuteActionAfterAnimation;

    public static void CallExcuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        ExcuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }
}

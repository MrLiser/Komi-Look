using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mfarm.Map;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;

    private Sprite currentSprite;

    private Image cursorImage;

    private RectTransform cursorCanvas;

    private Camera mainCamera;

    private Grid currentGrid;

    private Vector3 mouseWorldPos;

    private Vector3Int mouseGridPos;

    private bool cursorEnable;

    private bool cursorPositionValid;

    private ItemDetails currentItem;

    private Transform PlayerTransform => FindObjectOfType<Player>().transform;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforSceneUnLoadEvent += OnBeforSceneUnLoadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforSceneUnLoadEvent -= OnBeforSceneUnLoadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();

        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();

        currentSprite = normal;
        SetCursorImage(normal);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null) return;
        cursorImage.transform.position = Input.mousePosition;
        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }

    }

    private void CheckPlayerInput()
    {
        if(Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            EventHandler.CallMouseClickEvent(mouseWorldPos, currentItem);
        }
    }
    private void OnBeforSceneUnLoadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = Vector4.one;
    }
    /// <summary>
    /// 设置鼠标可用
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = Vector4.one;
    }

    /// <summary>
    /// 设置鼠标不可用
    /// </summary>
    private void SetCursorInValid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {

        if (isSelected)
        {
            currentItem = itemDetails;
            cursorEnable = true;
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                _ => normal
            };
        }
        else
        {
            currentSprite = normal;
            cursorEnable = false;
            currentItem = null;
        }

    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);

        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius
        || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }

        var currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            switch (currentItem.itemType)
            {
                case ItemType.Commodity:
                    if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid(); else SetCursorInValid();
                    break;
            }
        }
        else
        {
            SetCursorInValid();
        }
    }
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

}

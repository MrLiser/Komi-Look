using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    [Header("各部分动画列表")]
    public List<AnimatorType> animatorTypes;

    public Dictionary<string, Animator> animatorNameDic = new Dictionary<string, Animator>();

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            animatorNameDic.Add(anim.name, anim);
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforSceneUnLoadEvent += OnBeforSceneUnLoadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforSceneUnLoadEvent -= OnBeforSceneUnLoadEvent;
    }

    private void OnBeforSceneUnLoadEvent()
    {
        SwitchAnimator(PartType.None);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        PartType partType = itemDetails.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            _ => PartType.None
        };
        if (!isSelected)
        {
            partType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            holdItem.sprite = itemDetails.itemOnWorldSprite;
            holdItem.enabled = true;
        }
        SwitchAnimator(partType);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach (var anim in animatorTypes)
        {
            if(anim.partType == partType)
            {
                animatorNameDic[anim.partName.ToString()].runtimeAnimatorController = anim.overrideController;
            }
        }
    }
}

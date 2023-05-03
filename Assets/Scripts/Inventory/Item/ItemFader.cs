using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    
    public void FadeIn()
    {
        Color targetColor = Vector4.one;
        spriteRenderer.DOColor(targetColor, Settings.fadeDuration);
    }

    
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRenderer.DOColor(targetColor, Settings.fadeDuration);
    }
}

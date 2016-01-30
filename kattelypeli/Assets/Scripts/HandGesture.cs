using UnityEngine;
using System.Collections.Generic;

public class HandGesture : MonoBehaviour
{
    public SpriteRenderer handSpriteRenderer;

    private int currentSpriteIndex = -1;
    public int CurrentSpriteIndex
    {
        get { return currentSpriteIndex; }
    }

    void Awake()
    {
        if (handSpriteRenderer == null)
            handSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Disable()
    {
        currentSpriteIndex = -1;
        handSpriteRenderer.enabled = false;
    }

    public void SetSprite(int _index, Sprite _sprite)
    {
        handSpriteRenderer.enabled = true;
        currentSpriteIndex = _index;
        handSpriteRenderer.sprite = _sprite;
    }
}

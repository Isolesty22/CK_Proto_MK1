using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorCursor : MonoBehaviour
{
    public Image image;
    public RectTransform rect;

    [ReadOnly]
    public int currentIndex;

    [Tooltip("Awake¶§ °Ù ÄÄÆ÷³ÍÆ®¸¦ ÇÕ´Ï´Ù.")]
    public bool getComponentOnAwake;

    private void Awake()
    {
        if (getComponentOnAwake)
        {
            image = GetComponent<Image>();
            rect = GetComponent<RectTransform>();
        }
    }

    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

    public void SetSprite(Sprite _sprite)
    {
        image.sprite = _sprite;
    }
}

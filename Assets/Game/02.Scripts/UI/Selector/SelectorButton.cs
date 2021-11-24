using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectorButton : MonoBehaviour, IPointerEnterHandler
{
    private SelectorController controller;

    [ReadOnly]
    public int index;

    [Tooltip("Awake¶§ °Ù ÄÄÆ÷³ÍÆ®¸¦ ÇÕ´Ï´Ù.")]
    public bool getComponentOnAwake;

    [SerializeField]
    public Button button
    {
        get;
        private set;
    }
    public RectTransform rect
    {
        get;
        private set;
    }
    public void SetController(SelectorController _controller)
    {
        controller = _controller;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.SelectButton(this);
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    ((IPointerEnterHandler)button).OnPointerEnter(eventData);
    //}

    private void Awake()
    {
        if (getComponentOnAwake)
        {
            rect = GetComponent<RectTransform>();
            button = GetComponent<Button>();
        }
    }

}

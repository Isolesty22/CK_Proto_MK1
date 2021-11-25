using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndingCredit : UIBase
{
    public RectTransform rectTransform;
    public UIMovieScreen movieScreen;
    [Header("흘러가는 시간")]
    public float moveTime;
    private Vector2 startPos;
    private Vector2 endPos;
    private void Start()
    {
        UIManager.Instance.AddDict(this);
        startPos = rectTransform.anchoredPosition;
        endPos = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y);
        movieScreen.Play();

    }


    float timer = 0f;
    float progress = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        progress = timer / moveTime;

        rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
    }
}

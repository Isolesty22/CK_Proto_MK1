using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEndingCredit : UIBase
{
    public RectTransform rectTransform;
    public UIMovieScreen movieScreen;
    [Header("흘러가는 시간")]
    public float moveTime;
    private Vector2 startPos;
    private Vector2 endPos;

    /// <summary>
    /// 크레딧이 끝나면 호출됩니다.
    /// </summary>
    public event Action onCreditMoveEnded = null;
    private void Awake()
    {
        startPos = rectTransform.anchoredPosition;
        endPos = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y);
    }
    private void Start()
    {
        UIManager.Instance.AddDict(this);
    }


    float timer = 0f;
    float progress = 0f;

    public IEnumerator Up()
    {
        timer = 0f;
        progress = 0f;

        while (progress < 1f)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                timer += Time.deltaTime * 3f;

            }
            else
            {

                timer += Time.deltaTime;
            }
            progress = timer / moveTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);

            yield return null;
        }

        onCreditMoveEnded?.Invoke();
    }
}

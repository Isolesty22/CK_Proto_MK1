using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{

    public Image backgroundImage;
    public Image loadingBarImage;



    #region Joke
    public RectTransform ipiaTransform;

    public float startPosY;

    public void CalcStartPosY()
    {
        startPosY = 0f - ipiaTransform.rect.height;
    }
    #endregion

    private void Start()
    {
        Init();
        //RegisterUIManager();
    }

    public override void Init()
    {
        CheckOpen();
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        Com.canvas.enabled = true;
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        Com.canvas.enabled = true;
        StartCoroutine(ProcessClose());
        return true;
    }

    private float fadeDuration = 0.5f;
    private IEnumerator ProcessOpen()
    {
        isOpen = false;

        float progress = 0f;
        float timer = 0f;

        //알파값 0으로 변경
        Com.canvasGroup.alpha = 0f;

        //페이드 인
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;
        isOpen = Com.canvas.enabled = true;
        yield break;
    }

    private IEnumerator ProcessClose()
    {
        isOpen = true;
      //  yield return new WaitUntil(() =>SceneChanger.Instance.);
        float progress = 0f;
        float timer = 0f;

        //알파값 1로 변경
        Com.canvasGroup.alpha = 1f;

        //페이드 아웃
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 0f;

        isOpen = Com.canvas.enabled = false;

        yield break;
    }
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}

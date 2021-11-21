using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMessage : UIBase
{
    public Text uiText;
    private string currentText;

    private WaitForSeconds waitSec;
    private IEnumerator open;
    private IEnumerator close;
    private IEnumerator openUseDuration;


    private float progress = 0f;
    private float timer = 0f;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        UIManager.Instance.AddDict(this);
    }
    public override void Init()
    {
        Com.canvas.enabled = false;
        CheckOpen();

        fadeDuration = 0.3f;
        waitSec = new WaitForSeconds(1f);

        open = ProcessOpen();
        close = ProcessClose();
        openUseDuration = ProcessOpenUseDuration();
    }

    public void SetWaitTime(float _waitTime)
    {
        waitSec = new WaitForSeconds(_waitTime);
    }
    public override bool Open()
    {
        if (openUseDuration != null)
        {
            StopCoroutine(openUseDuration);
            ClearTimer();
        }
        openUseDuration = ProcessOpenUseDuration();
        uiText.text = currentText;
        StartCoroutine(openUseDuration);
        return true;
    }
    public bool Open(string _string)
    {
        if (openUseDuration != null)
        {
            StopCoroutine(openUseDuration);
            ClearTimer();
        }
        openUseDuration = ProcessOpenUseDuration();
        currentText = _string;
        uiText.text = currentText;
        StartCoroutine(openUseDuration);
        return true;
    }

    public void SetText(string _string)
    {
        currentText = _string;
    }

    public override bool Close()
    {

        if (!isOpen)
        {
            return false;
        }

        close = ProcessClose();
        StartCoroutine(close);
        return true;
    }

    private void ClearTimer()
    {
        progress = 0f;
        timer = 0f;
    }

    private IEnumerator ProcessOpenUseDuration()
    {
        if (isOpen)
        {
            //ClearTimer();
            //uiText.text = currentText;
            //yield break;
            StopCoroutine(open);
            StopCoroutine(close);
        }
        open = ProcessOpen();
        close = ProcessClose();

        ClearTimer();
        yield return StartCoroutine(open);

        yield return waitSec;

        ClearTimer();
        yield return StartCoroutine(close);

        openUseDuration = null;
    }


    protected override IEnumerator ProcessOpen()
    {
        isOpen = true;


        ClearTimer();
        //알파값 0으로 변경
        Com.canvasGroup.alpha = 0f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 인
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;



        yield break;
    }

    protected override IEnumerator ProcessClose()
    {
        ClearTimer();
        //  yield return new WaitUntil(() =>SceneChanger.Instance.);

        //알파값 1로 변경
        Com.canvasGroup.alpha = 1f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 아웃
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 0f;
        Com.canvas.enabled = false;

        isOpen = false;

        yield break;
    }
}

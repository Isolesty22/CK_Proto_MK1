using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalk : UIBase
{
    [Header("UITalk")]
    public Text uiText;

    [Header("현재 대화 코드")]
    public int currentCode;
    private string currentText;
    private float duration;

    private List<Dictionary<string, object>> talkData = new List<Dictionary<string, object>>();

    private WaitForSeconds waitSec = new WaitForSeconds(1f);
    private IEnumerator open = null;
    private IEnumerator close = null;
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }

    public override void Init()
    {
        fadeDuration = 0.3f;
        waitSec = new WaitForSeconds(1f);

        open = ProcessOpen();
        close = ProcessClose();
    }

    /// <summary>
    /// UI상의 텍스트를 변경합니다.
    /// </summary>
    /// <param name="_text"></param>
    public void UpdateText(string _text)
    {
        uiText.text = _text;
    }
    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        return true;
    }

    private const string str_NAEYONG = "NAEYONG";
    private const string str_CODE = "NAEYONG";
    /// <summary>
    /// CODE에 따른 텍스트를 불러오고, UI의 지속시간을 설정합니다. 
    /// </summary>
    public void SetValue(int _CODE, float _duration)
    {
        currentCode = (int)talkData[_CODE][str_CODE];
        currentText = talkData[_CODE][str_NAEYONG] as string;
        waitSec = new WaitForSeconds(_duration);
    }

    /// <summary>
    /// CODE에 따른 텍스트를 불러옵니다.
    /// </summary>
    public void SetValue(int _CODE)
    {
        currentCode = (int)talkData[_CODE][str_CODE];
        currentText = talkData[_CODE][str_NAEYONG] as string;
    }

    public void SetTalkData(List<Dictionary<string, object>> _talkData)
    {
        talkData = null;
        talkData = _talkData;
    }


    /// <summary>
    /// 특정 초 이후 자동으로 닫힙니다.
    /// </summary>
    public IEnumerator ProcessOpenUseDuration()
    {
        if (isOpen)
        {
            StopCoroutine(open);
            StopCoroutine(close);
            open = ProcessOpen();
            close = ProcessClose();
        }

        yield return StartCoroutine(open);
        yield return waitSec;
        yield return StartCoroutine(close);
    }
    protected override IEnumerator ProcessOpen()
    {
        isOpen = true;

        float progress = 0f;
        float timer = 0f;

        //알파값 0으로 변경
        Com.canvasGroup.alpha = 0f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 인
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;
        yield break;
    }

    protected override IEnumerator ProcessClose()
    {
        //  yield return new WaitUntil(() =>SceneChanger.Instance.);
        float progress = 0f;
        float timer = 0f;

        //알파값 1로 변경
        Com.canvasGroup.alpha = 1f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 아웃
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 0f;
        Com.canvas.enabled = false;

        isOpen = false;
    }
}

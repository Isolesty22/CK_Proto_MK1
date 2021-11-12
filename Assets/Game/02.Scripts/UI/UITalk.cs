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
    [Tooltip("몇 번대인가!")]
    public int stageCode;
    [Tooltip("실제로 텍스트 출력에 사용되어지고 있는 코드")]
    private int realCode;

    private string currentText;

    private List<Dictionary<string, object>> talkData = new List<Dictionary<string, object>>();

    private WaitForSeconds waitSec = new WaitForSeconds(2f);
    private IEnumerator open = null;
    private IEnumerator close = null;
    private IEnumerator openUseDuration = null;
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }

    public override void Init()
    {
        Com.canvas.enabled = false;
        CheckOpen();

        fadeDuration = 0.3f;
        waitSec = new WaitForSeconds(2f);

        open = ProcessOpen();
        close = ProcessClose();
        openUseDuration = ProcessOpenUseDuration();
    }

    ///// <summary>
    ///// UI상의 텍스트를 변경합니다.
    ///// </summary>
    ///// <param name="_text"></param>
    //public void UpdateText(string _text)
    //{
    //    uiText.text = _text;
    //}
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
    private const string str_CODE = "CODE";
    /// <summary>
    /// CODE에 따른 텍스트를 불러오고, UI의 지속시간을 설정합니다. 
    /// </summary>
    public void SetValue(int _CODE, float _duration)
    {
        SetValue(_CODE);
        waitSec = new WaitForSeconds(_duration);
    }

    /// <summary>
    /// CODE에 따른 텍스트를 불러옵니다.
    /// </summary>
    public void SetValue(int _CODE)
    {


        if (_CODE - stageCode > talkData.Count)
        {
            Debug.Log("해당 코드는 데이터의 범위를 넘어섭니다.");
            return;
        }

        realCode = _CODE - stageCode;

        currentCode = (int)talkData[realCode][str_CODE];
        currentText = talkData[realCode][str_NAEYONG] as string;
    }

    public void SetTalkData(List<Dictionary<string, object>> _talkData)
    {
        talkData = null;
        talkData = _talkData;
    }


    public void StartTalk()
    {
        if (openUseDuration != null)
        {
            StopCoroutine(openUseDuration);
        }

        openUseDuration = ProcessOpenUseDuration();
        StartCoroutine(openUseDuration);
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
        }

        open = ProcessOpen();
        close = ProcessClose();
        yield return StartCoroutine(open);
        yield return waitSec;
        yield return StartCoroutine(close);

        openUseDuration = null;
    }
    protected override IEnumerator ProcessOpen()
    {
        isOpen = true;

        float progress = 0f;
        float timer = 0f;


        //알파값 0으로 변경
        Com.canvasGroup.alpha = 0f;

        uiText.text = currentText;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalk : UIBase
{
    //-----인스펙터
    [Header("UITalk")]
    public Text uiText;
    [Header("현재 대화 코드"), ReadOnly]
    public int currentCode;
    /// <summary>
    /// 몇 번대인가? csv 파일 내 CODE의 일련번호입니다.
    /// </summary>
    public int stageCode
    {
        get
        {
            return DataManager.Instance.stageCode;
        }

    }
    //-----



    //-----코루틴 관련
    private WaitForSeconds waitSec = new WaitForSeconds(2f);
    private IEnumerator open = null;
    private IEnumerator close = null;
    private IEnumerator openUseDuration = null;
    //-----

    [Tooltip("실제로 텍스트 출력에 사용되어지고 있는 코드")]
    private int realCode;
    private string currentText;

    private float progress = 0f;
    private float timer = 0f;

    //private List<Dictionary<string, object>> talkData = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> talkData
    {
        get { return DataManager.Instance.loadData_Talk_Result; }

    }

    private const string str_NAEYONG = "NAEYONG";
    private const string str_CODE = "CODE";

    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);


        //UIManager.Instance.uiTalk.SetTalkData(loadData_Talk_Result);
        //UIManager.Instance.uiTalk.stageCode = stageCode;
    }
    public override void Init()
    {
        Com.canvas.enabled = false;
        CheckOpen();

        fadeDuration = 0.3f;
        waitSec = new WaitForSeconds(3f);

        open = ProcessOpen();
        close = ProcessClose();
        openUseDuration = ProcessOpenUseDuration();
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

    private void ClearTimer()
    {

        Debug.Log("Clear Talk Timer!");
        progress = 0f;
        timer = 0f;
    }

    /// <summary>
    /// CODE에 따른 텍스트를 불러오고, UI의 지속시간을 설정합니다. 
    /// </summary>
    public void SetValue(int _CODE, float _duration)
    {
        if (_CODE - stageCode >= talkData.Count)
        {
            Debug.Log("해당 코드는 데이터의 범위를 넘어섭니다. 이전 텍스트를 출력합니다.");
        }
        else
        {
            realCode = _CODE - stageCode;
        }

        //Debug.Log(realCode);
        currentCode = (int)talkData[realCode][str_CODE];
        currentText = talkData[realCode][str_NAEYONG] as string;
        waitSec = new WaitForSeconds(_duration);
    }


    public void SetValue(float _duration)
    {
        waitSec = new WaitForSeconds(_duration);
    }

    public void SetText_Test(string _text)
    {
        currentText = _text;
    }

    /// <summary>
    /// CODE에 따른 텍스트를 불러옵니다.
    /// </summary>
    public void SetValue(int _CODE)
    {
        if (_CODE - stageCode >= talkData.Count)
        {
            Debug.Log("해당 코드는 데이터의 범위를 넘어섭니다. 이전 텍스트를 출력합니다.");
        }
        else
        {
            realCode = _CODE - stageCode;
        }
        Debug.Log(realCode);
        currentCode = (int)talkData[realCode][str_CODE];
        currentText = talkData[realCode][str_NAEYONG] as string;
    }

    /// <summary>
    /// 지속시간이 없는 상태로 그냥 열기만 합니다.<see cref="EndTalk"/>를 호출하면 창을 닫을 수 있습니다.
    /// </summary>
    public void StartTalkInfinity()
    {
        if (isOpen)
        {
            if (openUseDuration != null)
            {
                StopCoroutine(openUseDuration);
            }
            StopCoroutine(open);
            StopCoroutine(close);
        }
        ClearTimer();
        open = ProcessOpen();
        StartCoroutine(open);
    }
    public void StartTalk()
    {
        if (isOpen)
        {

            StopCoroutine(open);
            StopCoroutine(close);
        }
        if (openUseDuration != null)
        {
            StopCoroutine(openUseDuration);
        }


        openUseDuration = ProcessOpenUseDuration();
        StartCoroutine(openUseDuration);
    }

    /// <summary>
    /// 대화창을 닫습니다.
    /// </summary>
    public void EndTalk()
    {
        if (isOpen)
        {
            StopCoroutine(open);
            StopCoroutine(close);

            if (openUseDuration != null)
            {
                StopCoroutine(openUseDuration);
            }

            close = ProcessClose();
            StartCoroutine(close);
        }
    }
    /// <summary>
    /// 특정 초 이후 자동으로 닫힙니다.
    /// </summary>
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


        progress = 0f;
        timer = 0f;

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

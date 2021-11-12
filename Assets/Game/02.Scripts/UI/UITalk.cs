using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalk : UIBase
{
    [Header("UITalk")]
    public Text uiText;

    [Header("���� ��ȭ �ڵ�")]
    public int currentCode;
    [Tooltip("�� �����ΰ�!")]
    public int stageCode;
    [Tooltip("������ �ؽ�Ʈ ��¿� ���Ǿ����� �ִ� �ڵ�")]
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
    ///// UI���� �ؽ�Ʈ�� �����մϴ�.
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
    /// CODE�� ���� �ؽ�Ʈ�� �ҷ�����, UI�� ���ӽð��� �����մϴ�. 
    /// </summary>
    public void SetValue(int _CODE, float _duration)
    {
        SetValue(_CODE);
        waitSec = new WaitForSeconds(_duration);
    }

    /// <summary>
    /// CODE�� ���� �ؽ�Ʈ�� �ҷ��ɴϴ�.
    /// </summary>
    public void SetValue(int _CODE)
    {


        if (_CODE - stageCode > talkData.Count)
        {
            Debug.Log("�ش� �ڵ�� �������� ������ �Ѿ�ϴ�.");
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
    /// Ư�� �� ���� �ڵ����� �����ϴ�.
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


        //���İ� 0���� ����
        Com.canvasGroup.alpha = 0f;

        uiText.text = currentText;

        //ĵ���� Ȱ��ȭ
        Com.canvas.enabled = true;

        //���̵� ��
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

        //���İ� 1�� ����
        Com.canvasGroup.alpha = 1f;

        //ĵ���� Ȱ��ȭ
        Com.canvas.enabled = true;

        //���̵� �ƿ�
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

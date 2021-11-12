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
    /// UI���� �ؽ�Ʈ�� �����մϴ�.
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
    /// CODE�� ���� �ؽ�Ʈ�� �ҷ�����, UI�� ���ӽð��� �����մϴ�. 
    /// </summary>
    public void SetValue(int _CODE, float _duration)
    {
        currentCode = (int)talkData[_CODE][str_CODE];
        currentText = talkData[_CODE][str_NAEYONG] as string;
        waitSec = new WaitForSeconds(_duration);
    }

    /// <summary>
    /// CODE�� ���� �ؽ�Ʈ�� �ҷ��ɴϴ�.
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
    /// Ư�� �� ���� �ڵ����� �����ϴ�.
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

        //���İ� 0���� ����
        Com.canvasGroup.alpha = 0f;

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

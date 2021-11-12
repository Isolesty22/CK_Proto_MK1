using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalk : UIBase
{
    [Header("UITalk")]
    public Text uiText;
    
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }

    public override void Init()
    {

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

        yield break;
    }
}

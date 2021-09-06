using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : UIBase
{

    void Start()
    {
        Init();
        RegisterUIManager();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
            {
                UIManager.Instance.CloseTop();

            }
            else
            {
                UIManager.Instance.OpenThis(this);
            }

        }
    }
    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        Time.timeScale = 0f;
        return true;
    }


    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        Time.timeScale = 1f;
        return true;
    }

    public void Button_OpenSettings(UIBase _uiBase)
    {
        UIManager.Instance.OpenThis(_uiBase);
    }

    public void Button_ReturnGame()
    {
        UIManager.Instance.CloseTop();
    }
    public void Button_ReturnMain()
    {
        SceneChanger.Instance.LoadThisScene("TestHomeScene");
        Time.timeScale = 1f;
    }

    public void Button_QuitGame(UIPopup _uiBase)
    {
        UIManager.Instance.OpenThis(_uiBase);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    protected override IEnumerator ProcessOpen()
    {
        //ó������ �����ִ� ����(�����Է¹���)
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
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;

        yield break;
    }

}

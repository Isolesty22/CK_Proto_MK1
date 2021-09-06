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
        //처음부터 열려있는 판정(연속입력방지)
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
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;

        yield break;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : UIBase
{
    public GameObject blurVolume;
    void Start()
    {
        Init();
        blurVolume.SetActive(false);
        UIManager.Instance.AddDict(this);
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (UIManager.Instance.openUIcount == 0)
    //        {

    //            UIManager.Instance.OpenThis(this);
    //        }
    //        else
    //        {
    //            UIManager.Instance.CloseTop();
    //        }

    //    }
    //}

    public override bool Open()
    {
        blurVolume.SetActive(true);
        StartCoroutine(ProcessOpen());
        Time.timeScale = 0f;
        AudioManager.Instance.Pause();
        return true;
    }


    public override bool Close()
    {
        blurVolume.SetActive(false);
        StartCoroutine(ProcessClose());
        Time.timeScale = 1f;
        AudioManager.Instance.UnPause();
        return true;
    }

    public void Button_CloseTop()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.CloseTop();
    }
    public void Button_OpenThis(string _name)
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenThis(_name);
    }
    public void Button_ReturnFieldMap()
    {
        UIManager.Instance.PlayAudio_Click();
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        Com.canvasGroup.interactable = false;
        Time.timeScale = 1f;
        AudioManager.Instance.UnPause();
    }
    public void Button_QuitGame()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenQuitPopup();
    }

    public void Button_Restart()
    {
        UIManager.Instance.PlayAudio_Click();
        SceneChanger.Instance.LoadThisScene(SceneChanger.Instance.GetNowSceneName());
        Com.canvasGroup.interactable = false;
        Time.timeScale = 1f;
        AudioManager.Instance.UnPause();
    }

    public void Button_EndTutorial()
    {
        UIManager.Instance.PlayAudio_Click();
        GameManager.instance.EndTutorial();

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

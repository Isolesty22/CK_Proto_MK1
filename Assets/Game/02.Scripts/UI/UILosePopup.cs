using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILosePopup : UIBase
{
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }

    public override void Init()
    {
        CheckOpen();
    }
    public override bool Open()
    {
        Time.timeScale = 0f;
        StartCoroutine(ProcessOpen());
        return true;

        //Com.canvas.enabled = true;
        //return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {

        //Time.timeScale = 1f;
        //StartCoroutine(ProcessClose());
        return false;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }

    public void Button_Restart()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        SceneChanger.Instance.LoadThisScene(SceneChanger.Instance.GetNowSceneName());
        Com.canvasGroup.interactable = false;
        //Time.timeScale = 1f;
    }

    public void Button_ReturnFieldMap()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        Com.canvasGroup.interactable = false;
        //Time.timeScale = 1f;
    }

    public void Button_QuitGame()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        UIManager.Instance.OpenQuitPopup();
    }

}

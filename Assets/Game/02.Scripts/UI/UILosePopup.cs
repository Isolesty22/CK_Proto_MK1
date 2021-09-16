using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILosePopup : UIBase
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        CheckOpen();
    }
    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;

        //Com.canvas.enabled = true;
        //return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {

        StartCoroutine(ProcessClose());
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }

    public void Button_Restart()
    {
        SceneChanger.Instance.LoadThisScene(SceneChanger.Instance.GetNowSceneName());
        Com.canvasGroup.interactable = false;
        //Time.timeScale = 1f;
    }

    public void Button_ReturnFieldMap()
    {
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        Com.canvasGroup.interactable = false;
        //Time.timeScale = 1f;
    }

}

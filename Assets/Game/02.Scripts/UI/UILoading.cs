using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{

    public Image backgroundImage;
    public Image loadingBarImage;



    #region Joke
    public RectTransform ipiaTransform;

    public float startPosY;

    public void CalcStartPosY()
    {
        startPosY = 0f - ipiaTransform.rect.height;
    }
    #endregion

    private void Start()
    {
        Init();
        //RegisterUIManager();
    }

    public override void Init()
    {
        CheckOpen();
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
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

    protected override IEnumerator ProcessClose()
    {
        yield return StartCoroutine(base.ProcessClose());
        SceneChanger.Instance.isLoading = false;
    }

    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}

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
        Com.canvas.enabled = true;
        return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {
        Com.canvas.enabled = false;
        return isOpen = Com.canvas.enabled;
    }
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}

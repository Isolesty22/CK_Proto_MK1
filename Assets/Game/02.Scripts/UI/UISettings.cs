using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 환경설정 UI
/// </summary>
public class UISettings : UIBase
{

    public class Sliders
    {
        public Slider master;
        public Slider bgm;
        public Slider sfx;
    }

    [SerializeField] private Sliders sliders = new Sliders();

    public Sliders VolumeSlider => sliders;

    private void Start()
    {
        Init();
        RegisterUIManager();
    }

    public override void Init()
    {
        CheckOpen();
    }


    #region UI 기본

    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
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
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
    #endregion
}

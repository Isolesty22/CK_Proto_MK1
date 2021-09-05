using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 환경설정 UI
/// </summary>
public class UISettings : UIBase
{

    [Tooltip("현재 저장되어있는 데이터. \nDataManager의 Data_Settings와 동일해야합니다.")]
    private Data_Settings data_saved;

    [Tooltip("현재 수정 중인 데이터.")]
    private Data_Settings data_current;
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



    public void Button_Close(UIBase _uiBase)
    {
        //변경사항이 있다면
        if (!(data_current.IsEquals(data_saved)))
        {
            UIManager.Instance.OpenThis(_uiBase);
        }
        else //없다면
        {
            UIManager.Instance.CloseTop();
        }
    }

    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// 환경설정 UI
/// </summary>
public class UISettings : UIBase
{
    public AudioMixer audioMixer;

    [Tooltip("현재 저장되어있는 데이터. \nDataManager의 Data_Settings와 동일해야합니다.")]
    private Data_Settings data_saved;

    [Tooltip("현재 수정 중인 데이터.")]
    private Data_Settings data_current;

    [System.Serializable]
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

    public void Button_Save()
    {

    }
    public void ValueChanged_MasterSlider()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
    }


    /// <summary>
    /// UI들의 값을 _data에 있는 것으로 변경합니다.
    /// </summary>
    /// <param name="_data"></param>
    public void UpdateUI(Data_Settings _data)
    {
        VolumeSlider.master.value = GetFloat(_data.volume_master);
        VolumeSlider.bgm.value = GetFloat(_data.volume_bgm);
        VolumeSlider.sfx.value = GetFloat(_data.volume_sfx);
    }

    /// <summary>
    /// _data를 실제 설정에 반영합니다.
    /// </summary>
    /// <param name="_data"></param>
    public void UpdateSettings(Data_Settings _data)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(_data.volume_master)) * 20));
        audioMixer.SetFloat("SfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(_data.volume_bgm)) * 20));
        audioMixer.SetFloat("BgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(_data.volume_sfx)) * 20));
    }

    public void ValueChanged_BGMSlider()
    {
        audioMixer.SetFloat("BgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
    }
    public void ValueChanged_SFXSlider()
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
    }


    /// <summary>
    /// string을 float로 변환해줍니다.
    /// </summary>

    private float GetFloat(string input)
    {
        return (float)System.Convert.ToDouble(input);
    }

    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }


}

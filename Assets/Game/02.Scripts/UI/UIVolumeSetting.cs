using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


/// <summary>
/// 볼륨 설정 UI
/// </summary>
public class UIVolumeSetting : UIBase
{
    [Tooltip("현재 수정 중인 데이터.")]
    private Data_Settings currentData_settings;

    [Header("Volume 관련")]
    public AudioMixer audioMixer;


    [System.Serializable]
    public class Sliders
    {
        public Slider master;
        public Slider bgm;
        public Slider sfx;
    }

    [SerializeField] private Sliders sliders = new Sliders();
    public Sliders VolumeSlider => sliders;

    private DataManager dataManager = null;
    private UIManager uiManager = null;
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }

    public override void Init()
    {
        CheckOpen();
        VolumeSlider.master.onValueChanged.AddListener(delegate { OnValueChanged_MasterSlider(); });
        VolumeSlider.bgm.onValueChanged.AddListener(delegate { OnValueChanged_BGMSlider(); });
        VolumeSlider.sfx.onValueChanged.AddListener(delegate { OnValueChanged_SFXSlider(); });

        currentData_settings = new Data_Settings();


        dataManager = DataManager.Instance;

        if (!ReferenceEquals(dataManager, null))//null이 아닐 경우에만
        {
            //데이터매니저에서 데이터를 가져옴
            currentData_settings.CopyData(dataManager.currentData_settings);
        }

        AudioManager.Instance.SettingVolume(GetFloat(currentData_settings.volume_master), GetFloat(currentData_settings.volume_bgm), GetFloat(currentData_settings.volume_sfx));
    }


    #region UIBase---
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        this.enabled = true;
        UpdateUI();
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
    protected override IEnumerator ProcessClose()
    {
        //다 닫은다음에 false
        yield return base.ProcessClose();
        this.enabled = false;
    }
    #endregion

    private bool isSaving = false;
    private IEnumerator ProcessSave()
    {
        isSaving = true;
        //데이터 매니저의 현재 데이터를 변경된 데이터로 설정
        dataManager.currentData_settings.CopyData(currentData_settings);

        AudioManager.Instance.SettingVolume(GetFloat(currentData_settings.volume_master), GetFloat(currentData_settings.volume_bgm), GetFloat(currentData_settings.volume_sfx));

        //변경된 데이터 저장
        yield return StartCoroutine(dataManager.SaveCurrentData(DataName.settings));

        isSaving = false;

        UIManager.Instance.CloseTop();
    }


    #region UpdateXXX---
    /// <summary>
    /// UI들의 값을 currentData에 있는 것으로 변경합니다.
    /// </summary>
    public void UpdateUI()
    {
        VolumeSlider.master.value = GetFloat(currentData_settings.volume_master);
        VolumeSlider.bgm.value = GetFloat(currentData_settings.volume_bgm);
        VolumeSlider.sfx.value = GetFloat(currentData_settings.volume_sfx);
    }

    /// <summary>
    /// currentData를 실제 설정에 반영합니다.
    /// </summary>
    public void UpdateVolumeSetting()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(currentData_settings.volume_master)) * 20));
        audioMixer.SetFloat("SfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(currentData_settings.volume_bgm)) * 20));
        audioMixer.SetFloat("BgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, GetFloat(currentData_settings.volume_sfx)) * 20));
    }

    #endregion

    #region Button---

    public void Button_Close()
    {
        UIManager.Instance.PlayAudio_Click();
        //변경사항이 있다면
        if (!(currentData_settings.IsEquals(dataManager.currentData_settings)))
        {
            //팝업 띄우기
            uiManager.OpenPopup(eUIText.DataSave,
                Button_Changes_Save, Button_Changes_Close);
        }
        else //없다면
        {
            //그냥 닫음
            uiManager.CloseTop();
        }
    }

    public void Button_Save()
    {
        UIManager.Instance.PlayAudio_Click();
        if (isSaving)
        {
            return;
        }
        StartCoroutine(ProcessSave());
    }

    public void Button_SetDefault()
    {
        UIManager.Instance.PlayAudio_Click();
        //현재 데이터를 저장된 데이터로 변경(폐기)
        //currentData_settings.CopyData(DataManager.Instance.currentData_settings);
        currentData_settings = new Data_Settings();

        //UI 업데이트
        UpdateUI();

        //실제 설정 업데이트
        UpdateVolumeSetting();
    }

    /// <summary>
    /// 변경된 사항을 저장하시겠습니까? 어쩌고...의 버튼
    /// </summary>
    private void Button_Changes_Save()
    {
        UIManager.Instance.PlayAudio_Click();
        if (isSaving)
        {
            return;
        }

        StartCoroutine(ProcessSave());

        //닫기
        uiManager.CloseTop();
        uiManager.CloseTop();
    }
    private void Button_Changes_Close()
    {
        UIManager.Instance.PlayAudio_Click();
        //현재 데이터를 저장된 데이터로 변경(폐기)
        currentData_settings.CopyData(dataManager.currentData_settings);

        //UI 업데이트
        UpdateUI();

        //실제 설정 업데이트
        UpdateVolumeSetting();

        //닫기
        uiManager.CloseTop();
        uiManager.CloseTop();
    }

    #endregion

    #region OnValueChanged---
    public void OnValueChanged_MasterSlider()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        currentData_settings.volume_master = VolumeSlider.master.value.ToString();
    }
    public void OnValueChanged_BGMSlider()
    {
        audioMixer.SetFloat("BgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        currentData_settings.volume_bgm = VolumeSlider.bgm.value.ToString();
    }
    public void OnValueChanged_SFXSlider()
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        currentData_settings.volume_sfx = VolumeSlider.sfx.value.ToString();
    }

    #endregion

    /// <summary>
    /// string을 float로 변환해줍니다.
    /// </summary>
    private float GetFloat(string _input)
    {
        return (float)System.Convert.ToDouble(_input);
    }


}

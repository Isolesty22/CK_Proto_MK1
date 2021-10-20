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
    [Header("키입력 감지기")]
    public KeyInputDetector keyInputDetector;


    /// <summary>
    /// 현재 저장되어있는 데이터. DataManager의 Data_Settings와 동일해야합니다.
    /// </summary>
    private Data_Settings data_saved;

    [Tooltip("현재 수정 중인 데이터.")]
    private Data_Settings data_current = new Data_Settings();

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

    [Header("캔버스그룹")]
    public CanvasGroup canvasGroup_Volume;
    public CanvasGroup canvasGroup_Key;

    private DataManager dataManager;
    private UIManager uiManager;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        CheckOpen();
        VolumeSlider.master.onValueChanged.AddListener(delegate { OnValueChanged_MasterSlider(); });
        VolumeSlider.bgm.onValueChanged.AddListener(delegate { OnValueChanged_BGMSlider(); });
        VolumeSlider.sfx.onValueChanged.AddListener(delegate { OnValueChanged_SFXSlider(); });

        uiManager = UIManager.Instance;
        dataManager = DataManager.Instance;

        data_saved = new Data_Settings();
        data_current = new Data_Settings();

        if (ReferenceEquals(dataManager, null))//null일경우
        {
            data_saved.CopyData(new Data_Settings());
            data_current.CopyData(data_saved);
        }
        else
        {
            data_saved.CopyData(dataManager.currentData_settings);
            data_current.CopyData(dataManager.currentData_settings);
        }

    }


    #region UIBase---
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        this.enabled = true;
        UpdateUI(data_current);
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
    private IEnumerator ProcessSaveCurrentData()
    {
        isSaving = true;
        //데이터 매니저의 현재 데이터를 변경된 데이터로 설정
        dataManager.currentData_settings.CopyData(data_current);

        //변경된 데이터 저장
        yield return StartCoroutine(dataManager.SaveCurrentData(DataManager.fileName_settings));

        //변경된 데이터를 '저장된 데이터'로 변경
        data_saved.CopyData(data_current);
        isSaving = false;
    }


    #region UpdateXXX---
/// <summary>
/// UI들의 값을 _data에 있는 것으로 변경합니다.
/// </summary>
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

    #endregion

    #region Button---

    public void Button_Close()
    {
        //변경사항이 있다면
        if (!(data_current.IsEquals(data_saved)))
        {
            //팝업 띄우기
            uiManager.OpenPopup(eUIText.DataSave,
                Button_ChangesSave, Button_ChangesClose);
        }
        else //없다면
        {
            //그냥 닫음
            uiManager.CloseTop();
        }
    }

    public void Button_Save()
    {
        if (isSaving)
        {
            return;
        }
        StartCoroutine(ProcessSaveCurrentData());
    }

    private void Button_ChangesSave()
    {
        if (isSaving)
        {
            return;
        }

        StartCoroutine(ProcessSaveCurrentData());

        //닫기
        uiManager.CloseTop();
        uiManager.CloseTop();
    }
    private void Button_ChangesClose()
    {
        //현재 데이터를 저장된 데이터로 변경(폐기)
        data_current.CopyData(data_saved);

        //UI 업데이트
        UpdateUI(data_current);

        //실제 설정 업데이트
        UpdateSettings(data_current);

        //닫기
        uiManager.CloseTop();
        uiManager.CloseTop();
    }
    #endregion

    #region OnValueChanged---
    public void OnValueChanged_MasterSlider()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        data_current.volume_master = VolumeSlider.master.value.ToString();
    }
    public void OnValueChanged_BGMSlider()
    {
        audioMixer.SetFloat("BgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        data_current.volume_bgm = VolumeSlider.bgm.value.ToString();
    }
    public void OnValueChanged_SFXSlider()
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, VolumeSlider.master.value)) * 20);
        data_current.volume_sfx = VolumeSlider.sfx.value.ToString();
    }

    #endregion

    #region GetXX---
    /// <summary>
    /// string을 float로 변환해줍니다.
    /// </summary>
    private float GetFloat(string _input)
    {
        return (float)System.Convert.ToDouble(_input);
    }

    /// <summary>
    /// string을 KeyCode로 변환해줍니다.
    /// </summary>
    private KeyCode GetKeyCode(string _keyCode)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), _keyCode);
    }

    #endregion


}

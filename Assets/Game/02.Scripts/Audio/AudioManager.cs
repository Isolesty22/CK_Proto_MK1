using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임 사운드 관리
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }
    #endregion

    #region 변수들

    [Serializable]
    public class AudioSourceCol
    {
        public AudioSource audioSource_BGM;
        public AudioSource audioSource_EVM;
        public AudioSource audioSource_SFX;
    }
    [SerializeField] private AudioSourceCol audioSource = new AudioSourceCol();

    public AudioSourceCol AudioSources => audioSource;

    public Dictionary<string, AudioClip> clipDict_BGM = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_EVM = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_SFX = new Dictionary<string, AudioClip>();

    #endregion
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        StartCoroutine(LoadAudioClips());
    }


    private string audioFilePath = string.Empty;
    public IEnumerator LoadAudioClips()
    {
        audioFilePath = Application.dataPath + "/Game/10.Audios/";
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("SS501_URMan.mp3", audioFilePath));
        clipDict_BGM.Add("SS501_URMan.mp3", DataManager.Instance.fileManager.getAudioClip_Result);

        AudioSources.audioSource_BGM.clip = clipDict_BGM["SS501_URMan.mp3"];
    }



}

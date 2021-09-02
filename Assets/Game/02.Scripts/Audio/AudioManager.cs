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
    public class AudioSources
    {
        public AudioSource audioSource_BGM;
        public AudioSource audioSource_EVM;
        public AudioSource audioSource_SFX;
    }
    [SerializeField] private AudioSources audioSources = new AudioSources();

    public AudioSources Audios => audioSources;

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

    public IEnumerator Init()
    {
       yield return StartCoroutine(LoadAudioClips());
    }


    private string audioFilePath = string.Empty;
    public IEnumerator LoadAudioClips()
    {
        audioFilePath = Application.dataPath + "/Game/10.Audios/";
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("SS501_URMan.mp3", audioFilePath));
        clipDict_BGM.Add("SS501_URMan.mp3", DataManager.Instance.fileManager.getAudioClip_Result);
        Debug.Log("암욜맨을 불러왔습니다.");

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("The_Red_Knot.mp3", audioFilePath));
        clipDict_BGM.Add("The_Red_Knot.mp3", DataManager.Instance.fileManager.getAudioClip_Result);
        Debug.Log("홍연을 불러왔습니다..");

        Audios.audioSource_BGM.clip = clipDict_BGM["The_Red_Knot.mp3"];
        Audios.audioSource_BGM.time = 0f;
        Audios.audioSource_BGM.volume = 0.5f;
        Audios.audioSource_BGM.Play();

        yield return new WaitForSecondsRealtime(10f);   
        float _time = Audios.audioSource_BGM.time;

        Audios.audioSource_BGM.Stop();
        Audios.audioSource_BGM.clip = clipDict_BGM["SS501_URMan.mp3"];
        Audios.audioSource_BGM.time = 43f;
        Audios.audioSource_BGM.volume = 0.5f;
        Audios.audioSource_BGM.Play();

        yield return new WaitForSecondsRealtime(10f);
        Audios.audioSource_BGM.Stop();
        Audios.audioSource_BGM.clip = clipDict_BGM["The_Red_Knot.mp3"];
        Audios.audioSource_BGM.time = _time;
        Audios.audioSource_BGM.Play();
    }



}

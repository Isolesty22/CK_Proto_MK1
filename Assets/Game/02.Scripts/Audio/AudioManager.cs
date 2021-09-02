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


        while (true)
        {
            Audios.audioSource_BGM.clip = clipDict_BGM["The_Red_Knot.mp3"];
            Audios.audioSource_BGM.volume = 0.5f;
            Audios.audioSource_BGM.Play();

            yield return new WaitUntil(() => !Audios.audioSource_BGM.isPlaying);


            Audios.audioSource_BGM.clip = clipDict_BGM["SS501_URMan.mp3"];
            Audios.audioSource_BGM.volume = 0.5f;
            Audios.audioSource_BGM.Play();

            yield return new WaitUntil(() => !Audios.audioSource_BGM.isPlaying);
        }

    }




    #region BGM

    /// <summary>
    /// BGM을 반복재생할 것인가?
    /// </summary>
    public void SetBgmLoop(bool _isLoop)
    {
        Audios.audioSource_BGM.loop = _isLoop;
    }
    public void PlayBGM(string _fileName)
    {

    }

    /// <summary>
    /// BGM을 부드럽게 재생시킵니다(크레센도). 
    /// </summary>
    /// <param name="_fileName">오디오 파일 이름</param>
    /// <param name="_time">해당 시간동안 볼륨이 올라갑니다.</param>
    public void PlayBGM_Smooth(string _fileName, float _time)
    {

    }

    private IEnumerator ProcessBgmLerp(string _fileName, float _time)
    {
        yield break;
    }



    private float keepBgmTime = 0f;
    private AudioClip keepBgmClip = null;

    /// <summary>
    /// 현재 BGM과, 그게 어떤 부분까지 재생되었는지를 따로 저장해둡니다.
    /// </summary>
    public void KeepCurrentBgmClip()
    {
        keepBgmTime = Audios.audioSource_BGM.time;
        keepBgmClip = Audios.audioSource_BGM.clip;
    }

    /// <summary>
    /// 저장해뒀던 BGM을 반환합니다. 이것을 바로 Play할 경우, 저장했던 부분부터 재생됩니다.
    /// </summary>
    public AudioClip GetKeepBgmClip()
    {
        Audios.audioSource_BGM.time = keepBgmTime;
        return keepBgmClip;
    }

    #endregion
}

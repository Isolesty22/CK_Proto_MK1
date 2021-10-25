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
    public static AudioManager Instance;
    #endregion

    #region 변수들

    [Serializable]
    public class AudioSources
    {
        public AudioSource audioSource_BGM;
        public AudioSource audioSource_EVM;
        public AudioSource audioSource_SFX;
        public AudioSource audioSource_PAttack;
        public AudioSource audioSource_PParrying;
        public AudioSource audioSource_PWalk;
        public AudioSource audioSource_PRun;
        public AudioSource audioSource_PJump;
        public AudioSource audioSource_PLand;
        public AudioSource audioSource_PHit;
    }
    [Serializable]
    public class AudioClips
    {
        public AudioClip stage1BGM;
        public AudioClip stage1AmbientSound;
        public AudioClip stage2BGM;
    }


    [SerializeField] private AudioSources audioSources = new AudioSources();
    [SerializeField] private AudioClips audioClips = new AudioClips();

    public AudioClips clips => audioClips;
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
            Instance = instance;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
            if (Instance != this) //나 자신이 인스턴스가 아닐 경우
            {
                Debug.Log(this + " : 더 이상, 이 세계선에서는 존재할 수 없을 것 같아... 안녕.");
                Destroy(this.gameObject);
            }

        }
    }

    private void Start()
    {
    }

    public IEnumerator Init()
    {
        yield return StartCoroutine(LoadAudioClips());
    }


    public IEnumerator LoadAudioClips()
    {

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("SS501_URMan"));
        clipDict_BGM.Add("SS501_URMan", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("The_Red_Knot"));
        clipDict_BGM.Add("The_Red_Knot", DataManager.Instance.fileManager.getAudioClip_Result);

        /*
         * for (i = 0; i < audioFileNameList.Count; i++) {
         *  yield return StartCoroutine GetAudioClip(audioFileNameList[i]);
         *  clipDict_BGM.Add(audioFileNameList[i], getAudioClip_Result);
         *  }
         */

        //while (true)
        //{

        //    PlayBGM_Smooth("The_Red_Knot", 5f);

        //    yield return new WaitUntil(() => !Audios.audioSource_BGM.isPlaying);

        //    PlayBGM_Smooth("SS501_URMan", 1f);

        //    yield return new WaitUntil(() => !Audios.audioSource_BGM.isPlaying);
        //}

    }

    public IEnumerator TestPlayBgmLoop()
    {
        while (true)
        {

            PlayBGM_Smooth("The_Red_Knot", 5f);

            yield return new WaitUntil(() => !Audios.audioSource_BGM.isPlaying);

            PlayBGM_Smooth("SS501_URMan", 1f);

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
        AudioClip tempClip = clipDict_BGM[_fileName];
        Audios.audioSource_BGM.clip = tempClip;
        Audios.audioSource_BGM.Play();
    }

    /// <summary>
    /// BGM을 부드럽게 재생시킵니다(크레센도). 
    /// </summary>
    /// <param name="_fileName">오디오 파일 이름</param>
    /// <param name="_smoothTime">해당 시간동안 볼륨이 올라갑니다.</param>
    public void PlayBGM_Smooth(string _fileName, float _smoothTime)
    {
        StartCoroutine(ProcessBgmLerp(_fileName, _smoothTime, true));
    }


    public void StopBGM()
    {
        Audios.audioSource_BGM.Stop();
    }

    public void StopBGM_Smooth(string _fileName, float _smoothTime)
    {
        StartCoroutine(ProcessBgmLerp(_fileName, _smoothTime, false));
    }



    private readonly float myEarGuard = 0.5f;
    private IEnumerator ProcessBgmLerp(string _fileName, float _smoothTime, bool _isStart)
    {
        float timer = 0f;
        float progress = 0f;

        AudioClip tempClip = clipDict_BGM[_fileName];
        AudioSource source = Audios.audioSource_BGM;

        if (_isStart) //시작하는 경우
        {
            source.clip = tempClip;
            source.volume = 0f;
            source.Play();

            while (progress < 1f)
            {
                timer += Time.unscaledDeltaTime;
                progress = timer / _smoothTime;

                source.volume = progress - myEarGuard;

                yield return null;
            }


        }
        else //종료하는 경우
        {
            source.volume = 1f - myEarGuard;

            while (progress < 1f)
            {
                timer += Time.unscaledDeltaTime;
                progress = timer / _smoothTime;

                source.volume = 1f - progress - myEarGuard;

                yield return null;
            }

            source.Stop();
            source.volume = 1f- myEarGuard;
        }

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

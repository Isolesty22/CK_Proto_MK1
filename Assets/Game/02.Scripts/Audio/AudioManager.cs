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
        public AudioSource audioSource_UI;

        [Header("PlayerSFX")]
        public AudioSource audioSource_PAttack;
        public AudioSource audioSource_PParrying;
        public AudioSource audioSource_PWalk;
        public AudioSource audioSource_PRun;
        public AudioSource audioSource_PJump;
        public AudioSource audioSource_PLand;
        public AudioSource audioSource_PHit;
    }

    [Serializable]
    public class AudioVolume
    {
        [Range(0, 1)] public float bgm = 1f;
        [Range(0, 1)] public float evm = 1f;
        [Range(0, 1)] public float sfx = 1f;
        [Range(0, 1)] public float ui = 1f;

        [Range(0, 1)] public float pAttack = 1f;
        [Range(0, 1)] public float pParrying = 1f;
        [Range(0, 1)] public float pWalk = 1f;
        [Range(0, 1)] public float pRun = 1f;
        [Range(0, 1)] public float pJump = 1f;
        [Range(0, 1)] public float pLand = 1f;
        [Range(0, 1)] public float pHit = 1f;
    }

    [SerializeField] private AudioSources audioSources = new AudioSources();
    [SerializeField] private AudioVolume audioVolumes = new AudioVolume();

    public AudioSources Audios => audioSources;
    public AudioVolume Volumes => audioVolumes;


    public Dictionary<string, AudioClip> clipDict_BGM = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_EVM = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_SFX = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_ArrowHit = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_Monster = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_Player = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> clipDict_UI = new Dictionary<string, AudioClip>();
    public List<AudioClip> specialAttackClips = new List<AudioClip>();
    public AudioClip monsterDeath;
    public AudioClip powerAttack;
    public float currentMasterVolume;
    public float currentSFXVolume;
    public float curentBGMVolume;
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
        SetInitVolume();
    }

    private void Start()
    {
        SettingVolume();
    }

    public IEnumerator Init()
    {
        yield return StartCoroutine(LoadAudioClips());

    }

    public void SetInitVolume()
    {
        Volumes.ui = .5f;
        Volumes.pAttack = .3f;
        Volumes.pRun = .4f;
        Volumes.pWalk = .4f;
        Volumes.pJump = .5f;
        Volumes.pParrying = .7f;
        Volumes.pHit = 1f;
    }

    public IEnumerator LoadAudioClips()
    {
        #region BGM_Load
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/MainMenuBGM"));
        clipDict_BGM.Add("MainMenuBGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage1ambient"));
        clipDict_BGM.Add("Stage1ambient", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage1BGM"));
        clipDict_BGM.Add("Stage1BGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage2BGM"));
        clipDict_BGM.Add("Stage2BGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage3BGM"));
        clipDict_BGM.Add("Stage3BGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage3ambient"));
        clipDict_BGM.Add("Stage3ambient", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/Stage4BGM"));
        clipDict_BGM.Add("Stage4BGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/TutorialBGM"));
        clipDict_BGM.Add("TutorialBGM", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/WaterFall"));
        clipDict_BGM.Add("WaterFall", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("BGM/FieldMapBGM"));
        clipDict_BGM.Add("FieldMapBGM", DataManager.Instance.fileManager.getAudioClip_Result);
        #endregion

        #region ArrowHit
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Arrows/armadiloHit"));
        clipDict_ArrowHit.Add("armadiloHit", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Arrows/arrowHitMon"));
        clipDict_ArrowHit.Add("arrowHitMon", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Arrows/arrowHitObj"));
        clipDict_ArrowHit.Add("arrowHitObj", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Arrows/arrowHitPower"));
        clipDict_ArrowHit.Add("arrowHitPower", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Arrows/arrowHitSpecial"));
        clipDict_ArrowHit.Add("arrowHitSpecial", DataManager.Instance.fileManager.getAudioClip_Result);
        #endregion

        #region Player
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaAttack"));
        clipDict_Player.Add("IpeaAttack", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaHit"));
        clipDict_Player.Add("IpeaHit", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaJump"));
        clipDict_Player.Add("IpeaJump", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaLand"));
        clipDict_Player.Add("IpeaLand", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaParrying"));
        clipDict_Player.Add("IpeaParrying", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaRun"));
        clipDict_Player.Add("IpeaRun", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaRunOnWood"));
        clipDict_Player.Add("IpeaRunOnWood", DataManager.Instance.fileManager.getAudioClip_Result);

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Player/IpeaWalk"));
        clipDict_Player.Add("IpeaWalk", DataManager.Instance.fileManager.getAudioClip_Result);

        Audios.audioSource_PAttack.clip = clipDict_Player["IpeaAttack"];
        Audios.audioSource_PHit.clip = clipDict_Player["IpeaHit"];
        Audios.audioSource_PJump.clip = clipDict_Player["IpeaJump"];
        Audios.audioSource_PLand.clip = clipDict_Player["IpeaLand"];
        Audios.audioSource_PParrying.clip = clipDict_Player["IpeaParrying"];
        Audios.audioSource_PRun.clip = clipDict_Player["IpeaRun"];
        Audios.audioSource_PWalk.clip = clipDict_Player["IpeaWalk"];
        #endregion

        #region UI
        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("UI/Click"));
        clipDict_UI.Add("Click", DataManager.Instance.fileManager.getAudioClip_Result);

        //yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("UI/Close"));
        //clipDict_UI.Add("Close", DataManager.Instance.fileManager.getAudioClip_Result);
        #endregion

        for (int i = 0; i < 6; i++)
        {
            string n = "SpecialAttack/Arrow_" + (i + 1);
            yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip(n));
            specialAttackClips.Add(DataManager.Instance.fileManager.getAudioClip_Result);
        }

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("Monster/MonsterDeath"));
        monsterDeath = DataManager.Instance.fileManager.getAudioClip_Result;

        yield return StartCoroutine(DataManager.Instance.fileManager.GetAudioClip("SpecialAttack/PA2"));
        powerAttack = DataManager.Instance.fileManager.getAudioClip_Result;


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

    #region Setting

    public void SettingVolume()
    {
        Audios.audioSource_BGM.volume = Volumes.bgm * currentMasterVolume * curentBGMVolume;
        Audios.audioSource_EVM.volume = Volumes.evm * currentMasterVolume * curentBGMVolume;
        Audios.audioSource_SFX.volume = Volumes.sfx * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_UI.volume = Volumes.ui * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PAttack.volume = Volumes.pAttack * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PHit.volume = Volumes.pHit * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PJump.volume = Volumes.pJump * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PLand.volume = Volumes.pLand * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PParrying.volume = Volumes.pParrying * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PRun.volume = Volumes.pRun * currentMasterVolume * currentSFXVolume;
        Audios.audioSource_PWalk.volume = Volumes.pWalk * currentMasterVolume * currentSFXVolume;
    }

    public void SettingVolume(float masterVolume, float bgmVolume, float sfxVolume)
    {
        currentMasterVolume = masterVolume;
        curentBGMVolume = bgmVolume;
        currentSFXVolume = sfxVolume;

        Audios.audioSource_BGM.volume = Volumes.bgm * masterVolume * bgmVolume;
        Audios.audioSource_EVM.volume = Volumes.evm * masterVolume * bgmVolume;
        Audios.audioSource_SFX.volume = Volumes.sfx * masterVolume* sfxVolume;
        Audios.audioSource_UI.volume = Volumes.ui * masterVolume * sfxVolume;
        Audios.audioSource_PAttack.volume = Volumes.pAttack * masterVolume * sfxVolume;
        Audios.audioSource_PHit.volume = Volumes.pHit * masterVolume * sfxVolume;
        Audios.audioSource_PJump.volume = Volumes.pJump * masterVolume * sfxVolume;
        Audios.audioSource_PLand.volume = Volumes.pLand * masterVolume * sfxVolume;
        Audios.audioSource_PParrying.volume = Volumes.pParrying * masterVolume * sfxVolume;
        Audios.audioSource_PRun.volume = Volumes.pRun * masterVolume * sfxVolume;
        Audios.audioSource_PWalk.volume = Volumes.pWalk * masterVolume * sfxVolume;
    }

    public void SettingVolume(string masterVolume, string bgmVolume, string sfxVolume)
    {
        currentMasterVolume = GetFloat(masterVolume);
        curentBGMVolume = GetFloat(bgmVolume);
        currentSFXVolume = GetFloat(sfxVolume);

        Audios.audioSource_BGM.volume = Volumes.bgm * GetFloat(masterVolume) * GetFloat(bgmVolume);
        Audios.audioSource_EVM.volume = Volumes.evm * GetFloat(masterVolume) * GetFloat(bgmVolume);
        Audios.audioSource_SFX.volume = Volumes.sfx * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_UI.volume = Volumes.ui * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PAttack.volume = Volumes.pAttack * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PHit.volume = Volumes.pHit * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PJump.volume = Volumes.pJump * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PLand.volume = Volumes.pLand * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PParrying.volume = Volumes.pParrying * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PRun.volume = Volumes.pRun * GetFloat(masterVolume) * GetFloat(sfxVolume);
        Audios.audioSource_PWalk.volume = Volumes.pWalk * GetFloat(masterVolume) * GetFloat(sfxVolume);
    }
    private float GetFloat(string _input)
    {
        return (float)System.Convert.ToDouble(_input);
    }
    #endregion
}

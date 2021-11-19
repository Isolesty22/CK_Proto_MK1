using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Instance
    private static SceneChanger instance;
    public static SceneChanger Instance;
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = ;
    //        }
    //        return instance;
    //    }
    //    set
    //    {
    //        Instance = value;
    //    }
    //}

    #endregion


    public UILoading uiLoading;

    [Space(10)]

    [HideInInspector]
    public bool isLoading = false;
    [HideInInspector]
    public bool isSceneLoading = false;

    [SerializeField]
    private string moveSceneName = string.Empty;
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
    public void LoadThisScene(string _sceneName)
    {
        StartCoroutine(LoadThisSceneToName(_sceneName));
    }
    public bool IsStageScene(string _sceneName)
    {
        if (_sceneName.Contains("Stage"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private const string str_NAEYONG = "NAEYONG";
    /// <summary>
    /// 씬 로드 코루틴. LoadThisScene 함수를 호출했을 때 실행됩니다.
    /// </summary>
    public IEnumerator LoadThisSceneToName(string _sceneName)
    {
        AudioManager.Instance.SettingVolume(DataManager.Instance.currentData_settings.volume_master, DataManager.Instance.currentData_settings.volume_bgm, DataManager.Instance.currentData_settings.volume_sfx);
        AudioManager.Instance.Audios.audioSource_BGM.Stop();
        AudioManager.Instance.Audios.audioSource_EVM.Stop();
        isLoading = true;
        isSceneLoading = true;
        moveSceneName = _sceneName;

        int tooltipRandIndex = UnityEngine.Random.Range(0, DataManager.Instance.currentData_tooltip.Count);
        uiLoading.SetText(DataManager.Instance.currentData_tooltip[tooltipRandIndex][str_NAEYONG] as string);
        //게이지를 0으로 설정
        uiLoading.loadingBarImage.fillAmount = 0f;
        uiLoading.circleTransform.rotation = Quaternion.Euler(Vector3.zero);

        yield return StartCoroutine(uiLoading.OpenThis());
        //uiLoading.Open();

        //yield return new WaitUntil(() =>uiLoading.isOpen);


        //비동기로 로드하기
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(moveSceneName);
        asyncOperation.allowSceneActivation = false; //씬 활성화 false : 로딩이 끝나도 씬이 활성화되지 않음

        //씬 로딩이 끝나면 함수 호출
        SceneManager.sceneLoaded += LoadSceneEnd;

        float timer = 0f;
        float progress = 0f;

        Vector3 circleRot = new Vector3(0f, 5f, 0f);

        Resources.UnloadUnusedAssets();

        if (IsStageScene(moveSceneName))
        {
            yield return StartCoroutine(DataManager.Instance.LoadData_Talk(moveSceneName));

        }

        //저장
        yield return StartCoroutine(DataManager.Instance.SaveCurrentData(DataName.player));

        while (!asyncOperation.isDone) //로딩이 완료되기 전 까지만
        {
            timer += Time.unscaledDeltaTime;

            if (asyncOperation.progress < 0.9f)
            {
                progress = Mathf.Lerp(progress, asyncOperation.progress, timer);

                //게이지 올리기
                uiLoading.loadingBarImage.fillAmount = progress;

                if (progress >= asyncOperation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progress = Mathf.Lerp(progress, 1f, timer);

                uiLoading.loadingBarImage.fillAmount = progress;

                if (progress >= 1f)
                {
                    uiLoading.loadingBarImage.fillAmount = 1f;
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        Debug.Log("씬 로딩 완료!");

        asyncOperation.allowSceneActivation = true;

        Debug.Log("씬을 활성화했습니다.");

        //if (Instance == null)
        //{
        //    instance = this;
        //    Instance = instance;
        //}
        yield break;
    }
    /// <summary>
    /// 현재 활성화 되어있는 씬 이름을 반환합니다.
    /// </summary>
    public string GetNowSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public Action onSceneLoadEnded = null;
    public void LoadSceneEnd(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        SceneManager.sceneLoaded -= LoadSceneEnd;
        isSceneLoading = false;
        Debug.Log("LoadSceneEnd 함수 호출");
        onSceneLoadEnded?.Invoke();
        if (_scene.name != moveSceneName)
        {
            Debug.LogError("현재 씬과 이동하려고 했던 씬의 이름이 다르다!! 뭐임...?");
            Debug.LogError("현재 씬 이름 : " + _scene.name + "/ 이동하려고 헀던 씬 이름 : " + moveSceneName);
            return;
        }


        uiLoading.Close();
        Time.timeScale = 1f;
        #region BGMSet

        switch (GetNowSceneName())
        {
            case "Stage_00":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["TutorialBGM"];
                AudioManager.Instance.Volumes.bgm = 0.5f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Stop();
                break;
            case "FieldMap":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["FieldMapBGM"];
                AudioManager.Instance.Volumes.bgm = 0.7f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Stop();
                break;
            case "Stage_01":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["Stage1BGM"];
                AudioManager.Instance.Volumes.bgm = 0.2f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1;
                AudioManager.Instance.Audios.audioSource_EVM.clip = AudioManager.Instance.clipDict_BGM["Stage1ambient"];
                AudioManager.Instance.Volumes.evm = 0.3f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Play();
                break;
            case "Stage_02":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["Stage2BGM"];
                AudioManager.Instance.Volumes.bgm = 0.7f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Stop();
                break;
            case "Stage_03":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["Stage3BGM"];
                AudioManager.Instance.Volumes.bgm = 0.7f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1;
                AudioManager.Instance.Audios.audioSource_EVM.clip = AudioManager.Instance.clipDict_BGM["Stage3ambient"];
                AudioManager.Instance.Volumes.evm = 0.3f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Play();
                break;
            case "Stage_04":
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["Stage4BGM"];
                AudioManager.Instance.Volumes.bgm = 0.2f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Stop();
                break;
            default:
                AudioManager.Instance.Audios.audioSource_BGM.clip = AudioManager.Instance.clipDict_BGM["MainMenuBGM"];
                AudioManager.Instance.Volumes.bgm = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.pitch = 1f;
                AudioManager.Instance.Audios.audioSource_BGM.Play();
                AudioManager.Instance.Audios.audioSource_EVM.Stop();
                break;
        }

        #endregion

    }
}

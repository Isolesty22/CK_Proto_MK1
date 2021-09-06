using System.Collections;
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
        StartCoroutine(LoadThisScene_Joke(_sceneName));
    }
    public IEnumerator LoadThisScene_Joke(string _sceneName)
    {
        isLoading = true;

        moveSceneName = _sceneName;

        //시작위치 계산
        uiLoading.CalcStartPosY();
        float startY = uiLoading.startPosY - 1f;
        float absStartY = Mathf.Abs(uiLoading.startPosY - 1f);

        //이피아를 시작위치로
        uiLoading.ipiaTransform.anchoredPosition
            = new Vector2(uiLoading.ipiaTransform.anchoredPosition.x, uiLoading.startPosY);

        uiLoading.Open();

        yield return new WaitUntil(() =>uiLoading.isOpen);
        //비동기로 로드하기
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(moveSceneName);
        asyncOperation.allowSceneActivation = false; //씬 활성화 false : 로딩이 끝나도 씬이 활성화되지 않음

        SceneManager.sceneLoaded += LoadSceneEnd;

        //타이머
        float timer = 0f;

        //진행도
        float progress = 0f;

        while (!asyncOperation.isDone) //로딩이 완료되기 전 까지만
        {
            timer += Time.unscaledDeltaTime;

            #region 녹화용
            //progress = timer / 3f;
            //uiLoading.ipiaTransform.anchoredPosition
            //    = new Vector2(uiLoading.ipiaTransform.anchoredPosition.x, startY + (absStartY * progress));
            //if (progress >= 1f)
            //{
            //    yield return new WaitForSecondsRealtime(1f);
            //    asyncOperation.allowSceneActivation = true;
            //}
            #endregion
            #region real
            if (asyncOperation.progress < 0.9f)
            {
                progress = Mathf.Lerp(progress, asyncOperation.progress, timer);
                //이피아 이동시키기
                uiLoading.ipiaTransform.anchoredPosition
                    = new Vector2(uiLoading.ipiaTransform.anchoredPosition.x, startY + (absStartY * progress));

                if (progress >= asyncOperation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progress = Mathf.Lerp(progress, 1f, timer);

                uiLoading.ipiaTransform.anchoredPosition
                     = new Vector2(uiLoading.ipiaTransform.anchoredPosition.x, startY + (absStartY * progress));

                if (progress >= 1f)
                {
                    break;
                }
            }
            #endregion
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        Debug.Log("씬 로딩 완료.");

        asyncOperation.allowSceneActivation = true;

        Debug.Log("씬을 활성화했습니다.");

        //if (Instance == null)
        //{
        //    instance = this;
        //    Instance = instance;
        //}
        yield break;
    }


    public void LoadSceneEnd(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        if (_scene.name != moveSceneName)
        {
            Debug.LogError("현재 씬과 이동하려고 했던 씬의 이름이 다르다!! 뭐임...?");
            return;
        }
        SceneManager.sceneLoaded -= LoadSceneEnd;
        Debug.Log("LoadSceneEnd 함수 호출!!");

        uiLoading.Close();
    }
}

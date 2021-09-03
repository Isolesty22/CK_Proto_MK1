using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Instance
    private static SceneChanger instance;
    public static SceneChanger Instance;
    #endregion


    public UILoading uiLoading;

    [Space(10)]

    [SerializeField]
    private bool isLoading = false;

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

    public void LoadTestHomeScene()
    {
        SceneManager.LoadScene("TestHomeScene");
    }


    public IEnumerator LoadThisScene_Joke(string _sceneName)
    {
        isLoading = true;

        moveSceneName = _sceneName;

        //시작위치 계산
        uiLoading.CalcStartPosY();
        float startY = uiLoading.startPosY;
        float endY = 0f;

        //비동기로 로드
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(moveSceneName);
        asyncOperation.allowSceneActivation = false; //씬 활성화 false : 로딩이 끝나도 씬이 활성화되지 않음

        SceneManager.sceneLoaded += LoadSceneEnd;

        yield break;
    }


    public void LoadSceneEnd(Scene _scene, LoadSceneMode _loadSceneMode)
    {

        if (_scene.name == moveSceneName)
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임을 처음 시작할 때의 첫 로딩을 관리해주는 매니저
/// </summary>
public class GameStarter : MonoBehaviour
{
    public UISplashLogo uiSplashLogo;
    public UITitleScreen uiTitleScreen;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        StartCoroutine(ProcessStart());
    }
    private IEnumerator ProcessStart()
    {

        //스플래시가 끝날 때 까지 대기
        yield return StartCoroutine(uiSplashLogo.ProcessSplashAllImage());
        //yield return new WaitUntil(() => uiSplashLogo.isEnd);

        uiTitleScreen.Open();
        uiTitleScreen.SetText_pressKey(false);

        //타이틀 화면이 열릴 때 까지 대기
        yield return new WaitUntil(() => uiTitleScreen.isOpen);

        //데이터 파일 로드 
        SetText_Load("이피아가 이불을 정리하는 중...");
        yield return StartCoroutine(DataManager.Instance.Init_DataFiles());

        //오디오 파일 로드
        SetText_Load("이피아가 양치질 하는 중...");
        yield return StartCoroutine(AudioManager.Instance.Init());

        SetText_Load("이피아가 모든 준비를 끝마쳤습니다.");
        yield return new WaitForSecondsRealtime(1f);
        SetText_Load(string.Empty);

        //'아무 키나 누르세요' 동안 대기
        uiTitleScreen.SetText_pressKey(true);
        yield return StartCoroutine(uiTitleScreen.ProcessWaitPressAnyKey());
        uiTitleScreen.Close();

        //타이틀 화면이 닫힐 때 까지 대기
        yield return new WaitWhile(() => uiTitleScreen.isOpen);

        yield return new WaitForSecondsRealtime(1f);

        //메인 화면으로 이동
        StartCoroutine(SceneChanger.Instance.LoadThisSceneToName(SceneNames.mainMenu));

        //로딩이 끝날 때 까지 대기
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);

        yield return null;

        //메인 메뉴 Open
        //UIMainMenu uiMainMenu = UIManager.Instance.uiDict["UIMainMenu"] as UIMainMenu;
        //uiMainMenu.Open();

        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    private void SetText_Load(string _text)
    {
        uiTitleScreen.SetText_Load(_text);
    }
}

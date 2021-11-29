using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIBase
{

    [SerializeField]
    private SelectorController selectorController;

    public ParticleSystem particle;
    [Tooltip("까만색 패널")]
    public GameObject blackPanel;

    public UIMovieScreen movieScreen;

    private void Start()
    {
        particle.Play();
        Open();
    }
    protected override void CheckOpen()
    {
        base.CheckOpen();
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        return true;

    }
    public void Button_StartNewGame()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenPopup(eUIText.StartNewGame,
            StartNewGame, UIManager.Instance.CloseTop);
    }
    public void Button_StartTutorial()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenPopup(eUIText.StartTutorial,
            StartNewGame, UIManager.Instance.CloseTop);
    }

    private void StartNewGame()
    {
        //상호작용 불가
        Com.canvasGroup.interactable = false;
        UIManager.Instance.CloseTop();

        if (AudioManager.Instance.Audios.audioSource_BGM.isPlaying)
            AudioManager.Instance.Audios.audioSource_BGM.Stop();

        Debug.Log("Start New Game...");
        //DataManager.Instance.currentData_player = new Data_Player();
        //StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.DataName.settings));
        //SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);

        DataManager.Instance.currentData_player = new Data_Player();
        StartCoroutine(DataManager.Instance.SaveCurrentData(DataName.player));

        movieScreen.onMovieEnded += OnMovieEnded;
        StartCoroutine(movieScreen.playMovie);
    }

    private void Update()
    {
        if (UIManager.Instance.uiStackCount == 0)
        {
            selectorController.DetectKey();
        }
    }
    private void OnMovieEnded()
    {
        movieScreen.onMovieEnded -= OnMovieEnded;

        UIManager.Instance.OpenPopup(eUIText.StartTutorial, LoadTutorial, LoadFieldMap);
    }


    public void LoadFieldMap()
    {
        //스테이지 1 기준 데이터 생성
        Data_Player tempData = new Data_Player();
        tempData.currentStageName = SceneNames.stage_01;
        tempData.currentStageNumber = 1;
        DataManager.Instance.currentClearStageNumber = 0;
        DataManager.Instance.currentData_player.CopyData(tempData);

        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }
    public void LoadTutorial() => SceneChanger.Instance.LoadThisScene(SceneNames.stage_00);

    public void Button_ContinueGame()
    {
        UIManager.Instance.PlayAudio_Click();


        if (DataManager.Instance.isCreatedNewPlayerData) //데이터가 없었던 상태라면
        {
            //근데 이제 디폴트 생성자랑 데이터가 같을 때만 데이터 없다는 창 띄우기
            if (DataManager.Instance.currentData_player.IsEquals(new Data_Player()))
            {
                UIManager.Instance.OpenPopup(eUIText.NoPlayerData,
                     StartNewGame,
                      Button_Continue_Close);
            }
            //디폴트 생성자랑 데이터가 다르면 데이터가 있다는 뜻임. 필드맵 로드 ㄱㄱ
            else
            {
                //상호작용 불가
                Com.canvasGroup.interactable = false;
                SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
            }

        }
        // 필드맵 로드 ㄱㄱ
        else
        {
            //상호작용 불가
            Com.canvasGroup.interactable = false;
            SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        }

    }

    private void Button_Continue_OK()
    {
        UIManager.Instance.PlayAudio_Click();

        Com.canvasGroup.interactable = false;
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    private void Button_Continue_Close()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.CloseTop();

    }
    public void Button_OpenSettings()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenThis(UIName.UIOption_Main);
    }

    public void Button_QuitGame()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenQuitPopup();
    }
}

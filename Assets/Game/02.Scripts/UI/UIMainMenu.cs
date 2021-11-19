using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIBase
{

    public UIMovieScreen movieScreen;

    private void Start()
    {
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
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        UIManager.Instance.OpenPopup(eUIText.StartNewGame,
            StartNewGame, UIManager.Instance.CloseTop);
    }
    public void Button_StartTutorial()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        UIManager.Instance.OpenPopup(eUIText.StartTutorial,
            StartNewGame, UIManager.Instance.CloseTop);
    }

    private void StartNewGame()
    {
        //상호작용 불가
        Com.canvasGroup.interactable = false;
        UIManager.Instance.CloseTop();

        if(AudioManager.Instance.Audios.audioSource_BGM.isPlaying)
            AudioManager.Instance.Audios.audioSource_BGM.Stop();

        Debug.Log("Start New Game...");
        //DataManager.Instance.currentData_player = new Data_Player();
        //StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.DataName.settings));
        //SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);

        DataManager.Instance.currentData_player = new Data_Player();
        StartCoroutine(DataManager.Instance.SaveCurrentData(DataName.player));

        movieScreen.OnMovieEnded += OnMovieEnded;
        StartCoroutine(movieScreen.playingCoroutine);
    }

    private void OnMovieEnded()
    {
        movieScreen.OnMovieEnded -= OnMovieEnded;

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
    private IEnumerator ProcessStartNewGame()
    {
        //Com.canvasGroup.interactable = false;

        //DataManager.Instance.currentData_player = new Data_Player();
        //yield return StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.DataName.settings));
        //StartCoroutine(movieScreen.playingCoroutine);
        //SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);

        yield break;
    }
    public void Button_ContinueGame()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);

        if (DataManager.Instance.isCreatedNewPlayerData) //데이터가 없었던 상태라면
        {
            UIManager.Instance.OpenPopup(eUIText.NoPlayerData,
                StartNewGame,
                Button_Continue_Close);
        }
        else
        {
            //상호작용 불가
            Com.canvasGroup.interactable = false;
            SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        }

    }

    private void Button_Continue_OK()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);

        Com.canvasGroup.interactable = false;
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    private void Button_Continue_Close()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        UIManager.Instance.CloseTop();
    }
    public void Button_OpenSettings()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
        UIManager.Instance.OpenThis(UIName.UIOption_Main);
    }

    public void Button_QuitGame()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);

        UIManager.Instance.OpenQuitPopup();
    }
}

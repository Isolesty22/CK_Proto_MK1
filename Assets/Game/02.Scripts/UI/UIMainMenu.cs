using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIBase
{
    private void Start()
    {
        Init();
        Open();
    }
    public void TestFunc()
    {
        if (SceneChanger.Instance == null)
        {
            Debug.LogWarning("SceneChanger is Null");
        }

        StartCoroutine(SceneChanger.Instance.LoadThisScene_Joke(SceneNames.mainMenu));
    }

    public override void Init()
    {
        base.Init();
        RegisterUIManager();
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
    private readonly string str_startNewGame = "게임을 새로 시작하시겠습니까? \n 기존에 있던 데이터는 모두 삭제됩니다.";
    public void Button_StartNewGame()
    {
        UIManager.Instance.OpenPopup(str_startNewGame,
            StartNewGame, UIManager.Instance.CloseTop);
    }
    private void StartNewGame()
    {
        //상호작용 불가
        Com.canvasGroup.interactable = false;

        DataManager.Instance.currentData_player = new Data_Player();
        StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.fileName_settings));
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    private readonly string str_noPlayerData = "<b>저장된 데이터가 없습니다.</b> \n <size=30>새로운 게임을 시작합니다.</size>";
    public void Button_ContinueGame()
    {
        if (DataManager.Instance.isCreatedNewPlayerData) //데이터가 없었던 상태라면
        {
            UIManager.Instance.OpenPopup(str_noPlayerData,
                Button_Continue_OK,
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
        Com.canvasGroup.interactable = false;
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    private void Button_Continue_Close()
    {
        UIManager.Instance.CloseTop();
    }
    public void Button_OpenSettings(UIBase _uiBase)
    {
        UIManager.Instance.OpenThis(_uiBase);
    }

    public void Button_QuitGame()
    {
        UIManager.Instance.OpenPopup("게임을 종료하시겠습니까?",
            QuitGame, 
            UIManager.Instance.CloseTop);
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public override void RegisterUIManager()
    {
        UIManager.Instance.RegisterDictThis(this.GetType().Name, this);
        UIManager.Instance.RegisterListThis(this);

    }
}

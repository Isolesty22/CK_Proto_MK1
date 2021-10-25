using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CameraManager cameraManager
    {
        get
        {
            if (_cameraManager == null)
            {
                _cameraManager = FindObjectOfType<CameraManager>();
            }
            return _cameraManager;
        }
        set
        {
            _cameraManager = value;
        }
    }
    private CameraManager _cameraManager;
    public PlayerController playerController
    {
        get
        {
            if (_playerController == null)
            {
                _playerController = FindObjectOfType<PlayerController>();
            }
            return _playerController;
        }
        set
        {
            _playerController = value;
        }

    }
    private PlayerController _playerController;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;

            //   GameObject.DontDestroyOnLoad(this.gameObject);
        }

        //if (cameraManager == null)
        //{
            cameraManager = FindObjectOfType<CameraManager>();
        //}

        //if (playerController == null)
        //{
            playerController = FindObjectOfType<PlayerController>();
        //}
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StageClear();
        }
    }
    /// <summary>
    /// [임시] 다음 스테이지로 넘어갑니다.
    /// </summary>
    public void GoNextStage()
    {
        Data_Player data = new Data_Player();
        data.currentStageNumber = DataManager.Instance.currentData_player.currentStageNumber + 1;
        data.currentStageName = SceneNames.GetSceneNameUseStageNumber(data.currentStageNumber);
        data.finalStageNumber = data.currentStageNumber;
        data.finalStageName = data.currentStageName;

        DataManager.Instance.currentData_player.CopyData(data);
        SceneChanger.Instance.LoadThisScene(DataManager.Instance.currentData_player.currentStageName);
        DataManager.Instance.SaveCurrentData(DataManager.fileName_player);

    }

    /// <summary>
    /// [임시] 다음 스테이지를 열고 필드맵으로 돌아갑니다.
    /// </summary>
    public void StageClear()
    {
        DataManager.Instance.currentData_player.finalStageNumber += 1;
        DataManager.Instance.currentData_player.finalStageName = SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.finalStageNumber);

        SceneChanger.Instance.LoadThisScene(DataManager.Instance.currentData_player.finalStageName);
        DataManager.Instance.SaveCurrentData(DataManager.fileName_player);

    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}

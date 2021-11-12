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

    public TimelineManager timelineManager
    {
        get
        {
            if (_timelineManager == null)
            {
                _timelineManager = GetComponent<TimelineManager>();
            }
            return _timelineManager;
        }
        set
        {
            _timelineManager = value;
        }
    }

    private CameraManager _cameraManager;
    private PlayerController _playerController;
    private TimelineManager _timelineManager;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;

            //   GameObject.DontDestroyOnLoad(this.gameObject);
        }

        ////if (cameraManager == null)
        ////{
        //cameraManager = FindObjectOfType<CameraManager>();
        ////}

        ////if (playerController == null)
        ////{
        //playerController = FindObjectOfType<PlayerController>();
        ////}
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GoNextStage();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerController.Stat.hp = 3;
        }
    }


    /// <summary>
    /// Application.Quit;
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    #region 임시 함수들


    /// <summary>
    /// 튜토리얼을 시작합니다.
    /// </summary>
    public void StartTutorial()
    {
        SceneChanger.Instance.LoadThisScene("Stage_00");
    }

    /// <summary>
    /// [임시] 다음 스테이지로 넘어갑니다.
    /// </summary>
    public void GoNextStage()
    {
        //Data_Player data = new Data_Player();
        //data.currentStageNumber = DataManager.Instance.currentData_player.currentStageNumber + 1;
        //data.currentStageName = SceneNames.GetSceneNameUseStageNumber(data.currentStageNumber + 1);
        //data.finalStageNumber = data.currentStageNumber + 1;
        //data.finalStageName = SceneNames.GetSceneNameUseStageNumber(data.currentStageNumber + 1);

        // DataManager.Instance.currentData_player.CopyData(data);
        //SceneChanger.Instance.LoadThisScene(SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.currentStageNumber + 1));
        //DataManager.Instance.SaveCurrentData(DataManager.DataName.player);

        DataManager.Instance.currentClearStageNumber = DataManager.Instance.currentData_player.currentStageNumber;
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        //SceneChanger.Instance.LoadThisScene(SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.currentStageNumber + 1));

    }

    /// <summary>
    /// [임시] 그냥 필드맵으로 넘어갑니다.
    /// </summary>
    public void ReturnFieldMap()
    {
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    /// <summary>
    /// [임시] 다음 스테이지를 열고 필드맵으로 돌아갑니다.
    /// </summary>
    public void StageClear()
    {
        //DataManager.Instance.currentData_player.finalStageNumber += 1;
        //DataManager.Instance.currentData_player.finalStageName = SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.finalStageNumber);

        DataManager.Instance.currentClearStageNumber = DataManager.Instance.currentData_player.currentStageNumber;
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
        // SceneChanger.Instance.LoadThisScene(SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.currentStageNumber + 1));
        //DataManager.Instance.SaveCurrentData(DataManager.DataName.player);

    }
    #endregion
}

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
            playerController.Stat.hp += 1;
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
        DataManager.Instance.currentClearStageNumber = 0;
        SceneChanger.Instance.LoadThisScene("Stage_00");
    }
    public void EndTutorial()
    {
        //스테이지 1 기준 데이터 생성
        Data_Player tempData = new Data_Player();
        tempData.currentStageName = SceneNames.stage_01;
        tempData.currentStageNumber = 1;
        tempData.finalStageName = SceneNames.stage_00;
        tempData.finalStageNumber = 0;
        DataManager.Instance.currentClearStageNumber = 0;
        DataManager.Instance.currentData_player.CopyData(tempData);
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    /// <summary>
    /// 스테이지를 클리어하고싶을 때 호출합니다.
    /// </summary>
    public void StageClear()
    {
        if (playerController != null)
        {
            playerController.State.moveSystem = false;
        }

        if (SceneChanger.Instance.GetNowSceneName() == SceneNames.stage_00)
        {
            EndTutorial();
            return;
        }

        //현재 클리어한 스테이지 넘버를 현재 스테이지 넘버로 설정
        DataManager.Instance.currentClearStageNumber = DataManager.Instance.currentData_player.currentStageNumber;

        ////저장된 finalStageNumber보다 현재 클리어한 스테이지 넘버가 클 때...
        //if (DataManager.Instance.currentClearStageNumber > DataManager.Instance.currentData_player.finalStageNumber)
        //{
        //    //데이터 생성
        //    Data_Player data = new Data_Player();

        //    //혹시 몰라서 current부터 교체
        //    data.currentStageNumber = DataManager.Instance.currentClearStageNumber;
        //    data.currentStageName = SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentClearStageNumber);

        //    //현재 클리어한 스테이지로 finalStage들을 교체
        //    data.finalStageNumber = data.currentStageNumber;
        //    data.finalStageName = data.currentStageName;

        //    //데이터 설정
        //    DataManager.Instance.currentData_player.CopyData(data);
        //}

        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }
    /// <summary>
    /// 다음 스테이지로 넘어갑니다.
    /// </summary>
    public void GoNextStage()
    {
        StageClear();
        //SceneChanger.Instance.LoadThisScene(SceneNames.GetSceneNameUseStageNumber(DataManager.Instance.currentData_player.currentStageNumber + 1));
    }

    /// <summary>
    /// [임시] 그냥 필드맵으로 넘어갑니다.
    /// </summary>
    public void ReturnFieldMap()
    {
        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }

    #endregion
}

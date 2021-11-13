using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// 데이터 입/출력 관리
/// </summary>
public class DataManager : MonoBehaviour
{
    #region Instance
    private static DataManager instance;
    public static DataManager Instance;
    #endregion


    #region Class
    public class Data_Talk
    {
        public List<Dictionary<string, object>> talk_stage_00 = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> talk_stage_01 = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> talk_stage_02 = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> talk_stage_03 = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> talk_stage_04 = new List<Dictionary<string, object>>();

    }


    #endregion

    public FileManager fileManager;

    [SerializeField, Tooltip("현재 상태")]
    private eDataManagerState currentState;

    #region 파일 이름 및 경로

    [Tooltip("/DataFiles/")]
    public string dataFilePath = string.Empty;
    #endregion

    [HideInInspector]
    public string currentFilePath;

    [Tooltip("현재 반영되어있는 설정 데이터")]
    public Data_Settings currentData_settings;

    [Tooltip("현재 반영되어있는 데이터")]
    public Data_Player currentData_player;

    [HideInInspector, Tooltip("플레이어 데이터를 새로 생성했는가? \n (로드 시 Data_Player 파일이 없는 경우에 true)")]
    public bool isCreatedNewPlayerData = false;

    public Data_Talk currentData_talk = new Data_Talk();
    public List<Dictionary<string, object>> currentData_tooltip = new List<Dictionary<string, object>>();



    [Tooltip("현재 클리어한 스테이지 번호")]
    public int currentClearStageNumber = 0;

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

    //private void Start()
    //{
    //    StartCoroutine(Init_DataFiles());
    //}

    public IEnumerator Init_DataFiles()
    {
        SetCurrentState(eDataManagerState.CHECK);

        dataFilePath = Application.dataPath + "/DataFiles/";

        #region DataFiles 폴더 존재 유무 검사
        DirectoryInfo directoryInfo = new DirectoryInfo(dataFilePath);

        //DataFiles 폴더가 없으면 폴더 생성
        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
            Debug.LogWarning(dataFilePath + "폴더를 생성했습니다.");
        }
        #endregion

        #region 세팅,플레이어 파일 검사

        yield return StartCoroutine(CheckThisFile(DataName.settings));

        isCreatedNewPlayerData = false;
        yield return StartCoroutine(CheckThisFile(DataName.player));

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        #endregion
        SetCurrentState(eDataManagerState.LOAD);

        yield return StartCoroutine(LoadData_Settings());

        yield return StartCoroutine(LoadData_Player());

        yield return StartCoroutine(LoadData_Tooltip());

        SetCurrentState(eDataManagerState.FINISH);
    }

    #region CheckFiles

    /// <summary>
    // 해당 이름을 가진 파일 및 폴더들이 존재하는지 검사하고, 없으면 생성합니다.
    /// </summary>
    private IEnumerator CheckThisFile(string _fileName)
    {
        currentFilePath = dataFilePath + _fileName;

        //설정 파일이 존재하는지 검사
        yield return StartCoroutine(fileManager.IsExist(_fileName, dataFilePath));

        if (fileManager.isExist_Result == false)// 파일이 없으면
        {
            Debug.LogWarning(_fileName + " 파일이 없습니다. 새로 만든다!");

            SetCurrentState(eDataManagerState.CREATE);

            switch (_fileName)
            {
                case DataName.settings:
                    //기본값 생성
                    Data_Settings settingsData = new Data_Settings();
                    currentData_settings.CopyData(settingsData);
                    break;

                case DataName.player:
                    //플레이어 데이터를 새로 생성했다는 표시
                    isCreatedNewPlayerData = true;

                    //기본값 생성
                    Data_Player playerData = new Data_Player();
                    currentData_player.CopyData(playerData);
                    break;

                default:
                    Debug.LogError("해당하는 파일이 없습니다.");
                    break;
            }

            //데이터 저장
            yield return StartCoroutine(SaveCurrentData(_fileName));

        }
        else
        {
            Debug.Log(_fileName + " 파일이 존재합니다.");
        }

    }

    #endregion

    #region LoadData
    /// <summary>
    /// 모든 데이터를 불러와 currentData에 저장합니다.
    /// </summary>
    private IEnumerator LoadAllData()
    {
        yield return StartCoroutine(LoadData_Settings());

        yield return StartCoroutine(LoadData_Player());

    }

    /// <summary>
    /// 데이터를 불러와 currentData에 저장합니다.
    /// </summary>
    private IEnumerator LoadData_Settings()
    {
        //파일 읽어오기
        yield return StartCoroutine(fileManager.ReadText(DataName.settings, dataFilePath));

        //제대로 읽어졌으면
        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            Data_Settings loadedData = JsonUtility.FromJson<Data_Settings>(fileManager.readText_Result);

            currentData_settings = loadedData;
        }
        else // 없을 경우
        {
            Data_Settings defaultData = new Data_Settings();
            currentData_settings = defaultData;

            yield return StartCoroutine(SaveCurrentData(DataName.settings));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }

    /// <summary>
    /// 데이터를 불러와 currentData에 저장합니다.
    /// </summary>
    private IEnumerator LoadData_Player()
    {

        //파일 읽어오기
        yield return StartCoroutine(fileManager.ReadText(DataName.player, dataFilePath));

        //제대로 읽어졌으면
        if (!string.IsNullOrEmpty(fileManager.readText_Result))
        {
            Data_Player loadedData = JsonUtility.FromJson<Data_Player>(fileManager.readText_Result);

            currentData_player = loadedData;
        }
        else // 없을 경우
        {
            Data_Player defaultData = new Data_Player();
            currentData_player = defaultData;

            yield return StartCoroutine(SaveCurrentData(DataName.player));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        isCreatedNewPlayerData = CheckNewPlayerData();
    }
    private List<Dictionary<string, object>> loadData_Talk_Result = new List<Dictionary<string, object>>();
    private int stageCode = 0;
    public IEnumerator LoadData_Talk(string _stageName)
    {
        string dataName = "Data_Talk_" + _stageName;
        loadData_Talk_Result = null;
        loadData_Talk_Result = CsvReader.Read("DataFiles/Talk/" + dataName);

        stageCode = 900;

        switch (dataName)
        {
            case DataName.talk_stage_00:
                currentData_talk.talk_stage_00 = loadData_Talk_Result;
                stageCode = 900;
                break;

            case DataName.talk_stage_01:
                currentData_talk.talk_stage_01 = loadData_Talk_Result;
                stageCode = 100;

                break;

            case DataName.talk_stage_02:
                currentData_talk.talk_stage_02 = loadData_Talk_Result;
                stageCode = 200;
                break;

            case DataName.talk_stage_03:
                currentData_talk.talk_stage_03 = loadData_Talk_Result;
                stageCode = 300;

                break;

            case DataName.talk_stage_04:
                currentData_talk.talk_stage_04 = loadData_Talk_Result;
                stageCode = 400;
                break;

            default:
                Debug.LogError("해당하는 데이터가 없습니다.");
                break;
        }

        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.OnScenenLoadEnded += UpdateTalkData;
        }

        yield break;
    }

    public IEnumerator LoadData_Tooltip()
    {
        loadData_Talk_Result = null;
        currentData_tooltip = CsvReader.Read("DataFiles/" + DataName.tooltip);
        yield break;
    }
    #endregion

    public void UpdateTalkData()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.OnScenenLoadEnded -= UpdateTalkData;

        }
        UIManager.Instance.uiTalk.SetTalkData(loadData_Talk_Result);
        UIManager.Instance.uiTalk.stageCode = stageCode;
    }
    /// <summary>
    /// 플레이어 데이터가 새로 만든 플레이어 데이터와 다를게 없는지 검사합니다.
    /// </summary>
    /// <returns>기본값과 같으면 true를 반환합니다.</returns>
    private bool CheckNewPlayerData()
    {
        if (currentData_player.IsEquals(new Data_Player()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region SaveData
    /// <summary>
    /// currentData를 json파일로 저장합니다.
    /// </summary>
    /// <param name="_fileName">저장할 파일 이름</param>
    public IEnumerator SaveCurrentData(string _fileName)
    {
        SetCurrentState(eDataManagerState.SAVE);

        GameData currentData = null;

        switch (_fileName)
        {
            case DataName.settings:
                currentData = currentData_settings;
                if (GameManager.instance != null)
                {
                    if (GameManager.instance.playerController != null)
                    {
                        GameManager.instance.playerController.Key.CopyData(currentData_settings.keySetting);

                    }
                }
                break;

            case DataName.player:
                currentData = currentData_player;
                break;

            default:
                break;
        }

        //json 형식으로 변경
        string jsonData = JsonUtility.ToJson(currentData, true);
        yield return StartCoroutine(fileManager.WriteText(_fileName, jsonData, dataFilePath));


        SetCurrentState(eDataManagerState.FINISH);
    }

    #endregion

    public void SetCurrentState(eDataManagerState _state)
    {
        if (currentState != _state)
        {
            currentState = _state;
            // Debug.Log("DataManager : " + "상태 변경! " + currentState.ToString());
        }

    }

    #region legacy_SaveData

    ///// <summary>
    ///// currentData를 json파일로 저장합니다.
    ///// </summary>
    //private IEnumerator SaveData_Settings()
    //{
    //    //json 형식으로 변경
    //    string jsonData = JsonUtility.ToJson(currentData_settings, true);
    //    yield return StartCoroutine(fileManager.WriteText(DataName.settings, jsonData, dataFilePath));
    //}

    ///// <summary>
    ///// currentData를 json파일로 저장합니다.
    ///// </summary>
    //private IEnumerator SaveData_Player()
    //{
    //    //json 형식으로 변경
    //    string jsonData = JsonUtility.ToJson(currentData_player, true);
    //    yield return StartCoroutine(fileManager.WriteText(DataName.player, jsonData, dataFilePath));
    //}

    #endregion

}

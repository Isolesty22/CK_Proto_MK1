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

    public FileManager fileManager;

    [SerializeField, Tooltip("현재 상태")]
    private eDataManagerState currentState;

    #region 파일 이름 및 경로
    private const string fileName_player = "Data_Player.dat";
    private const string fileName_settings = "Data_Settings.dat";

    [Tooltip("/DataFiles/")]
    private string dataFilePath = string.Empty;
    #endregion

    [HideInInspector]
    public string currentFilePath;

    [Tooltip("현재 반영되어있는 설정 데이터")]
    public Data_Settings currentData_settings;

    [Tooltip("현재 반영되어있는 설정 데이터")]
    public Data_Player currentData_player;


    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        dataFilePath = Application.dataPath + "/DataFiles/";

        yield return StartCoroutine(CheckFiles());


    }

    #region CheckFiles
    /// <summary>
    /// 파일 및 폴더들이 존재하는지 검사하고, 없으면 생성합니다.
    /// </summary>
    private IEnumerator CheckFiles()
    {
        SetCurrentState(eDataManagerState.CHECK);

        #region DataFiles 폴더 검사
        DirectoryInfo directoryInfo = new DirectoryInfo(dataFilePath);

        //DataFiles 폴더가 없으면 폴더 생성
        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
            Debug.LogWarning(dataFilePath + "폴더를 생성했습니다.");
        }
        #endregion

        //파일 검사--

        yield return StartCoroutine(CheckThisFile(fileName_settings));

        yield return StartCoroutine(CheckThisFile(fileName_player));

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        SetCurrentState(eDataManagerState.FINISH);
    }

    /// <summary>
    // 해당 이름을 가진 파일 및 폴더들이 존재하는지 검사하고, 없으면 생성합니다.
    /// </summary>
    private IEnumerator CheckThisFile(string _fileName)
    {

        SetCurrentState(eDataManagerState.CHECK);

        currentFilePath = dataFilePath + _fileName;

        //설정 파일이 존재하는지 검사
        yield return StartCoroutine(fileManager.IsExist(_fileName, dataFilePath));

        if (fileManager.isExist_Result == false)// 파일이 없으면
        {
            Debug.LogWarning(_fileName + " 파일이 없습니다. 새로 만든다!");

            SetCurrentState(eDataManagerState.CREATE);

            switch (_fileName)
            {
                case fileName_settings:
                    //기본값 생성
                    Data_Settings settingsData = new Data_Settings();
                    currentData_settings.CopyData(settingsData);
                    break;

                case fileName_player:
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

            yield break;
        }
        else
        {
            Debug.Log(_fileName + " 파일이 존재합니다.");
            SetCurrentState(eDataManagerState.FINISH);
        }

    }

    #endregion


    private IEnumerator LoadData()
    {
        yield break;
    }

    #region SaveData
    /// <summary>
    /// currentData를 json파일로 저장합니다.
    /// </summary>
    /// <param name="_fileName">저장할 파일 이름</param>
    private IEnumerator SaveCurrentData(string _fileName)
    {
        SetCurrentState(eDataManagerState.SAVE);

        GameData currentData = null;

        switch (_fileName)
        {
            case fileName_settings:
                currentData = currentData_settings;
                break;

            case fileName_player:
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
            Debug.Log("DataManager : " + "상태 변경! " + currentState.ToString());
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
    //    yield return StartCoroutine(fileManager.WriteText(fileName_settings, jsonData, dataFilePath));
    //}

    ///// <summary>
    ///// currentData를 json파일로 저장합니다.
    ///// </summary>
    //private IEnumerator SaveData_Player()
    //{
    //    //json 형식으로 변경
    //    string jsonData = JsonUtility.ToJson(currentData_player, true);
    //    yield return StartCoroutine(fileManager.WriteText(fileName_player, jsonData, dataFilePath));
    //}

    #endregion

}

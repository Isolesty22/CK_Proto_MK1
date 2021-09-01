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



    #region 파일 이름
    private const string fileName_player = "Data_Player.dat";
    private const string fileName_settings = "Data_Settings.dat";
    #endregion
    [Tooltip("/DataFiles/")]
    private string dataFilePath = string.Empty;

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
        dataFilePath = Application.dataPath + "/DataFiles/";
    }

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        StartCoroutine(CheckFiles());
    }


    private string filePath;
    /// <summary>
    /// 파일 및 폴더들이 존재하는지 검사하고, 없으면 생성합니다.
    /// </summary>
    private IEnumerator CheckFiles()
    {
        currentState = eDataManagerState.CHECK;

        #region DataFiles 폴더 검사
        DirectoryInfo directoryInfo = new DirectoryInfo(dataFilePath);

        //DataFiles 폴더가 없으면 폴더 생성
        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
        }
        #endregion

        //파일 검사--

        yield return StartCoroutine(CheckThisFile(fileName_settings));

        yield return StartCoroutine(CheckThisFile(fileName_player));


        yield return null;

    }
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

            //파일 생성
            fileManager.CreateFile(currentFilePath);

            switch (_fileName)
            {
                case fileName_settings:
                    //기본값 생성
                    Data_Settings settingsData = new Data_Settings();
                    currentData_settings.CopyData(settingsData);

                    //기본 데이터 저장
                    yield return StartCoroutine(SaveData_Settings());
                    break;


                case fileName_player:
                    //기본값 생성
                    Data_Player playerData = new Data_Player();
                    currentData_player.CopyData(playerData);

                    //기본 데이터 저장
                    yield return StartCoroutine(SaveData_Player());
                    break;


                default:
                    Debug.LogError("해당하는 파일이 없습니다.");
                    break;
            }


        }
        else
        {
            Debug.Log(_fileName + " 파일이 존재합니다.");
        }
    }

    /// <summary>
    /// currentData를 json파일로 저장합니다.
    /// </summary>
    private IEnumerator SaveData_Settings()
    {
        //json 형식으로 변경
        string jsonData = JsonUtility.ToJson(currentData_settings, true);
        yield return StartCoroutine(fileManager.WriteText(fileName_settings, jsonData, dataFilePath));
    }

    /// <summary>
    /// currentData를 json파일로 저장합니다.
    /// </summary>
    private IEnumerator SaveData_Player()
    {
        //json 형식으로 변경
        string jsonData = JsonUtility.ToJson(currentData_player, true);
        yield return StartCoroutine(fileManager.WriteText(fileName_player, jsonData, dataFilePath));
    }


}

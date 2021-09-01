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


    [HideInInspector]
    public string currentFilePath;


    #region 파일 이름
    private string fileName_player = "Data_Player.dat";
    private string fileName_settings = "Data_Settings.dat";
    #endregion

    [SerializeField, Tooltip("현재 상태")]
    private eDataManagerState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }


    public void Init()
    {

    }



    private string filePath;
    private DirectoryInfo directoryInfo;
    /// <summary>
    /// 파일 및 폴더들이 존재하는지 검사합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckFileExist()
    {

        yield return null;
    }

}

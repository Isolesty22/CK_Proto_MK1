using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private FileBase fileBase;

    #region legacy
//    private void Awake()
//    {

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN

//        fileBase = new FileBaseWindows();
//#endif

//#if UNITY_PS4
//                fileBase = new FileBasePS4();
//#endif

//    }
    #endregion
    public IEnumerator WriteText(string dataName, string data, string path)
    {
        yield return StartCoroutine(fileBase.WriteText(dataName, data, path)); // 이 코루틴이 끝날 때 까지.

    }

    [HideInInspector] public string readText_Result;

    /// <summary>
    /// 파일을 읽은 후, ,readText_Result에 저장합니다.
    /// </summary>
    /// <param name="dataName">파일의 이름을 적습니다. 예시 : "testFile.png"  </param>
    /// <param name="dataPath">파일의 이름을 제외한 경로를 적습니다. 예시 : "Assets/data/" </param>
    public IEnumerator ReadText(string dataName, string path)
    {

        readText_Result = string.Empty;
        yield return StartCoroutine(fileBase.ReadText(dataName, path));
        readText_Result = fileBase.readText_Result;
    }

    [HideInInspector] public bool isExist_Result;


    /// <summary>
    /// 해당 파일이 존재하는지 확인합니다. 결과는 isExist_Result에 저장됩니다.
    /// </summary>
    /// <param name="dataName">파일의 이름을 적습니다. 예시 : "testFile.png"  </param>
    /// <param name="dataPath">파일의 이름을 제외한 경로를 적습니다. 예시 : "Assets/data/" </param>
    public IEnumerator IsExist(string dataName, string dataPath)
    {
        yield return StartCoroutine(fileBase.IsExist(dataName, dataPath));
        isExist_Result = fileBase.isExist_result;
    }
}

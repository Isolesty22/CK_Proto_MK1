using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private FileBase fileBase;

    private void Awake()
    {
        #region legacy
        //#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        //        fileBase = new FileBaseWindows();
        //#endif

        //#if UNITY_PS4
        //        fileBase = new FileBasePS4();
        //#endif
        #endregion
    }
    public IEnumerator WriteText(string dataName, string data, string path)
    {
        yield return StartCoroutine(fileBase.WriteText(dataName, data, path)); // �� �ڷ�ƾ�� ���� �� ����.

    }

    [HideInInspector] public string readText_Result;

    /// <summary>
    /// ������ ���� ��, ,readText_Result�� �����մϴ�.
    /// </summary>
    /// <param name="dataName">������ �̸��� �����ϴ�. ���� : "testFile.png"  </param>
    /// <param name="dataPath">������ �̸��� ������ ��θ� �����ϴ�. ���� : "Assets/data/" </param>
    /// <returns></returns>
    public IEnumerator ReadText(string dataName, string path)
    {

        readText_Result = string.Empty;
        yield return StartCoroutine(fileBase.ReadText(dataName, path));
        readText_Result = fileBase.readText_Result;
    }

    [HideInInspector] public bool isExist_Result;


    /// <summary>
    /// �ش� ������ �����ϴ��� Ȯ���մϴ�. ����� isExist_Result�� ����˴ϴ�.
    /// </summary>
    /// <param name="dataName">������ �̸��� �����ϴ�. ���� : "testFile.png"  </param>
    /// <param name="dataPath">������ �̸��� ������ ��θ� �����ϴ�. ���� : "Assets/data/" </param>
    /// <returns></returns>
    public IEnumerator IsExist(string dataName, string dataPath)
    {
        yield return StartCoroutine(fileBase.IsExist(dataName, dataPath));
        isExist_Result = fileBase.isExist_Result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// Windows OS������ FileBase�Դϴ�.
/// </summary>
public class FileBaseWindows : FileBase
{

//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public override IEnumerator WriteText(string _dataName, string _data, string _path)
    {
        string path = _path + _dataName;

        File.WriteAllText(path, _data);
        yield break;
    }
    /// <summary>
    /// �ؽ�Ʈ ���� ������ �о�ɴϴ�. ����� readText_Result�� ����˴ϴ�.
    /// </summary>
    /// <param name="_dataName">������ �̸�. ���� : testFile.png</param>
    /// <returns></returns>
    public override IEnumerator ReadText(string _dataName, string _path)
    {
        readText_Result = string.Empty;
        //string path = GetDataLocation_Persistent() + dataName;
        string path = _path + _dataName;

        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists == true)
        {
            readText_Result = File.ReadAllText(path);
        }
        else
        {
            Debug.LogWarning("������ �������� �ʽ��ϴ�." + " [" + _dataName + "]");
            //���� �б� ����
        }

        yield break;
    }


    /// <summary>
    /// Application.dataPath : ��������/��������_Data/
    /// </summary>
    public override string GetDataLocation_DataPath()
    {
        return Application.dataPath + "/";
        //�������� ��쿡�� ���� ��� �ڿ� / �ٿ���� ���� ���ó�� �Ǿ������ ������...����? 
    }


    /// <summary>
    /// ������ �����ϴ��� Ȯ�����ϴ�. ����� isExit_Result�� ����˴ϴ�.
    /// </summary>
    /// <param name="dataName">������ �̸�. ���� : testFile.png</param>
    public override IEnumerator IsExist(string dataName, string dataPath)
    {
        string path = dataPath + dataName;
        isExist_result = File.Exists(path);
        yield break;
    }
//#endif
}

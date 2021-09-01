using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBase
{
    public virtual IEnumerator WriteText(string _dataName, string _data, string _path)
    {
        yield break;
    }

    public string readText_Result;
    public virtual IEnumerator ReadText(string _dataName, string _path)
    {
        yield break;
    }

    /// <summary>
    /// Application.dataPath�� ����ؼ� ��θ� �����ɴϴ�.
    /// </summary>
    public virtual string GetDataLocation_DataPath()
    {
        return string.Empty;
    }

    /// <summary>
    /// IsExist�� �����
    /// </summary>
    public bool isExist_result;
    public virtual IEnumerator IsExist(string dataName, string dataPath)
    {
        yield break;
    }

}

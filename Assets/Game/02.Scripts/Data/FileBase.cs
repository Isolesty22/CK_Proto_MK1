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
    /// Application.dataPath를 사용해서 경로를 가져옵니다.
    /// </summary>
    public virtual string GetDataLocation_DataPath()
    {
        return string.Empty;
    }

    /// <summary>
    /// IsExist의 결과값
    /// </summary>
    public bool isExist_result;
    public virtual IEnumerator IsExist(string dataName, string dataPath)
    {
        yield break;
    }

    /// <summary>
    /// 파일을 생성합니다.
    /// </summary>
    /// <param name="_path">파일, 파일 확장자를 포함한 전체 경로</param>
    public virtual void CreateFile(string _path)
    {
   
    }

}

using System.Collections;
using System.Collections.Generic;
using System.IO;
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


    public AudioClip getAudioClip_Result;
    public virtual IEnumerator GetAudioClip(string _fileName)
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
    /// 파일을 생성합니다. 근데 C#에서는 File.Create()를 할 필요없다고 합니다...
    /// https://answers.unity.com/questions/990496/ioexception-sharing-violation-on-path-please-help.html
    /// </summary>
    /// <param name="_path">파일, 파일 확장자를 포함한 전체 경로</param>
    public virtual void CreateFile(string _path)
    {

    }

    /// <summary>
    /// 오디오 파일 이름에서 확장자를 가져와 AudioType으로 변환합니다.
    /// </summary>
    /// <param name="_fileName">확장자를 포함한 파일 이름</param>
    protected AudioType GetAudioType(string _fileName)
    {
        string[] tempExtension = _fileName.Split('.');


        //마지막이 확장자일테니...
        switch (tempExtension[tempExtension.Length - 1])
        {
            case "wav":
            case "WAV":
                return AudioType.WAV;

            case "ogg":
            case "OGG":
                return AudioType.OGGVORBIS;

            case "mp3":
            case "MP3":
                return AudioType.MPEG;
            default:
                return AudioType.UNKNOWN;
        }
    }

}

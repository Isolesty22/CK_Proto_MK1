using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eResolutionType
{
    /// <summary>
    /// 1280x720
    /// </summary>
    HD,

    /// <summary>
    /// 1920x1080
    /// </summary>
    FHD,

    /// <summary>
    /// 3840x2160
    /// </summary>
    UHD,
}

public enum eDataManagerState
{

    DEFAULT,

    /// <summary>
    /// 데이터 파일 검사 중
    /// </summary>
    CHECK,

    /// <summary>
    /// 데이터 파일을 생성하는 중
    /// </summary>
    CREATE,

    /// <summary>
    /// 데이터 파일을 읽는 중
    /// </summary>
    LOAD,

    /// <summary>
    /// 데이터 파일을 적용하는 중
    /// </summary>
    SAVE,

    /// <summary>
    /// 작업 완료
    /// </summary>
    FINISH,

    /// <summary>
    /// 무...무슨일이지?
    /// </summary>
    ERROR,
    
}

public class EnumCollection : MonoBehaviour
{

}

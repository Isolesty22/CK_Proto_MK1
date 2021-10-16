using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePhase
{
    Phase_1,
    Phase_2,
    Phase_3,
    Phase_Finish
}

public enum eBearState
{
    None,
    [InspectorName("대기")]
    Idle,

    [InspectorName("발구르기")]
    Stamp,

    [InspectorName("포효_A")]
    Roar_A,
    [InspectorName("포효_B")]
    Roar_B,

    [InspectorName("내려치기_A")]
    Strike_A,
    [InspectorName("내려치기_B")]
    Strike_B,
    [InspectorName("내려치기_C")]
    Strike_C,

    [InspectorName("할퀴기_A")]
    Claw_A,
    [InspectorName("할퀴기_B")]
    Claw_B,
    [InspectorName("할퀴기_C")]
    Claw_C,

    [InspectorName("스매쉬")]
    Smash,

    [InspectorName("집중")]
    Concentrate,


    [InspectorName("랜덤 1번")]
    Random_1,

    [InspectorName("랜덤 2번")]
    Random_2,

    [InspectorName("랜덤 3번")]
    Random_3,

    [InspectorName("랜덤 4번")]
    Random_4,


    Powerless,

    [InspectorName("페이즈2 전환 연출(돌진)")]
    Rush,

    [InspectorName("페이즈3 전환 연출(걷기)")]
    FinalWalk,

    [InspectorName("죽음")]
    Die,
}

public enum eGloomState
{

}

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

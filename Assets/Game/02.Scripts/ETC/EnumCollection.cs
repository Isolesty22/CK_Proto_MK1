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
    [InspectorName("곰_대기")]
    Idle,

    [InspectorName("곰_발구르기")]
    Stamp,

    [InspectorName("곰_포효_A")]
    Roar_A,
    [InspectorName("곰_포효_B")]
    Roar_B,

    [InspectorName("곰_내려치기_A")]
    Strike_A,
    [InspectorName("곰_내려치기_B")]
    Strike_B,
    [InspectorName("곰_내려치기_C")]
    Strike_C,

    [InspectorName("곰_할퀴기_A")]
    Claw_A,
    [InspectorName("곰_할퀴기_B")]
    Claw_B,
    [InspectorName("곰_할퀴기_C")]
    Claw_C,

    [InspectorName("곰_스매쉬")]
    Smash,

    [InspectorName("곰_집중")]
    Concentrate,


    [InspectorName("곰_페이즈1 랜덤_1")]
    Random_Phase1_1,
    [InspectorName("곰_페이즈1 랜덤_2")]
    Random_Phase1_2,
    [InspectorName("곰_페이즈2 랜덤_1")]
    Random_Phase2_1,
    [InspectorName("곰_페이즈2 랜덤_2")]
    Random_Phase2_2,
    [InspectorName("곰_페이즈3 랜덤_1")]
    Random_Phase3_1,
    [InspectorName("곰_페이즈3 랜덤_2")]
    Random_Phase3_2,

    Powerless,

    [InspectorName("곰_페이즈2 전환 연출(돌진)")]
    Rush,
    [InspectorName("곰_페이즈3 전환 연출(걷기)")]
    FinalWalk,

    [InspectorName("곰_죽음")]
    Die,
}

public enum eBossState
{
    None,
    [InspectorName("곰_대기")]
    BearState_Idle,

    [InspectorName("곰_발구르기")]
    BearState_Stamp,

    [InspectorName("곰_포효_A")]
    BearState_Roar_A,
    [InspectorName("곰_포효_B")]
    BearState_Roar_B,

    [InspectorName("곰_내려치기_A")]
    BearState_Strike_A,
    [InspectorName("곰_내려치기_B")]
    BearState_Strike_B,
    [InspectorName("곰_내려치기_C")]
    BearState_Strike_C,

    [InspectorName("곰_할퀴기_A")]
    BearState_Claw_A,
    [InspectorName("곰_할퀴기_B")]
    BearState_Claw_B,
    [InspectorName("곰_할퀴기_C")]
    BearState_Claw_C,

    [InspectorName("곰_스매쉬")]
    BearState_Smash,

    [InspectorName("곰_집중")]
    BearState_Concentrate,


    [InspectorName("곰_페이즈1 랜덤_1")]
    BearState_Random_Phase1_1,
    [InspectorName("곰_페이즈1 랜덤_2")]
    BearState_Random_Phase1_2,
    [InspectorName("곰_페이즈2 랜덤_1")]
    BearState_Random_Phase2_1,
    [InspectorName("곰_페이즈2 랜덤_2")]
    BearState_Random_Phase2_2,
    [InspectorName("곰_페이즈3 랜덤_1")]
    BearState_Random_Phase3_1,
    [InspectorName("곰_페이즈3 랜덤_2")]
    BearState_Random_Phase3_2,

    BearState_Powerless,

    [InspectorName("곰_페이즈2 전환 연출(돌진)")]
    BearState_Rush,
    [InspectorName("곰_페이즈3 전환 연출(걷기)")]
    BearState_FinalWalk,

    [InspectorName("곰_죽음")]
    BearState_Die,
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

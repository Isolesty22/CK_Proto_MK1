using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePhase
{
    Phase_1,
    Phase_2,
    //  Phase_3,
    //  Phase_Finish
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

    [InspectorName("할퀴기_A")]
    Claw_A,
    [InspectorName("할퀴기_B")]
    Claw_B,

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

    [InspectorName("죽음")]
    Die
}

public enum eGloomState
{
    [InspectorName("대기/None")]
    None,

    [InspectorName("대기/Idle(사용금지)")]
    Idle,

    [InspectorName("추격")]
    Chase,

    [InspectorName("도약")]
    Leap,

    [InspectorName("공명")]
    Resonance,

    [InspectorName("위협")]
    Threat,

    [InspectorName("가시숲")]
    ThornForest,

    [InspectorName("방해")]
    Obstruct,

    [InspectorName("가시밭길")]
    ThornPath,

    [InspectorName("파동")]
    Wave,

    [InspectorName("사용금지/전진")]
    Advance,

    [InspectorName("사용금지/광폭화")]
    Berserk,

    [InspectorName("사용금지/무력화")]
    Powerless,

    [InspectorName("사용금지/죽음")]
    Die

}
public enum eUIText
{
    StartNewGame,
    NoPlayerData,
    DataDelete,
    DataSave,
    Exit,


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


public enum eDiretion
{
    Left,
    Right,
    Up,
    Down
}


[System.Serializable]
public class BoxColliderInfo
{
    public Vector3 center;
    public Vector3 size;
}

[System.Serializable]
public class MapBlock
{
    [System.Serializable]
    public class Positions
    {
        [ReadOnly]
        public Vector3 min;

        [ReadOnly]
        public Vector3 max;

        /// <summary>
        /// 땅부분의 가운데 포지션입니다.
        /// </summary>
        [ReadOnly]
        public Vector3 groundCenter;

        [ReadOnly]
        public Vector3 topCenter;
    }
    public enum eType
    {
        /// <summary>
        /// 기본 타입입니다.
        /// </summary>
        None,
        /// <summary>
        /// 낭떠러지에게 부여되는 타입입니다.
        /// </summary>
        Empty,

        /// <summary>
        /// 가시덩쿨등 중복되면 안되는 오브젝트들이 소환된 상태일 때 부여되는 타입입니다.
        /// </summary>
        Used,
    }
    private eType originType;
    [ReadOnly]
    public eType currentType;

    public Positions positions = new Positions();

    public void SetOriginType(eType _type)
    {
        originType = _type;
    }
    public void SetMinMax(Vector3 _min, Vector3 _max)
    {
        positions.min = _min;
        positions.max = _max;
    }

    public void SetGroundCenter(Vector3 _groundCenter)
    {
        positions.groundCenter = _groundCenter;
    }
    public void SetTopCenter(Vector3 _topCenter)
    {
        positions.topCenter = _topCenter;
    }

    public void SetCurrentType(eType _type)
    {
        currentType = _type;
    }
    public void SetCurrentTypeToOrigin()
    {
        currentType = originType;
    }

}

/// <summary>
/// 데미지를 받을 수 있는 오브젝트에게 상속합니다.
/// </summary>
public interface IDamageable
{
    public void OnHit();
    public void ReceiveDamage();
}

public static class TagName
{
    public static readonly string Player = "Player";
    public static readonly string Arrow = "Arrow";
    public static readonly string ParryingObject = "ParryingObject";
    public static readonly string Monster = "Monster";
    public static readonly string Boss = "Boss";
    // public static readonly string  = "FieldMap";
}

public static class DataName
{
    public const string settings = "Data_Settings.dat";
    public const string player = "Data_Player.dat";
    public const string tooltip = "Data_Tooltip";
    public const string talk_stage_00 = "Data_Talk_Stage_00";
    public const string talk_stage_01 = "Data_Talk_Stage_01";
    public const string talk_stage_02 = "Data_Talk_Stage_02";
    public const string talk_stage_03 = "Data_Talk_Stage_03";
    public const string talk_stage_04 = "Data_Talk_Stage_04";

}
public static class UIName
{
    public static readonly string UILosePopup = "UILosePopup";
    public static readonly string UIKeySetting = "UIKeySetting";
    public static readonly string UIVolumeSetting = "UIVolumeSetting";
    public static readonly string UIPause = "UIPause";
    public static readonly string UITalk = "UITalk";

    /// <summary>
    /// 필드맵에 있는 옵션창입니다.
    /// </summary>
    public static readonly string UIOption_Field = "UIOption_Field";

    /// <summary>
    /// 메인메뉴에 있는 옵션창입니다.
    /// </summary>
    public static readonly string UIOption_Main = "UIOption_Main";

}


public static class SceneNames
{
    public static readonly string mainMenu = "MainMenu";
    public static readonly string title = "Title";
    public static readonly string fieldMap = "FieldMap";

    public const string stage_00 = "Stage_00";
    public const string stage_01 = "Stage_01";
    public const string stage_02 = "Stage_02";
    public const string stage_03 = "Stage_03";
    public const string stage_04 = "Stage_04";

    public static string GetSceneNameUseStageNumber(int _number)
    {
        string str = "Stage_";
        switch (_number)
        {
            case 0:
                return str + "00";
            case 1:
                return str + "01";
            case 2:
                return str + "02";
            case 3:
                return str + "03";
            case 4:
                return str + "04";
            default:
                return str + "00";
        }
    }

}

public class EnumCollection : MonoBehaviour
{

}

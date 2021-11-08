using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{

}

[System.Serializable]
public class Data_Settings : GameData
{
    //볼륨값
    public string volume_master;
    public string volume_sfx;
    public string volume_bgm;

    //음소거 상태
    public bool isMute_master;
    public bool isMute_sfx;
    public bool isMute_bgm;

    //해상도
    public eResolutionType resolutionType;

    //키세팅
    public KeyOption keySetting;

    /// <summary>
    /// 디폴트 생성자
    /// </summary>
    public Data_Settings()
    {
        volume_master = "0.7";
        volume_bgm = "0.5";
        volume_sfx = "0.5";

        isMute_master = false;
        isMute_bgm = false;
        isMute_sfx = false;

        resolutionType = eResolutionType.FHD;
        keySetting = new KeyOption();
    }
    public Data_Settings(Data_Settings _data)
    {
        CopyData(_data);
    }

    /// <summary>
    /// 현재 데이터를 해당 데이터로 설정합니다.
    /// </summary>
    public void CopyData(Data_Settings _data)
    {
        volume_master = _data.volume_master;
        volume_bgm = _data.volume_bgm;
        volume_sfx = _data.volume_sfx;

        isMute_master = _data.isMute_master;
        isMute_bgm = _data.isMute_bgm;
        isMute_sfx = _data.isMute_sfx;

        resolutionType = eResolutionType.FHD;

        keySetting.CopyData(_data.keySetting);
    }


    /// <summary>
    /// 데이터 내용이 같은가?
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public bool IsEquals(Data_Settings _data)
    {
        return volume_master == _data.volume_master &&
               volume_sfx == _data.volume_sfx &&
               volume_bgm == _data.volume_bgm &&
               isMute_master == _data.isMute_master &&
               isMute_sfx == _data.isMute_sfx &&
               isMute_bgm == _data.isMute_bgm &&
               resolutionType == _data.resolutionType &&
               keySetting.IsEquals(_data.keySetting);
    }
}


[System.Serializable]
public class Data_Player : GameData
{
    /// <summary>
    /// 현재 플레이어가 존재하는 스테이지
    /// </summary>
    public string currentStageName;
    public int currentStageNumber;

    /// <summary>
    /// 최대 기록?
    /// </summary>
    public string finalStageName;
    public int finalStageNumber;

    /// <summary>
    /// 디폴트 생성자
    /// </summary>
    public Data_Player()
    {
        currentStageName = "Stage_01";
        currentStageNumber = 1;
        finalStageName = "Stage_00";
        finalStageNumber = 0;
    }

    public Data_Player(Data_Player _data)
    {
        CopyData(_data);
    }

    /// <summary>
    /// 현재 데이터를 해당 데이터로 설정합니다.
    /// </summary>
    public void CopyData(Data_Player _data)
    {
        currentStageName = _data.currentStageName;
        currentStageNumber = _data.currentStageNumber;
        finalStageName = _data.finalStageName;
        finalStageNumber = _data.finalStageNumber;
    }

    /// <summary>
    /// 데이터 내용이 같은가?
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public bool IsEquals(Data_Player _data)
    {
        return currentStageName == _data.currentStageName &&
               currentStageNumber == _data.currentStageNumber &&
               finalStageName == _data.finalStageName &&
               finalStageNumber == _data.finalStageNumber;
    }

}

public static class SceneNames
{
    public static readonly string mainMenu = "MainMenu";
    public static readonly string loading = "Loading";
    public static readonly string fieldMap = "FieldMap";
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

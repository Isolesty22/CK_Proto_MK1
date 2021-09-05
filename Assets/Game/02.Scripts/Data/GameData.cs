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
               resolutionType == _data.resolutionType;
    }
}


[System.Serializable]
public class Data_Player : GameData
{
    /// <summary>
    /// 현재 스테이지 이름(씬이름?)
    /// </summary>
    public string currentStageName;

    /// <summary>
     /// 디폴트 생성자
     /// </summary>
    public Data_Player()
    {
        currentStageName = "NONE";
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
    }
}

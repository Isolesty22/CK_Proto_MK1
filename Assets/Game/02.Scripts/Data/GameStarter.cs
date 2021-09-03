using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임을 처음 시작할 때의 첫 로딩을 관리해주는 매니저
/// </summary>
public class GameStarter : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.Init_DataFiles());
        Debug.Log("데이터 파일을 불러왔습니다.");

        yield return StartCoroutine(AudioManager.Instance.Init());
        Debug.Log("오디오 파일을 불러왔습니다.");
    }

}

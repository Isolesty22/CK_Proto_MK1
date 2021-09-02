using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiwonTestRoutine : MonoBehaviour
{

    public bool isPlay = false;
    private IEnumerator Start()
    {
        //if (isPlay == false)
        //{
            yield return StartCoroutine(DataManager.Instance.Init_DataFiles());
            Debug.Log("데이터 파일을 불러왔습니다.");

            yield return StartCoroutine(AudioManager.Instance.Init());
            Debug.Log("오디오 파일을 불러왔습니다.");
        //}
        //else
        //{
            StartCoroutine(AudioManager.Instance.TestPlayBgmLoop());
        //}

    }
}

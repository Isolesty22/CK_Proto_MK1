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
            Debug.Log("������ ������ �ҷ��Խ��ϴ�.");

            yield return StartCoroutine(AudioManager.Instance.Init());
            Debug.Log("����� ������ �ҷ��Խ��ϴ�.");
        //}
        //else
        //{
            StartCoroutine(AudioManager.Instance.TestPlayBgmLoop());
        //}

    }
}

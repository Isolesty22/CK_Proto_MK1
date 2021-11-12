using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using UnityEngine.Events;

public class JiwonTestRoutine : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.LoadData_Talk("Stage_00"));
        DataManager.Instance.OnSceneLoadEnded();

    }
    private int tempTalkCode = 900;
    public void Button_Talk()
    {
        UIManager.Instance.Talk(tempTalkCode);
        tempTalkCode += 1;
    }
}



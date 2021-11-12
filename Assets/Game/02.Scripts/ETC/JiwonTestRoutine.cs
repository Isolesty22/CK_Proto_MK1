using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using UnityEngine.Events;

public class JiwonTestRoutine : MonoBehaviour
{

    private void Start()
    {
        //DataManager.Instance.StartLoadData_Talk("Stage_00");

    }
    private int tempTalkCode = 900;
    public void Button_Talk()
    {
        UIManager.Instance.Talk(tempTalkCode);
        tempTalkCode += 1;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
//using UnityEngine.Events;

public class JiwonTestRoutine : MonoBehaviour
{
    public KeyInputDetector keyInputDetector;
    public Text text;

    public KeyCode changedKey;

    private void Start()
    {
        changedKey = keyInputDetector.GetKeyCode("Space");
        text.text = changedKey.ToString();
    }
    public void Button_DoDetecting()
    {
        keyInputDetector.StartDetect();
        StartCoroutine(ProcessKeyChange());
    }

    private IEnumerator ProcessKeyChange()
    {
        while (true)
        {
            if (!keyInputDetector.isDetecting)
            {
                break;
            }

            yield return null;
        }

        //키 감지가 끝나면
        changedKey = keyInputDetector.currentKeyCode;
        text.text = changedKey.ToString();
        yield break;
        
    }

}

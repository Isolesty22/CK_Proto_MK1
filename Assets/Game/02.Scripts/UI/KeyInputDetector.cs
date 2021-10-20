using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// 키 입력을 한 번 감지합니다.
/// </summary>
public class KeyInputDetector : MonoBehaviour
{
    private Event currentEvent = null;

    public bool isDetecting = false;

    [HideInInspector]
    public KeyCode currentKeyCode = KeyCode.None;


    /// <summary>
    /// 키 입력 감지를 시작합니다. 입력된 키는 currentKeyCode에 저장됩니다.
    /// </summary>
    public void StartDetect()
    {
        isDetecting = true;
        gameObject.SetActive(true);
    }
    public void EndDetect()
    {
        isDetecting = false;
        gameObject.SetActive(false);
    }
    private void OnGUI()
    {
        if (!isDetecting)
        {
            return;
        }

        currentEvent = Event.current;
        //isDetecting
        if (currentEvent.isKey)
        {
            currentKeyCode = currentEvent.keyCode;
            Debug.LogWarning("[" + currentKeyCode.ToString() + "]" + "키가 입력되었습니다.");

            currentKeyCode = GetKeyCode(currentKeyCode.ToString());

            isDetecting = false;
            this.gameObject.SetActive(false);
        }
    }

    public KeyCode GetKeyCode(string _keyCode)
    {
        KeyCode tempKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), _keyCode);
        return tempKeyCode;
    }


}

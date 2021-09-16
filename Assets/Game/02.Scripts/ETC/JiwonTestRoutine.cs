using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
//using UnityEngine.Events;

[System.Serializable]
public class KeyChangeButton
{
    public string keyType;
    public Button button;
    public Text text;
}
public class JiwonTestRoutine : MonoBehaviour
{
    public KeyInputDetector keyInputDetector;
    public Text text;

    public KeyCode changedKey;

    public Data_KeySetting data_keySetting;

    public List<KeyChangeButton> keyButtonList = new List<KeyChangeButton>();
    private void Start()
    {
        //changedKey = keyInputDetector.GetKeyCode("Space");
        //text.text = changedKey.ToString();

    }

    private bool isChangingKey = false;

    public delegate void ChangeKey();
    private ChangeKey changeKey;

    private const string str_moveRight = "moveRight";
    private const string str_moveLeft = "moveLeft";
    private const string str_crouch = "crouch";
    private const string str_lookUp = "lookUp";
    private const string str_attack = "attack";
    private const string str_jump = "jump";
    private const string str_counter = "counter";

    public void Button_InputChangeKey(string _keyType)
    {
        if (isChangingKey)
        {
            return;
        }

        isChangingKey = true;
        StartCoroutine(WaitInputKey(_keyType));
    }
    private IEnumerator WaitInputKey(string _keyType)
    {
        while (true)
        {
            if (!keyInputDetector.isDetecting)
            {
                break;
            }

            yield return null;
        }

      //  ChangeThisKey(_keyType);
        isChangingKey = false;
    }

    public void ChangeKey_moveRight()
    {

    }
}



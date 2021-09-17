using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Reflection;
//using UnityEngine.Events;

[System.Serializable]
public class KeyChangeButton
{
    public string keyType;
    public Text text;
}
public class JiwonTestRoutine : MonoBehaviour
{
    public KeyInputDetector keyInputDetector;
    public Text text;

    public KeyCode changedKey;

    public Data_KeySetting data_keySetting;

    public List<KeyChangeButton> keyButtonList = new List<KeyChangeButton>();

    private Dictionary<string, KeyChangeButton> keyTextDict = new Dictionary<string, KeyChangeButton>();

    /// <summary>
    /// ex: <A, "moveLeft">
    /// </summary>
    private Dictionary<KeyCode, string> keyInfoDict = new Dictionary<KeyCode, string>();


    public void Init()
    {
        for (int i = 0; i < keyButtonList.Count; i++)
        {
            keyTextDict.Add(keyButtonList[i].keyType, keyButtonList[i]);
        }

        for (int i = 0; i < keyButtonList.Count; i++)
        {
            FieldInfo _testField = data_keySetting.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);

            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keySetting), _testField.Name);

            UpdateKeyText(keyButtonList[i].keyType, _testField.GetValue(data_keySetting).ToString());
        }
    }

    private void Awake()
    {
        Init();
    }

    private bool isChangingKey = false;

    public void Button_InputChangeKey(string _keyType)
    {
        if (isChangingKey)
        {
            return;
        }

        isChangingKey = true;
        StartCoroutine(WaitInputKey(_keyType));
    }
    /// <summary>
    /// 키 입력을 기다리고, 키가 입력되었으면 키를 바꿉니다.
    /// </summary>
    /// <param name="_keyType"></param>
    /// <returns></returns>
    private IEnumerator WaitInputKey(string _keyType)
    {
        keyInputDetector.StartDetect();
        Debug.LogWarning("Wait Input Key...");
        while (true)
        {
            if (!keyInputDetector.isDetecting)
            {
                break;
            }

            yield return null;
        }

        ChangeThisKey(_keyType);
        isChangingKey = false;
    }

    public void ChangeThisKey(string _keyType)
    {
        //사용하고 있는 키가 아니라면!!
        if (IsUsedKey(keyInputDetector.currentKeyCode))
        {
            return;
        }

        FieldInfo _testField = data_keySetting.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);

        if (ReferenceEquals(_testField, null))
        {
            Debug.LogError("존재하지 않는 KeyType입니다("+_keyType+"). 버튼 OnClick을 확인하세요.");
            return;
        }
        _testField.SetValue(data_keySetting, keyInputDetector.currentKeyCode);
        UpdateKeyText(_keyType, _testField.GetValue(data_keySetting).ToString());

    }

    public bool IsUsedKey(KeyCode _inputKey)
    {
        string tempKeyType;


        //이미 키가 존재할경우
        if (keyInfoDict.TryGetValue(_inputKey, out tempKeyType))
        {
            Debug.LogError("해당 키는 이미 " + tempKeyType + "에 할당되어있습니다.");
            return true;
        }
        else //존재하지 않을경우
        {
            if (_inputKey == KeyCode.Escape)
            {
                Debug.LogError(_inputKey.ToString() + "키는 사용하실 수 없습니다.");
                return true;
            }

            return false;
        }
    }
    public void UpdateKeyText(string _keyType, string _changedKey)
    {
        keyTextDict[_keyType].text.text = _changedKey;
    }
}



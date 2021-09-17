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
    /// Ű �Է��� ��ٸ���, Ű�� �ԷµǾ����� Ű�� �ٲߴϴ�.
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
        //����ϰ� �ִ� Ű�� �ƴ϶��!!
        if (IsUsedKey(keyInputDetector.currentKeyCode))
        {
            return;
        }

        FieldInfo _testField = data_keySetting.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);

        if (ReferenceEquals(_testField, null))
        {
            Debug.LogError("�������� �ʴ� KeyType�Դϴ�("+_keyType+"). ��ư OnClick�� Ȯ���ϼ���.");
            return;
        }
        _testField.SetValue(data_keySetting, keyInputDetector.currentKeyCode);
        UpdateKeyText(_keyType, _testField.GetValue(data_keySetting).ToString());

    }

    public bool IsUsedKey(KeyCode _inputKey)
    {
        string tempKeyType;


        //�̹� Ű�� �����Ұ��
        if (keyInfoDict.TryGetValue(_inputKey, out tempKeyType))
        {
            Debug.LogError("�ش� Ű�� �̹� " + tempKeyType + "�� �Ҵ�Ǿ��ֽ��ϴ�.");
            return true;
        }
        else //�������� �������
        {
            if (_inputKey == KeyCode.Escape)
            {
                Debug.LogError(_inputKey.ToString() + "Ű�� ����Ͻ� �� �����ϴ�.");
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



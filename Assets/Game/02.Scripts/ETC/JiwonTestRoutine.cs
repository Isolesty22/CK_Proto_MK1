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
    public Button button;
    public Text text;
}
public class JiwonTestRoutine : MonoBehaviour
{
    public KeyInputDetector keyInputDetector;

    public KeyCode changedKey;

    public Data_KeySetting data_keySetting;

    public List<KeyChangeButton> keyButtonList = new List<KeyChangeButton>();

    private Dictionary<string, KeyChangeButton> keyButtonDict = new Dictionary<string, KeyChangeButton>();

    /// <summary>
    /// ex: <A, "moveLeft">
    /// </summary>
    private Dictionary<KeyCode, string> keyInfoDict = new Dictionary<KeyCode, string>();


    public void Init()
    {
        int length = keyButtonList.Count;

        //keyButtonDict 생성
        for (int i = 0; i < length; i++)
        {
            keyButtonDict.Add(keyButtonList[i].keyType, keyButtonList[i]);
        }

        //keyButtonList에 등록된 Button들의 OnClick()에 Button_InputChangeKey() 추가
        for (int i = 0; i < length; i++)
        {
            int index = i;
            keyButtonList[index].button.onClick.AddListener(delegate { Button_InputChangeKey(keyButtonList[index].keyType); });
        }
        //keyInfoDict 생성
        for (int i = 0; i < length; i++)
        {
            FieldInfo _testField = data_keySetting.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);

            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keySetting), _testField.Name);
        }
        //Key Text 업데이트
        UpdateAllKeyText();
    }

    private void Start()
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

        StartChangingKey();
        StartCoroutine(WaitInputKey(_keyType));
    }
    /// <summary>
    /// 키 입력을 기다리고, 키가 입력되었으면 ChangeThisKey()를 호출합니다.
    /// </summary>
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

    }

    public void ChangeThisKey(string _keyType)
    {
        //사용하고 있는 키라면 return
        if (IsUsedKey(keyInputDetector.currentKeyCode))
        {
            EndChangingKey();
            return;
        }

        //_keyType 이름의 변수 받아오기
        FieldInfo _testField = data_keySetting.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);

        //해당 이름의 변수가 없으면 return
        if (ReferenceEquals(_testField, null))
        {
            Debug.LogError("존재하지 않는 KeyType입니다(" + _keyType + "). 버튼 OnClick을 확인하세요.");

            EndChangingKey();
            return;
        }

        //변경 전의 키코드
        KeyCode prevKeyCode = (KeyCode)_testField.GetValue(data_keySetting);
        keyInfoDict.Remove(prevKeyCode);

        //해당 _keyType의 키코드 변경
        _testField.SetValue(data_keySetting, keyInputDetector.currentKeyCode);

        //변경된 현재 키코드
        KeyCode currentKeyCode = (KeyCode)_testField.GetValue(data_keySetting);

        //텍스트 업데이트
        //UpdateAllKeyText();
        UpdateKeyText(_keyType, currentKeyCode.ToString());
        keyInfoDict.Add(currentKeyCode, _keyType);

        EndChangingKey();
    }

    public void StartChangingKey()
    {
        isChangingKey = true;
    }
    public void EndChangingKey()
    {
        isChangingKey = false;
    }
    /// <summary>
    /// 이미 사용하고 있는 키인가?
    /// </summary>
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

    /// <summary>
    /// /UI의 Key Text를 변경합니다.
    /// </summary>
    public void UpdateKeyText(string _keyType, string _changedKey)
    {
        keyButtonDict[_keyType].text.text = _changedKey;
    }

    /// <summary>
    /// /UI의 Key Text를 변경합니다.
    /// </summary>
    public void UpdateAllKeyText()
    {

        for (int i = 0; i < keyButtonList.Count; i++)
        {
            FieldInfo _testField = data_keySetting.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            keyButtonList[i].text.text = _testField.GetValue(data_keySetting).ToString();
        }
    }
}



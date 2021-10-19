using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class UIKeySetting : UIBase
{
    #region UIBase
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true; ;
    }

    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        return true;
    }


    public void CloseMe()
    {
        UIManager.Instance.CloseTop();
    }

    #endregion

    public KeyInputDetector keyInputDetector;

    public KeyCode changedKey;

    public KeyOption data_keyOption;

    public List<KeyChangeButton> keyButtonList = new List<KeyChangeButton>();

    private Dictionary<string, KeyChangeButton> keyButtonDict = new Dictionary<string, KeyChangeButton>();

    /// <summary>
    /// ex: <A, "moveLeft">
    /// </summary>
    private Dictionary<KeyCode, string> keyInfoDict = new Dictionary<KeyCode, string>();

    /// <summary>
    /// 키를 변경하고 있는 중인가?
    /// </summary>
    private bool isChangingKey = false;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Init_Dict();
        CheckOpen();
    }

    private void Init_Dict()
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
            FieldInfo _testField = data_keyOption.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);

            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keyOption), _testField.Name);
        }
        //Key Text 업데이트
        UpdateAllKeyText();

    }


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
        //사용하고 있는 키라면
        if (IsUsedKey(keyInputDetector.currentKeyCode))
        {
            EndChangingKey();

            //다시 키 입력 받기
            StartChangingKey();
            StartCoroutine(WaitInputKey(_keyType));
            return;
        }

        //_keyType 이름의 변수 받아오기
        FieldInfo _testField = data_keyOption.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);

        //해당 이름의 변수가 없으면 return
        if (ReferenceEquals(_testField, null))
        {
            Debug.LogError("존재하지 않는 KeyType입니다(" + _keyType + "). 버튼 OnClick을 확인하세요.");

            EndChangingKey();
            return;
        }

        //변경 전의 키코드
        KeyCode prevKeyCode = (KeyCode)_testField.GetValue(data_keyOption);
        keyInfoDict.Remove(prevKeyCode);

        //해당 _keyType의 키코드 변경
        _testField.SetValue(data_keyOption, keyInputDetector.currentKeyCode);

        //변경된 현재 키코드
        KeyCode currentKeyCode = (KeyCode)_testField.GetValue(data_keyOption);

        //텍스트 업데이트
        //UpdateAllKeyText();
        string tempStr = TryReturnArrowKeyString(currentKeyCode);

        UpdateKeyText(_keyType, tempStr);
        keyInfoDict.Add(currentKeyCode, tempStr);
        EndChangingKey();
    }


    private string TryReturnArrowKeyString(KeyCode _keyCode)
    {
        switch (_keyCode)
        {
            case KeyCode.RightArrow:
                return "→";

            case KeyCode.UpArrow:
                return "↑";

            case KeyCode.DownArrow:
                return "↓";

            case KeyCode.LeftArrow:
                return "←";

            default:
                return _keyCode.ToString();
        }
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
            FieldInfo _testField = data_keyOption.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            keyButtonList[i].text.text = _testField.GetValue(data_keyOption).ToString();
        }
    }

}

[System.Serializable]
public class KeyChangeButton
{
    public string keyType;
    public Button button;
    public Text text;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class UIKeySetting : UIBase
{

    //해야함 : 초기화 후 오류 발생함. 딕셔너리 등등 정리를 좀 해야할듯.....
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
        if (!CanSave())
        {
            Debug.Log("Cant Save!");
            return false;
        }
        StartCoroutine(ProcessClose());
        return true;
    }


    public void CloseMe()
    {
        UIManager.Instance.CloseTop();
    }

    #endregion

    public Image failedImage;
    public Sprite failedBoxSprite;
    public Sprite basicBoxSprite;

    public KeyInputDetector keyInputDetector;

    [HideInInspector]
    public KeyCode changedKey;

    [HideInInspector]
    public KeyOption data_keyOption;
    //public KeyOption data_keyOption_current;
    public KeyOption data_keyOption_saved;

    public List<KeyChangeButton> keyButtonList = new List<KeyChangeButton>();

    private Dictionary<string, KeyChangeButton> keyButtonDict = new Dictionary<string, KeyChangeButton>();

    private Dictionary<string, FieldInfo> fieldInfoDict = new Dictionary<string, FieldInfo>();
    /// <summary>
    /// ex: <A, "moveLeft">
    /// </summary>
    private Dictionary<KeyCode, string> keyInfoDict = new Dictionary<KeyCode, string>();

    /// <summary>
    /// 키를 변경하고 있는 중인가?
    /// </summary>
    private bool isChangingKey = false;


    private void Awake()
    {
        Init_KeyButtonListAndDict();
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        CheckOpen();

        if (DataManager.Instance == null)
        {
            data_keyOption = new KeyOption();
            data_keyOption_saved = new KeyOption();
        }
        else
        {
            data_keyOption = new KeyOption(DataManager.Instance.currentData_settings.keySetting);
            data_keyOption_saved = new KeyOption(DataManager.Instance.currentData_settings.keySetting);


        }

        int length = keyButtonList.Count;

        //keyInfoDict 생성
        for (int i = 0; i < length; i++)
        {
            FieldInfo _testField = data_keyOption.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);
            fieldInfoDict.Add(_testField.Name, _testField);
            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keyOption), _testField.Name);
        }
        //Key Text 업데이트
        UpdateAllKeyText();
    }

    private void Init_KeyButtonListAndDict()
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
            failedImage.gameObject.SetActive(true);

            KeyChangeButton button = keyButtonDict[_keyType];
            button.button.image.sprite = failedBoxSprite;
            button.isFailed = true;

            //다시 키 입력 받기
            StartCoroutine(WaitInputKey(_keyType));
            return;
        }
        else
        {

            failedImage.gameObject.SetActive(false);

            KeyChangeButton button = keyButtonDict[_keyType];
            button.button.image.sprite = basicBoxSprite;
            button.isFailed = false;

            keyInputDetector.EndDetect();
            EndChangingKey();

        }



        //_keyType 이름의 변수 받아오기
        // FieldInfo _testField = data_keyOption.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);
        FieldInfo _testField = null;
        fieldInfoDict.TryGetValue(_keyType, out _testField);

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
            FieldInfo _testField = fieldInfoDict[keyButtonList[i].keyType];
            keyButtonList[i].text.text = TryReturnArrowKeyString((KeyCode)_testField.GetValue(data_keyOption));

        }
    }
    public void Button_Save()
    {
        if (isSaving || !CanSave())
        {
            return;
        }

        StartCoroutine(ProcessSaveCurrentData());

    }
    private bool isSaving = false;
    private IEnumerator ProcessSaveCurrentData()
    {
        isSaving = true;

        EndChangingKey();
        keyInputDetector.EndDetect();

        //데이터 매니저의 현재 데이터를 변경된 데이터로 설정
        DataManager.Instance.currentData_settings.keySetting.CopyData(data_keyOption);

        //변경된 데이터 저장
        yield return StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.fileName_settings));

        data_keyOption_saved.CopyData(data_keyOption);
        isSaving = false;
    }


    public void Button_SetDefaultData()
    {
        if (isSaving)
        {
            return;
        }


        StartCoroutine(ProcessSetDefaultData());

    }

    private IEnumerator ProcessSetDefaultData()
    {
        isSaving = true;
        EndChangingKey();
        keyInputDetector.EndDetect();

        keyInfoDict.Clear();
        data_keyOption = new KeyOption();

        yield return StartCoroutine(ProcessSaveCurrentData());

        for (int i = 0; i < keyButtonList.Count; i++)
        {
            FieldInfo fieldInfo = fieldInfoDict[keyButtonList[i].keyType];
            keyInfoDict.Add(((KeyCode)fieldInfo.GetValue(data_keyOption)), fieldInfo.Name);
        }
        UpdateAllKeyText();

        UpdateAllFailedState();
        isSaving = false;

    }

    private void UpdateAllFailedState()
    {
        int length = keyButtonList.Count;

        bool activeFailedImage = false;
        for (int i = 0; i < length; i++)
        {

            KeyChangeButton tempButton = keyButtonList[i];

            if (tempButton.isFailed)
            {
                tempButton.image.sprite = failedBoxSprite;
                activeFailedImage = true;
            }
            else
            {
                tempButton.image.sprite = basicBoxSprite;
            }

        }

        if (!activeFailedImage)
        {
            failedImage.gameObject.SetActive(false);
        }
    }
    private bool CanSave()
    {
        int length = keyButtonList.Count;

        for (int i = 0; i < length; i++)
        {
            if (keyButtonList[i].isFailed)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDestroy()
    {
        keyInputDetector.EndDetect();
    }


}

[System.Serializable]
public class KeyChangeButton
{
    public string keyType;
    public Button button;
    public Image image;
    public Text text;

    [HideInInspector]
    public bool isFailed = false;



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class UIKeySetting : UIBase
{

    //�ؾ��� : �ʱ�ȭ �� ���� �߻���. ��ųʸ� ��� ������ �� �ؾ��ҵ�.....
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
    /// Ű�� �����ϰ� �ִ� ���ΰ�?
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

        //keyInfoDict ����
        for (int i = 0; i < length; i++)
        {
            FieldInfo _testField = data_keyOption.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);
            fieldInfoDict.Add(_testField.Name, _testField);
            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keyOption), _testField.Name);
        }
        //Key Text ������Ʈ
        UpdateAllKeyText();
    }

    private void Init_KeyButtonListAndDict()
    {
        int length = keyButtonList.Count;

        //keyButtonDict ����
        for (int i = 0; i < length; i++)
        {
            keyButtonDict.Add(keyButtonList[i].keyType, keyButtonList[i]);
        }

        //keyButtonList�� ��ϵ� Button���� OnClick()�� Button_InputChangeKey() �߰�
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
    /// Ű �Է��� ��ٸ���, Ű�� �ԷµǾ����� ChangeThisKey()�� ȣ���մϴ�.
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
        //����ϰ� �ִ� Ű���
        if (IsUsedKey(keyInputDetector.currentKeyCode))
        {
            failedImage.gameObject.SetActive(true);

            KeyChangeButton button = keyButtonDict[_keyType];
            button.button.image.sprite = failedBoxSprite;
            button.isFailed = true;

            //�ٽ� Ű �Է� �ޱ�
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



        //_keyType �̸��� ���� �޾ƿ���
        // FieldInfo _testField = data_keyOption.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);
        FieldInfo _testField = null;
        fieldInfoDict.TryGetValue(_keyType, out _testField);

        //�ش� �̸��� ������ ������ return
        if (ReferenceEquals(_testField, null))
        {
            Debug.LogError("�������� �ʴ� KeyType�Դϴ�(" + _keyType + "). ��ư OnClick�� Ȯ���ϼ���.");

            EndChangingKey();
            return;
        }



        //���� ���� Ű�ڵ�
        KeyCode prevKeyCode = (KeyCode)_testField.GetValue(data_keyOption);
        keyInfoDict.Remove(prevKeyCode);

        //�ش� _keyType�� Ű�ڵ� ����
        _testField.SetValue(data_keyOption, keyInputDetector.currentKeyCode);

        //����� ���� Ű�ڵ�
        KeyCode currentKeyCode = (KeyCode)_testField.GetValue(data_keyOption);

        //�ؽ�Ʈ ������Ʈ
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
                return "��";

            case KeyCode.UpArrow:
                return "��";

            case KeyCode.DownArrow:
                return "��";

            case KeyCode.LeftArrow:
                return "��";

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
    /// �̹� ����ϰ� �ִ� Ű�ΰ�?
    /// </summary>
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

    /// <summary>
    /// /UI�� Key Text�� �����մϴ�.
    /// </summary>
    public void UpdateKeyText(string _keyType, string _changedKey)
    {
        keyButtonDict[_keyType].text.text = _changedKey;
    }

    /// <summary>
    /// /UI�� Key Text�� �����մϴ�.
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

        //������ �Ŵ����� ���� �����͸� ����� �����ͷ� ����
        DataManager.Instance.currentData_settings.keySetting.CopyData(data_keyOption);

        //����� ������ ����
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

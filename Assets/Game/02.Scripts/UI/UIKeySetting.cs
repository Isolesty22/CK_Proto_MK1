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
    /// Ű�� �����ϰ� �ִ� ���ΰ�?
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
        //keyInfoDict ����
        for (int i = 0; i < length; i++)
        {
            FieldInfo _testField = data_keyOption.GetType().GetField(keyButtonList[i].keyType, BindingFlags.Public | BindingFlags.Instance);
            Debug.Log("TestField String : " + _testField.Name);

            keyInfoDict.Add((KeyCode)_testField.GetValue(data_keyOption), _testField.Name);
        }
        //Key Text ������Ʈ
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
            EndChangingKey();

            //�ٽ� Ű �Է� �ޱ�
            StartChangingKey();
            StartCoroutine(WaitInputKey(_keyType));
            return;
        }

        //_keyType �̸��� ���� �޾ƿ���
        FieldInfo _testField = data_keyOption.GetType().GetField(_keyType, BindingFlags.Public | BindingFlags.Instance);

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

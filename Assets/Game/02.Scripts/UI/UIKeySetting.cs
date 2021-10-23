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


    [Header("입력 감지기")]
    public KeyInputDetector keyInputDetector;

    [Header("키세팅 실패 문구")]
    public GameObject failedImage;

    [Header("기본/실패 스프라이트")]
    public Sprite basicBoxSprite;
    public Sprite failedBoxSprite;



    [Tooltip("실제 저장된 데이터와는 관련없는, 저장되기 전의 현재 데이터입니다.")]
    private KeyOption currentData_keyOption = new KeyOption();

    [Tooltip("키세팅에 사용되는 버튼들입니다.")]
    public KeyChangeButton[] keyChangeButtons;

    [Tooltip("keyType을 key로 갖는 딕셔너리.")]
    private Dictionary<string, KeyChangeButton> keyChangeButtonDict;

    private int length;

    [Tooltip("키 변경을 위해 입력을 받고있는 상태인가?")]
    private bool isChangingKey = false;
    private bool isSaving;
    private IEnumerator waitInputChangeKey;

    private void Start()
    {
        currentData_keyOption.CopyData(DataManager.Instance.currentData_settings.keySetting);

        Init_Dict();
        Init_KeyChangeButtons();

        UpdateAllUI();
    }

    /// <summary>
    /// keyChangeButtonDict에 keyChangeButtons를 추가합니다.
    /// </summary>
    public void Init_Dict()
    {
        keyChangeButtonDict = new Dictionary<string, KeyChangeButton>();

        length = keyChangeButtons.Length;

        //keyChangeButtons를 딕셔너리에 추가
        for (int i = 0; i < length; i++)
        {
            keyChangeButtonDict.Add(keyChangeButtons[i].keyType, keyChangeButtons[i]);
        }
    }

    /// <summary>
    /// keyChangeButtons에 등록된 버튼들을 초기화합니다.
    /// </summary>
    public void Init_KeyChangeButtons()
    {
        for (int i = 0; i < length; i++)
        {
            KeyChangeButton tempButton = keyChangeButtons[i];

            tempButton.Init();
            tempButton.field = currentData_keyOption.GetType().GetField(tempButton.keyType, BindingFlags.Public | BindingFlags.Instance);
            tempButton.keyCode = (KeyCode)tempButton.field.GetValue(currentData_keyOption);
            tempButton.button.onClick.AddListener(delegate { Button_InputChangeKey(tempButton.keyType); });
        }
    }


    /// <summary>
    /// 키 입력을 기다리고, 입력되었을 경우 키 변경을 시도합니다.
    /// </summary>
    private IEnumerator WaitInputChangeKey(string _keyType)
    {
        isChangingKey = true;

        //감지 시작
        keyInputDetector.StartDetect();

        yield return null;

        //감지가 끝날 때 까지 대기
        yield return new WaitWhile(() => keyInputDetector.isDetecting);

        //감지 되었으면 감지 끝내기
        keyInputDetector.EndDetect();

        //감지 되었으면 키 변경 시도
        TryChangeThisKey(_keyType);
    }

    private void InputChangeKey(string _keyType)
    {
        //이미 다른 키를 변경 중이었을 경우에는, 해당 키를 변경할 수 있도록 Stop후 재시작
        if (isChangingKey)
        {
            Debug.Log("Stop!");
            StopCoroutine(waitInputChangeKey);
            keyInputDetector.EndDetect();
            isChangingKey = false;
        }

        waitInputChangeKey = WaitInputChangeKey(_keyType);

        Debug.Log("KeyChange Button! : " + _keyType);

        StartCoroutine(waitInputChangeKey);
    }

    /// <summary>
    /// 해딩 키 변경을 시도하고, 실패했을 경우 다시 키 입력을 받습니다.
    /// </summary>
    private void TryChangeThisKey(string _keyType)
    {
        KeyChangeButton currentButton = keyChangeButtonDict[_keyType];
        KeyCode currentKeyCode = keyInputDetector.currentKeyCode;

        ////같은 키로 변경을 시도했을 경우
        //if (currentButton.keyCode == currentKeyCode)
        //{
        //    //그냥 아무것도 하지 않고 종료
        //    isChangingKey = false;
        //    return;
        //}

        bool isFailed = false;


        for (int i = 0; i < length; i++)
        {
            //같은 키코드가 있을 경우 실패 판정
            if (keyChangeButtons[i].keyCode == currentKeyCode)
            {
                if (keyChangeButtons[i] == currentButton)
                    continue;

                isFailed = true;
                break;
            }
        }

        //실패 판정이 났을 경우 다시 감지
        if (isFailed || !IsPossibleKey(currentKeyCode))
        {
            Debug.Log("실패 판정입니다.");
            SetFailed(true);

            InputChangeKey(_keyType);

            //실패 이미지로 변경
            currentButton.image.sprite = failedBoxSprite;
            currentButton.text.text = TryConvertString(keyInputDetector.currentKeyCode);
            currentButton.isFailed = true;

            return;
        }
        else // 성공 판정이 났을 경우
        {
            // 원래 실패 상태였을 경우
            if (currentButton.isFailed)
            {
                //기본 이미지로 변경
                currentButton.image.sprite = basicBoxSprite;
                currentButton.isFailed = false;
                SetFailed(false);
            }
        }

        //키 변경을 할 수 있는 상태라면 fieldInfo를 통해 값 변경
        currentButton.field.SetValue(currentData_keyOption, keyInputDetector.currentKeyCode);

        //버튼의 키코드 변경
        currentButton.keyCode = keyInputDetector.currentKeyCode;

        //텍스트 변경
        currentButton.text.text = TryConvertString(keyInputDetector.currentKeyCode);

        isChangingKey = false;
    }

    private void SetFailed(bool _active) => failedImage.gameObject.SetActive(_active);

    private void UpdateAllUI()
    {
        UpdateUI_Text();
        UpdateUI_Box();
    }


    private void UpdateUI_Text()
    {
        for (int i = 0; i < length; i++)
        {
            keyChangeButtons[i].text.text = TryConvertString(keyChangeButtons[i].keyCode);
        }
    }

    /// <summary>
    /// Box UI를 업데이트합니다.
    /// </summary>
    private void UpdateUI_Box()
    {

        for (int i = 0; i < length; i++)
        {
            if (keyChangeButtons[i].isFailed)
            {
                keyChangeButtons[i].image.sprite = failedBoxSprite;
            }
            else
            {
                keyChangeButtons[i].image.sprite = basicBoxSprite;
            }
        }
    }

    /// <summary>
    /// isFailed인 키가 있을 경우 저장할 수 없습니다.
    /// </summary>
    /// <returns></returns>
    private bool CanSave()
    {

        for (int i = 0; i < length; i++)
        {
            if (keyChangeButtons[i].isFailed)
            {
                return false;
            }
        }
        return true;
    }



    //해야함 : LINQ로 변경할 수 있을 것 같으니 실험해보기
    /// <summary>
    /// 키 설정이 가능한 키인지 체크합니다.
    /// </summary>
    private bool IsPossibleKey(KeyCode _keyCode)
    {
        switch (_keyCode)
        {
            case KeyCode.Backspace:
                return false;
            case KeyCode.Return:
                return false;
            case KeyCode.Escape:
                return false;
            case KeyCode.Semicolon:
                return false;
            case KeyCode.BackQuote:
                return false;
            default:
                return true;
        }
    }

    /// <summary>
    /// KeyCode를 String으로 변환합니다. 특정 KeyCode의 경우에는 지정된 문자로 변환횝니다.
    /// </summary>

    private string TryConvertString(KeyCode _keyCode)
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

    /// <summary>
    /// 키 변경 버튼 함수
    /// </summary>
    private void Button_InputChangeKey(string _keyType)
    {
        InputChangeKey(_keyType);

    }

    /// <summary>
    /// 초기화 버튼 함수
    /// </summary>
    public void Button_SetDefault()
    {
        currentData_keyOption = new KeyOption();
        Init_KeyChangeButtons();
        SetFailed(false);
        UpdateAllUI();
    }


    /// <summary>
    /// 적용 버튼 함수
    /// </summary>
    public void Button_Save()
    {
        if (isSaving || !CanSave())
        {
            return;
        }

        StartCoroutine(ProcessSave());
    }

    private IEnumerator ProcessSave()
    {
        isSaving = true;

        DataManager.Instance.currentData_settings.keySetting.CopyData(currentData_keyOption);

        yield return StartCoroutine(DataManager.Instance.SaveCurrentData(DataManager.fileName_settings));
        Debug.Log("Save 완료");
        isSaving = false;
    }

}



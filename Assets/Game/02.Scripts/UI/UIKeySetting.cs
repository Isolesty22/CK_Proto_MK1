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
        UpdateAllUI();
        keyInputDetector.Init();
        StartCoroutine(ProcessOpen());
        return true; ;
    }

    public override bool Close()
    {
        //변경사항이 있다면
        if (!(currentData_keyOption.IsEquals(DataManager.Instance.currentData_settings.keySetting)))
        {
            //팝업 띄우기
            UIManager.Instance.OpenPopup(eUIText.DataNotSave,
                Button_Changes_Yes, Button_Changes_No);
            return false;
        }
        else //없다면
        {
            //그냥 닫음
            StartCoroutine(ProcessClose());
            isChangingKey = false;
            keyInputDetector.Init();
            return true;
        }
    }
    /// <summary>
    /// 이대로 종료하시겠습니까? 네 의 버튼
    /// </summary>
    private void Button_Changes_Yes()
    {
        UIManager.Instance.PlayAudio_Click();
        isChangingKey = false;

        //현재 데이터를 저장된 데이터로 변경(폐기)
        currentData_keyOption.CopyData(DataManager.Instance.currentData_settings.keySetting);

        //버튼 초기화
        Init_KeyChangeButtons();
        SetFailed(false);
        //UI 업데이트
        UpdateAllUI();

        //창을 아예 닫기
        UIManager.Instance.CloseTop();
        UIManager.Instance.CloseTop();
    }
    private void Button_Changes_No()
    {
        UIManager.Instance.PlayAudio_Click();
        isChangingKey = false;
        UIManager.Instance.CloseTop();
    }

    public void CloseMe()
    {
        UIManager.Instance.CloseTop();
    }

    #endregion


    [Header("입력 감지기")]
    public KeyInputDetector keyInputDetector;

    public SelectorController selectorController;

    [Header("키세팅 실패 문구")]
    public GameObject failedImage;
    public Text uiText;

    [Header("기본/실패 스프라이트")]
    public Sprite basicBoxSprite;
    public Sprite failedBoxSprite;



    [Tooltip("실제 저장된 데이터와는 관련없는, 저장되기 전의 현재 데이터입니다.")]
    private KeyOption currentData_keyOption = new KeyOption();

    [Tooltip("키세팅에 사용되는 버튼들입니다.")]
    public KeyChangeButton[] keyChangeButtons;

    [Tooltip("keyType을 key로 갖는 딕셔너리.")]
    private Dictionary<string, KeyChangeButton> keyChangeButtonDict;
    //private Dictionary <string, Selec>

    private int length;

    [Tooltip("키 변경을 위해 입력을 받고있는 상태인가?")]
    private bool isChangingKey = false;
    private bool isSaving;
    private IEnumerator waitInputKey;






    private void Start()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogError("데이터매니저가 존재하지 않아서 키세팅 데이터를 가져올 수 없었습니다.");
        }
        else
        {
            currentData_keyOption = new KeyOption();
            currentData_keyOption.CopyData(DataManager.Instance.currentData_settings.keySetting);
        }

        uiText.text = "";
        Init_Dict();
        Init_KeyChangeButtons();
        selectorController.SelectButton(selectorController.selectIndexOnStart);
        UpdateAllUI();

        UIManager.Instance.AddDict(this);
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
            tempButton.button.onClick.AddListener(() => Button_InputChangeKey(tempButton.keyType));
        }
    }


    /// <summary>
    /// 키 입력을 기다리고, 입력되었을 경우 키 변경을 시도합니다.
    /// </summary>
    private IEnumerator WaitInputKey(string _keyType)
    {
        isChangingKey = true;

        //감지 시작
        keyInputDetector.StartDetect();

        yield return null;

        //감지가 끝날 때 까지 대기
        yield return new WaitWhile(() => keyInputDetector.isDetecting);

        //감지 되었으면 감지 끝내기
        keyInputDetector.EndDetect();

        //if (keyInputDetector.currentKeyCode == KeyCode.Escape)
        //{
        //    isChangingKey = false;
        //    uiText.text = "";
        //    yield break;
        //}

        //다시 UI매니저에서 ESC키를 감지하게
        UIManager.Instance.StartDetectingCloseKey();

        //셀렉터 포지션을 업데이트하게
        selectorController.DoUpdateSelectorPosition();
        //감지 되었으면 키 변경 시도
        TryChangeThisKey(_keyType);
    }

    private void InputChangeKey(string _keyType)
    {
        UIManager.Instance.StopDetectingCloseKey();

        //셀렉터 포지션을 업데이트하게
        selectorController.DoNotUpdateSelectorPosition();

        //이미 다른 키를 변경 중이었을 경우에는, 해당 키를 변경할 수 있도록 Stop후 재시작
        if (isChangingKey)
        {
            Debug.Log("Stop!");
            StopCoroutine(waitInputKey);
            keyInputDetector.EndDetect();
            isChangingKey = false;

            if (keyInputDetector.currentKeyCode == KeyCode.Escape)
            {
                UIManager.Instance.StartDetectingCloseKey();
                selectorController.DoUpdateSelectorPosition();

                return;
            }
        }
        else
        {
            uiText.text = "키 입력 대기 중...";
        }
        waitInputKey = WaitInputKey(_keyType);

        Debug.Log("KeyChange Button! : " + _keyType);


        StartCoroutine(waitInputKey);
    }

    /// <summary>
    /// 해딩 키 변경을 시도하고, 실패했을 경우 다시 키 입력을 받습니다.
    /// </summary>
    private void TryChangeThisKey(string _keyType)
    {
        KeyChangeButton currentButton = keyChangeButtonDict[_keyType];
        KeyCode currentKeyCode = keyInputDetector.currentKeyCode;

        bool isFailed = false;
        bool isImpossibled = false;

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


        isImpossibled = !IsPossibleKey(currentKeyCode);

        if (isFailed)
        {
            uiText.text = "이미 사용 중인 키입니다.";
        }
        else if (isImpossibled)
        {
            uiText.text = "해당 키는 사용할 수 없습니다.";
        }
        else
        {
            uiText.text = "";
        }

        //실패 판정이 났을 경우 다시 감지
        if (isFailed || isImpossibled)
        {
            //실패 상태로 변경
            currentButton.image.sprite = failedBoxSprite;
            currentButton.text.text = "";// TryConvertString(keyInputDetector.currentKeyCode);
            currentButton.isFailed = true;

            keyInputDetector.EndDetect();

            //다시 키 입력 받기
            InputChangeKey(_keyType);
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

    private void SetFailed(bool _active)
    {

        if (_active)
        {
            uiText.text = "키 등록에 실패했습니다.";
        }
        else
        {
            uiText.text = "";
        }
    }


    /// <summary>
    /// 모든 UpdateUI를 호출합니다.
    /// </summary>
    private void UpdateAllUI()
    {
        UpdateUI_Text();
        UpdateUI_Box();
    }


    /// <summary>
    /// 키 Text UI를 업데이트합니다.
    /// </summary>
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

    /// <summary>
    /// 키 설정이 가능한 키인지 체크합니다...
    /// </summary>
    private bool IsPossibleKey(KeyCode _keyCode)
    {

        //아니...이게...이게 맞나? 진짜 이렇게 해아하는건가?
        switch (_keyCode)
        {
            case KeyCode.Q:
            case KeyCode.W:
            case KeyCode.E:
            case KeyCode.R:
            case KeyCode.T:
            case KeyCode.Y:
            case KeyCode.U:
            case KeyCode.I:
            case KeyCode.O:
            case KeyCode.P:
            case KeyCode.A:
            case KeyCode.S:
            case KeyCode.D:
            case KeyCode.F:
            case KeyCode.G:
            case KeyCode.H:
            case KeyCode.J:
            case KeyCode.K:
            case KeyCode.L:
            case KeyCode.Z:
            case KeyCode.X:
            case KeyCode.C:
            case KeyCode.V:
            case KeyCode.B:
            case KeyCode.N:
            case KeyCode.LeftArrow:
            case KeyCode.RightArrow:
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
                return true;

            default:
                return false;
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
        UIManager.Instance.PlayAudio_Click();
        InputChangeKey(_keyType);
    }

    /// <summary>
    /// 초기화 버튼 함수
    /// </summary>
    public void Button_SetDefault()
    {
        UIManager.Instance.PlayAudio_Click();
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
        UIManager.Instance.PlayAudio_Click();
        if (isSaving || !CanSave())
        {
            Debug.Log("키 변경 중에는 저장할 수 없습니다.");
            return;
        }
        isSaving = true;
        StartCoroutine(ProcessSave());
    }

    private IEnumerator ProcessSave()
    {
        isSaving = true;
        DataManager.Instance.currentData_settings.keySetting.CopyData(currentData_keyOption);

        yield return StartCoroutine(DataManager.Instance.SaveCurrentData(DataName.settings));
        Debug.Log("Save 완료");

        isSaving = false;
        UIManager.Instance.CloseTop();
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPopup_New : UIBase
{
    public Dictionary<eUIText, Sprite> textImageDict = new Dictionary<eUIText, Sprite>();

    public Image textImage = null;
    private Sprite currentSprite;
    [Header("팝업 관련")]
    public Button button_left;
    public Button button_right;
    public UIPopupTextImages[] textImageArray;

    /// <summary>
    /// 왼쪽 버튼에 해당되는 키를 눌렀을 때, 이벤트를 실행해줍니다. 
    /// </summary>
    private IEnumerator coDetectEnterKey = null;

    /// <summary>
    /// 팝업창의 내용, 버튼 이벤트를 초기화합니다.
    /// </summary>
    /// <param name="_text">내용(\n등은 제대로 적용됨)</param>
    /// <param name="_yes">왼쪽 버튼에 적용되는 이벤트</param>
    /// <param name="_no">오른쪽 버튼에 적용되는 이벤트</param>
    public void Init_Popup(eUIText _uiText, UnityAction _left, UnityAction _right)
    {
        Com.canvasGroup.interactable = true;

        if (textImageDict.TryGetValue(_uiText, out currentSprite))
        {
            textImage.sprite = currentSprite;
            textImage.SetNativeSize();
        }
        else
        {
            textImage.sprite = null;
        }

        button_left.onClick.AddListener(() => Init_AddListener());
        button_right.onClick.AddListener(() => Init_AddListener());

        button_left.onClick.AddListener(_left);
        button_right.onClick.AddListener(_right);
    }


    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 버튼에 애드 리스너를 할 때 기본적으로 넣어줘야합니다. 
    /// </summary>
    private void Init_AddListener()
    {
        UIManager.Instance.PlayAudio_Click();
        Com.canvasGroup.interactable = false;
    }
    public override void Init()
    {
        base.Init();

        int length = textImageArray.Length;
        for (int i = 0; i < length; i++)
        {
            textImageDict.Add(textImageArray[i].uiText, textImageArray[i].textSprite);
        }
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen());

        if (coDetectEnterKey != null)
        {
            StopCoroutine(coDetectEnterKey);
        }
        coDetectEnterKey = CoDetectEnterKey();
        StartCoroutine(coDetectEnterKey);
        return true;

        //Com.canvas.enabled = true;
        //return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {
        //RemoveListeners
        button_right.onClick.RemoveAllListeners();
        button_left.onClick.RemoveAllListeners();
        SetEventSystemNull();
        StartCoroutine(ProcessClose());

        if (coDetectEnterKey != null)
        {
            StopCoroutine(coDetectEnterKey);
            coDetectEnterKey = null;
        }
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }


    private IEnumerator CoDetectEnterKey()
    {
        yield return null;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                if (Com.canvasGroup.alpha >= 1f && Com.canvasGroup.interactable)
                {
                    Debug.Log("Invoke!");
                    button_left.onClick.Invoke();
                }
            }
            yield return null;
        }
    }

    public void CloseMe() => UIManager.Instance.CloseTop();
}

[System.Serializable]
public class UIPopupTextImages
{
    public eUIText uiText;
    public Sprite textSprite;

}

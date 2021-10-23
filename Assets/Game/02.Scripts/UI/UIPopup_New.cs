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
    /// 팝업창의 내용, 버튼 이벤트를 초기화합니다.
    /// </summary>
    /// <param name="_text">내용(\n등은 제대로 적용됨)</param>
    /// <param name="_yes">왼쪽 버튼에 적용되는 이벤트</param>
    /// <param name="_no">오른쪽 버튼에 적용되는 이벤트</param>
    public void Init_Popup(eUIText _uiText, UnityAction _left, UnityAction _right)
    {
        if (textImageDict.TryGetValue(_uiText, out currentSprite))
        {
            textImage.sprite = currentSprite;
            textImage.SetNativeSize();
        }
        else
        {
            textImage.sprite = null;
        }
        button_left.onClick.AddListener(_left);
        button_right.onClick.AddListener(_right);
    }
    private void Start()
    {
        Init();
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
        return true;

        //Com.canvas.enabled = true;
        //return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {
        //RemoveListeners
        button_right.onClick.RemoveAllListeners();
        button_left.onClick.RemoveAllListeners();

        StartCoroutine(ProcessClose());
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }

    public void CloseMe() => UIManager.Instance.CloseTop();

}

[System.Serializable]
public class UIPopupTextImages
{
    public eUIText uiText;
    public Sprite textSprite;
    
}

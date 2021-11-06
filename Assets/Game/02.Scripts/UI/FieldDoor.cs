using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldDoor : MonoBehaviour
{

    public string stageName;
    public RectTransform rectTransform;
    [Space(5)]
    public Image doorImage_Default;
    public Image doorImage_Open;

    public enum eMode
    {
        Default,
        Selected,
        Open,
    }
    [Space(5)]
    public eMode mode;
    public void Init()
    {
        switch (mode)
        {
            case eMode.Default:
                break;

            case eMode.Selected:
                break;

            case eMode.Open:
                break;
            default:
                break;
        }
    }
    public void Open()
    {
        StartCoroutine(ProcessOpen());
    }
    public void Button_EnterStage()
    {
        //해야함 : 스테이지 입장하기.
    }

    private IEnumerator ProcessOpen()
    {
        yield break;
    }

    /// <summary>
    /// 렉트트랜스폼의 포지션을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return rectTransform.position;
    }
}

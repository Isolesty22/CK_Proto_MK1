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

    [Space(5)]
    public Image lockImage;
    public Sprite lockSprite_Default;
    public Sprite lockSprite_Open;
    public Image blackPanel;

    [Space(5)]
    [Tooltip("아마도 스테이지를 해금할 때 알파값을 조정하는 그룹입니다.")]
    public CanvasGroup canvasGroup;
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
    private void Start()
    {
        Open();
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
        yield return new WaitForSeconds(0.5f);
        lockImage.sprite = lockSprite_Open;
        yield return new WaitForSeconds(0.4f);

        float timer = 0f;
        float progress = 0f;
        float fadeTime = 1f;

        Color startColor = new Color(1f, 1f, 1f, 0.8f);
        Color endColor = new Color(0f, 0f, 0f, 0f);
        while (progress < 1f)
        {
            timer += Time.deltaTime;

            progress = timer / fadeTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// 렉트트랜스폼의 포지션을 반환합니다.
    /// </summary>
    public Vector3 GetPosition()
    {
        return rectTransform.position;
    }
}

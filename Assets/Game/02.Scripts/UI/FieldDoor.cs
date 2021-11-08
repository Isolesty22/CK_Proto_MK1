using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldDoor : MonoBehaviour
{

    [Header("스테이지 정보")]
    [HideInInspector]
    public string stageName;
    public int stageNumber;

    [Space(5)]
    public FieldMapManager fieldMapManager;
    public RectTransform rectTransform;

    [Tooltip("셀렉터가 참조할 트랜스폼.")]
    public RectTransform selectTransform;
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
        // Default,
        Lock,
        //Selected,
        Open,
    }
    [Space(5)]
    public eMode mode = eMode.Lock;

    public void Init()
    {
        //이름 설정
        stageName = SceneNames.GetSceneNameUseStageNumber(stageNumber);
        if (mode == eMode.Open)
        {
            AlreadyOpen();
        }
        //Open();
    }

    /// <summary>
    /// 문을 엽니다.
    /// </summary>
    public void Open()
    {
        StartCoroutine(ProcessOpen());
    }
    public void Button_EnterStage()
    {
        DataManager.Instance.currentData_player.currentStageNumber = stageNumber;
        DataManager.Instance.currentData_player.currentStageName = stageName;

        //저장은 딱히 안해도 될것같지만 일단 해보기
        DataManager.Instance.SaveCurrentData(DataManager.fileName_player);

        SceneChanger.Instance.LoadThisScene(stageName);
    }

    //public void Button_SelectDoor()
    //{
    //    fieldMapManager.SelectDoor(stageNumber);

    //}


    /// <summary>
    /// 문을 즉시 열려있는 상태로 전환합니다. 
    /// </summary>
    private void AlreadyOpen()
    {
        lockImage.sprite = lockSprite_Open;
        canvasGroup.alpha = 0f;
        mode = eMode.Open;
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

        mode = eMode.Open;
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

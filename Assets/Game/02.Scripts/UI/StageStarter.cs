using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 시작 연출등
/// </summary>
public class StageStarter : MonoBehaviour
{
    [Header("타임라인 사용 여부")]
    public bool useTimeline;

    [Tooltip("true일 때, 타임라인이 끝나면 자동으로 플레이어 UI가 열리지 않습니다(플레이어의 조작 불가도 그대로입니다).")]
    [Header("자동으로 플레이어 UI를 열지 않기")]
    public bool doNotOpenPlayerUI;

    [Header("Null Check")]
    public bool isDebug;

    [Tooltip("스테이지 시작 후 출력되는 시작 UI가 출력되어있는 시간")]
    public float waitTime;

    public Transform playerMoveEndPosition;

    [Header("Stage Start UI")]
    public UIStageStart uiStageStart;
    private void Start()
    {
        if (useTimeline)
        {
            GameManager.instance.playerController.State.moveSystem = true;
            GameManager.instance.timelineManager.onTimelineEnded += OnTimelineEnded;
        }
        else
        {
            GameManager.instance.timelineManager.onTimelineEnded += OnTimelineEnded;
        }


        //if (SceneChanger.Instance != null)
        //{
        //    yield return new WaitWhile(() => SceneChanger.Instance.isLoading);
        //    StartCoroutine(OpenUI());
        //}
      //  yield break;
    }

    public void OnTimelineEnded()
    {
        GameManager.instance.timelineManager.onTimelineEnded -= OnTimelineEnded;

        if (useTimeline)
        {
            if (!doNotOpenPlayerUI)
            {
                GameManager.instance.playerController.State.moveSystem = false;
                UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
                ui.Open();
            }
                return;
        }
        else
        {
            StartCoroutine(CoOpenPlayerUI());
        }
        //StartCoroutine(OpenUI());
    }

    private IEnumerator CoOpenPlayerUI()
    {
        GameManager.instance.playerController.State.moveSystem = true;
        GameManager.instance.playerController.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(2f);
        GameManager.instance.playerController.State.moveSystem = false;
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Open();
    }
    private IEnumerator OpenUI()
    {

        yield return new WaitForSeconds(0.5f);
        //StageStart 열기.
        //UIManager랑 상관없어야하므로 그냥 Open 호출
        uiStageStart.Open();

        //Fade할 시간 정하기
        uiStageStart.SetFadeDuration(0.5f);

        //대기
        //일시정지에 영향을 받으면 좋을것같아서
        float totalWaitTime = uiStageStart.GetFadeDuration() + waitTime;
        yield return new WaitForSeconds(totalWaitTime);

        uiStageStart.Close();
    }

    //    private IEnumerator WaitSceneLoading()
    //{
    //    if (isDebug)
    //    {
    //        Debug.LogWarning("StageStarter : Debug Mode!! 빌드 시에는 isDebug를 꺼주세요.");
    //        if (SceneChanger.Instance != null)
    //        {
    //            while (SceneChanger.Instance.isLoading)
    //            {
    //                yield return null;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        while (SceneChanger.Instance.isLoading)
    //        {
    //            yield return null;
    //        }
    //    }
    //    //로딩이 끝날 때 까지 대기


    //}
}

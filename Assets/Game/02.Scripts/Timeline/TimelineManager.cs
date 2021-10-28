using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director;

    public Action OnTimelineEnded;

    private IEnumerator Start()
    {
        //로딩이 끝날 때 까지 대기
        yield return new WaitUntil(() => !SceneChanger.Instance.isLoading);

        //로딩이 끝나면 타임라인 재생 시작
        Play();

    }


    /// <summary>
    /// 타임라인 재생이 끝날 때 까지 기다립니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitTimelineEnd()
    {
        yield return new WaitUntil(() => director.state != PlayState.Playing);

        Debug.LogWarning("[TimelineManager] End Timeline : " + director.name + "." + director.playableAsset.name);

        //OnTileLineEnded 호출
        OnTimelineEnded.Invoke();
    }

    public void SetTimeline(TimelineAsset _timeline)
    {
        director.playableAsset = _timeline;
    }
    public void Play()
    {
        director.Play();
        StartCoroutine(WaitTimelineEnd());
    }
}

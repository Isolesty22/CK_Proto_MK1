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
    private void Awake()
    {

    }

    private void Start()
    {
    }

    /// <summary>
    /// 타임라인 재생이 끝날 때 까지 기다립니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitTimelineEnd()
    {
        yield return new WaitUntil(() => director.state != PlayState.Playing);
        Debug.LogWarning("[TimelineManager] End Timeline : " + director.name + "." + director.playableAsset.name);
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

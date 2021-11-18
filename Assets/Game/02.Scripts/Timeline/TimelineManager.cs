using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

public class TimelineManager : MonoBehaviour
{

    [Tooltip("true일 경우, 로딩이 끝나면 지정된 타임라인을 플레이합니다.")]
    public bool playOnLoadingEnded;
    public PlayableDirector director;

    public Action onTimelineEnded = null;

    private void Awake()
    {
        //로딩 후 영상 재생에 체크가 안되어있을 경우
        if (!playOnLoadingEnded)
        {
            if (director != null)
            {
                //디렉터가 달린 게임 오브젝트를 비활성화
                director.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        if (SceneChanger.Instance == null)
        {
            Debug.LogWarning("SceneChanger가 null입니다. 로딩 대기 없이 타임라인을 그냥 Play합니다.");
            if (director != null)
            {
                director.Play();
                StartCoroutine(WaitTimelineEnd());
            }
            else
            {
                StartCoroutine(OnTimelineEnd());
            }
            return;
        }
        if (playOnLoadingEnded)
        {
            StartCoroutine(ProcessPlayTimeline());
        }
        else
        {
            StartCoroutine(OnTimelineEnd());
        }
    }

    private IEnumerator ProcessPlayTimeline()
    {
        //로딩이 끝날 때 까지 대기
        yield return new WaitUntil(() => !SceneChanger.Instance.isSceneLoading);

        //로딩이 끝나면 타임라인 재생 시작
        Play();

    }
    /// <summary>
    /// 타임라인 재생이 끝날 때 까지 기다립니다.
    /// </summary>
    private IEnumerator WaitTimelineEnd()
    {
        yield return new WaitUntil(() => director.state != PlayState.Playing);

        Debug.LogWarning("[TimelineManager] End Timeline : " + director.name + "." + director.playableAsset.name);

        //타임라인이 달린 오브젝트 비활성화
        director.gameObject.SetActive(false);

        //혹시 몰라서 한프레임 대기 후에 OnTileLineEnded 호출
        yield return null;
        onTimelineEnded?.Invoke();

    }

    private IEnumerator OnTimelineEnd()
    {
        yield return null;
        onTimelineEnded?.Invoke();
    }

    public void SetTimeline(TimelineAsset _timeline)
    {
        director.playableAsset = _timeline;
    }
    public void Play()
    {
        if (director != null)
        {
            director.Play();
            StartCoroutine(WaitTimelineEnd());
        }
    }
}

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
        //�ε��� ���� �� ���� ���
        yield return new WaitUntil(() => !SceneChanger.Instance.isLoading);

        //�ε��� ������ Ÿ�Ӷ��� ��� ����
        Play();

    }


    /// <summary>
    /// Ÿ�Ӷ��� ����� ���� �� ���� ��ٸ��ϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitTimelineEnd()
    {
        yield return new WaitUntil(() => director.state != PlayState.Playing);

        Debug.LogWarning("[TimelineManager] End Timeline : " + director.name + "." + director.playableAsset.name);

        //OnTileLineEnded ȣ��
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

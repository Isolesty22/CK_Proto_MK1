using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UIMovieScreen : UIBase
{

    public VideoPlayer videoPlayer;

    public IEnumerator openAndPlayCoroutine;
    private void Awake()
    {
        openAndPlayCoroutine = ProcessOpen();
    }
    public override void Init()
    {
        CheckOpen();
    }
    public override bool Open()
    {
        isOpen = true;
        fadeDuration = 0.3f;
        StartCoroutine(ProcessOpen());
        return true;
    }
    public override bool Close()
    {
        fadeDuration = 0.5f;
        StartCoroutine(ProcessClose());
        return true;
    }
    public void Play()
    {
        videoPlayer.Play();
        StartCoroutine(EndPlaying());
    }
    public void OnPressSkip()
    {
        videoPlayer.Stop();
    }

    private IEnumerator EndPlaying()
    {
        yield return null;
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        Debug.Log("end");
    }
    protected override IEnumerator ProcessOpen()
    {
        yield return base.ProcessOpen();
        Play();
    }

    protected override IEnumerator ProcessClose()
    {
        return base.ProcessClose();
    }
}

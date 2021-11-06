using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UIMovieScreen : UIBase
{
    public VideoPlayer videoPlayer;

    public IEnumerator playingCoroutine;

    public Action OnMovieEnded = null;

    private bool isSkip = false;
    private void Awake()
    {
        playingCoroutine = Playing();
        isSkip = false;
        //OnMovieEnded +=  delegate{ Close(); };
    }
    //private void Start()
    //{
    //    StartCoroutine(playingCoroutine);
    //}
    public override void Init()
    {
        CheckOpen();
    }
    public override bool Open()
    {
        isOpen = true;
        fadeDuration = 0.5f;
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

        StartCoroutine(Playing());
    }
    public void OnPressSkip()
    {
        isSkip = true;
    }

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    private IEnumerator Playing()
    {
        videoPlayer.Play();
        StartCoroutine(ProcessOpen());
        long frameCount = Convert.ToInt64(videoPlayer.frameCount) - 1;

       UIManager.Instance?.StopDetectingCloseKey();

        while (true)
        {

            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Z) || isSkip)
            {
                break;
            }
            if (videoPlayer.frame == frameCount)
            {
                break;
            }

            yield return null;
        }
        videoPlayer.Stop();
        OnMovieEnded?.Invoke();
    }
}

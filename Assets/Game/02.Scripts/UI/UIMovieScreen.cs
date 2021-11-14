using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class UIMovieScreen : UIBase
{

    public Image blackPanel;
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

    Color startColor = new Color(0, 0, 0, 0);
    Color endColor = new Color(0, 0, 0, 1);

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    private IEnumerator Playing()
    {
        yield return StartCoroutine(ProcessOpen());
        blackPanel.gameObject.SetActive(false);
        videoPlayer.Play();
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

        float timer = 0f;
        float progress = 0f;


        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / 1f;

            blackPanel.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }
        blackPanel.gameObject.SetActive(true);

        videoPlayer.Stop();
        OnMovieEnded?.Invoke();
    }
}

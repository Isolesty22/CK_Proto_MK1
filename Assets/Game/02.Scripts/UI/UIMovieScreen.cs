using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class UIMovieScreen : UIBase
{

    [Tooltip("영상이 재생될 렌더 텍스처입니다.")]
    public RenderTexture renderTexture;
    public VideoPlayer videoPlayer;

    [Space(5)]

    [Tooltip("영상이 끝나면 해당 패널이 페이드 됩니다.")]
    public Image blackPanel;

    [Space(5)]
    public VideoClip[] videoClips;

    private bool isSkip = false;

    [Tooltip("true일 경우, 영상 플레이가 끝날 때 렌더텍스처를 해제하지 않습니다.")]
    private bool doNotRelease;

    //Black Panel Colors
    private Color startColor = new Color(0, 0, 0, 0);
    private Color endColor = new Color(0, 0, 0, 1);

    public IEnumerator playMovie;
    public event Action onMovieEnded = null;

    private void Awake()
    {
        playMovie = CoPlayMovie();
        isSkip = false;
        //OnMovieEnded +=  delegate{ Close(); };
    }
    private void Start()
    {
        Init();
        UIManager.Instance.AddDict(this);
    }
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
        StartCoroutine(CoPlayMovie());
    }


    public void Play(int _index)
    {
        videoPlayer.clip = videoClips[_index];
        StartCoroutine(CoPlayMovie());
    }
    public void OnPressSkip()
    {
        UIManager.Instance.PlayAudio_Click();
        isSkip = true;
    }

    private IEnumerator CoPlayMovie()
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
        onMovieEnded?.Invoke();

        if (!doNotRelease)
        {
            renderTexture.Release();
        }
    }
}

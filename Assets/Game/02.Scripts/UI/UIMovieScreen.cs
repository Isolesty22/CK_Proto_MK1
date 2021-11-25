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

    public GameObject waitingPanel;

    [Space(5)]

    [Tooltip("영상이 끝나면 해당 패널이 페이드 됩니다.")]
    public Image blackPanel;
    [Space(5)]
    [Tooltip("플레이 스피드를 조절하는 버튼")]
    public Button playbackSpeedButton;

    [Serializable]
    public class PlaybackSpriteSet
    {
        public Sprite defaultSprite;
        public SpriteState sprites;
    }
    public PlaybackSpriteSet x1SpriteSet;
    public PlaybackSpriteSet x2SpriteSet;

    [Space(5)]
    public VideoClip[] videoClips;


    //Black Panel Colors
    private Color startColor = new Color(0, 0, 0, 0);
    private Color endColor = new Color(0, 0, 0, 1);

    public IEnumerator playMovie;
    public event Action onMovieEnded = null;

    private bool isSkip = false;

    [Tooltip("true일 경우, 영상 플레이가 끝날 때 렌더텍스처를 해제하지 않습니다.")]
    private bool doNotRelease;

    private bool isPlaybackSpeedUp;

    private void Awake()
    {
        playMovie = CoPlayMovie();
        isSkip = false;
        isPlaybackSpeedUp = false;
        videoPlayer.playbackSpeed = 1f;
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
    /// <summary>
    /// 아무것도 안하고 재생만 합니다.
    /// </summary>
    public void Play()
    {
        StartCoroutine(CoPlayMovie());
    }

    public void Button_ToggleSpeed()
    {
        //스피드가 올라간 상태라면
        if (isPlaybackSpeedUp)
        {
            videoPlayer.playbackSpeed = 1f;
            playbackSpeedButton.image.sprite = x1SpriteSet.defaultSprite;

            playbackSpeedButton.spriteState = x1SpriteSet.sprites;
        }
        //스피드가 안올라간 상태라면
        else
        {
            videoPlayer.playbackSpeed = 2f;
            playbackSpeedButton.image.sprite = x2SpriteSet.defaultSprite;
            playbackSpeedButton.spriteState = x2SpriteSet.sprites;
        }
        isPlaybackSpeedUp = !isPlaybackSpeedUp;
    }
    /// <summary>
    /// 해당 인덱스의 영상을 재생합니다.
    /// </summary>
    /// <param name="_index"></param>
    public void Play(int _index)
    {
        videoPlayer.clip = videoClips[_index];
        StartCoroutine(CoPlayMovie());
    }
    public void OnPressSkip()
    {
        //UIManager.Instance.PlayAudio_Click();
        isSkip = true;
    }

    private IEnumerator CoPlayMovie()
    {

        Debug.Log("CoPlayMoive");
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

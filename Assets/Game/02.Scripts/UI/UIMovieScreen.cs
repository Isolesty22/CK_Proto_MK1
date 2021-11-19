using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class UIMovieScreen : UIBase
{

    public Image blackPanel;
    public RenderTexture renderTexture;
    public VideoPlayer videoPlayer;

    [Space(5)]
    public VideoClip[] videoClips;


    public IEnumerator playingCoroutine;

    public Action OnMovieEnded = null;

    private bool isSkip = false;

    [Tooltip("true일 경우, 영상 플레이가 끝날 때 렌더텍스처를 해제하지 않습니다.")]
    private bool doNotRelease;
    private void Awake()
    {
        playingCoroutine = Playing();
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
        StartCoroutine(Playing());
    }


    public void Play(int _index)
    {
        videoPlayer.clip = videoClips[_index];
        StartCoroutine(Playing());
    }
    public void OnPressSkip()
    {
        AudioManager.Instance.Audios.audioSource_UI.PlayOneShot(AudioManager.Instance.clipDict_UI["Click"]);
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
        if (!doNotRelease)
        {
            renderTexture.Release();
        }
    }
}

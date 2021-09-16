using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 청강문화산업대, 팀 로고스플래시
/// </summary>

public class UISplashLogo : MonoBehaviour
{
    [Header("로고 이미지")]
    public Image logoImage;

    [Header("배경")]
    public Image background;

    public float progress = 0f;
    public float duration = 0f;

    [Header("로고 이미지 리스트")]
    public List<SplashImage> splashImageList = new List<SplashImage>();

    public bool isEnd = false;
    private void Awake()
    {
        isEnd = false;
    }

    public IEnumerator ProcessSplashAllImage()
    {
        int currentCount = 0;
        int count = splashImageList.Count;

        while (currentCount < count)
        {
            yield return StartCoroutine(SplashingThis(splashImageList[currentCount]));
            currentCount += 1;
        }

        isEnd = true;
    }


    private readonly Color alphaZero = new Color(1, 1, 1, 0);
    private readonly Color alphaOne = new Color(1, 1, 1, 1);
    private IEnumerator SplashingThis(SplashImage _splashImage)
    {
        progress = 0f;
        float timer = 0f;

        //알파값 0으로 변경
        logoImage.color = alphaZero;
        duration = _splashImage.duration_wait;

        //wait만큼 대기
        while (timer < duration)
        {
            timer += Time.smoothDeltaTime;

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        //초기화
        timer = 0;

        logoImage.sprite = _splashImage.sprite;
        duration = _splashImage.duration_front;
        yield return null;

        //페이드 인
        while (progress < 1f)
        {
            timer += Time.smoothDeltaTime;
            progress = timer / duration;

            logoImage.color = new Color(1, 1, 1, progress);

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        logoImage.color = alphaOne;

        //초기화
        timer = 0f;
        duration = _splashImage.duration_mid;

        //대기
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        //초기화
        progress = 0f;
        timer = 0f;
        duration = _splashImage.duration_back;
        yield return null;

        //페이드 아웃
        while (progress < 1f)
        {
            timer += Time.smoothDeltaTime;
            progress = timer / duration;

            logoImage.color = new Color(1, 1, 1, 1f - progress);

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        logoImage.color = alphaZero;
        yield break;
    }

}

[System.Serializable]
public class SplashImage
{
    [Header("재생 이미지")]
    public Sprite sprite;

    [Header("재생 시간")]

    [Tooltip("대기 시간"), Range(0.2f, 2f)]
    public float duration_wait;

    [Tooltip("페이드 인 시간"), Range(0.2f, 2f)]
    public float duration_front;

    [Tooltip("온전한 로고가 보여지는 시간"), Range(0.2f, 2f)]
    public float duration_mid;

    [Tooltip("페이드 아웃 시간"), Range(0.2f, 2f)]
    public float duration_back;
}

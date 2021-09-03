using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 청강문화산업대, 팀 로고스플래시
/// </summary>

public class UISplashLogo : MonoBehaviour
{


    [Header("로고 이미지 리스트")]
    public List<SplashImage> splashImageList = new List<SplashImage>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class SplashImage
{
    [Header("재생 이미지")]
    public Image image;

    [Header("재생 시간")]

    [Tooltip("페이드 인 시간"), Range(0.2f, 1.5f)]
    public float duration_front;

    [Tooltip("온전한 로고가 보여지는 시간"), Range(0.2f, 1.5f)]
    public float duration_mid;

    [Tooltip("페이드 아웃 시간"), Range(0.2f, 1.5f)]
    public float duration_back;
}

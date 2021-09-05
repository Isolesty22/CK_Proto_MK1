using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// û����ȭ�����, �� �ΰ��÷���
/// </summary>

public class UISplashLogo : MonoBehaviour
{
    [Header("�ΰ� �̹���")]
    public Image logoImage;

    [Header("���")]
    public Image background;

    public float progress = 0f;
    public float duration = 0f;

    [Header("�ΰ� �̹��� ����Ʈ")]
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

        //���İ� 0���� ����
        logoImage.color = alphaZero;
        duration = _splashImage.duration_wait;

        //wait��ŭ ���
        while (timer < duration)
        {
            timer += Time.smoothDeltaTime;

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        //�ʱ�ȭ
        timer = 0;

        logoImage.sprite = _splashImage.sprite;
        duration = _splashImage.duration_front;
        yield return null;

        //���̵� ��
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

        //�ʱ�ȭ
        timer = 0f;
        duration = _splashImage.duration_mid;

        //���
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            //if (Input.anyKeyDown)
            //{
            //    break;
            //}
            yield return null;
        }

        //�ʱ�ȭ
        progress = 0f;
        timer = 0f;
        duration = _splashImage.duration_back;
        yield return null;

        //���̵� �ƿ�
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
    [Header("��� �̹���")]
    public Sprite sprite;

    [Header("��� �ð�")]

    [Tooltip("��� �ð�"), Range(0.2f, 2f)]
    public float duration_wait;

    [Tooltip("���̵� �� �ð�"), Range(0.2f, 2f)]
    public float duration_front;

    [Tooltip("������ �ΰ� �������� �ð�"), Range(0.2f, 2f)]
    public float duration_mid;

    [Tooltip("���̵� �ƿ� �ð�"), Range(0.2f, 2f)]
    public float duration_back;
}

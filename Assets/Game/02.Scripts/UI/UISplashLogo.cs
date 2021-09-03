using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// û����ȭ�����, �� �ΰ��÷���
/// </summary>

public class UISplashLogo : MonoBehaviour
{


    [Header("�ΰ� �̹��� ����Ʈ")]
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
    [Header("��� �̹���")]
    public Image image;

    [Header("��� �ð�")]

    [Tooltip("���̵� �� �ð�"), Range(0.2f, 1.5f)]
    public float duration_front;

    [Tooltip("������ �ΰ� �������� �ð�"), Range(0.2f, 1.5f)]
    public float duration_mid;

    [Tooltip("���̵� �ƿ� �ð�"), Range(0.2f, 1.5f)]
    public float duration_back;
}

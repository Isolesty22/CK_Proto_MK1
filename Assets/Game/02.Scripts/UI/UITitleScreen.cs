using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타이틀 화면을 담당한다-!! 
/// </summary>
public class UITitleScreen : UIBase
{

    [Space(10)]
    [Tooltip("현재 load 중 뭘 하고 있는지 표시하는 텍스트")]
    public Text text_load;


    [SerializeField]
    [Tooltip("시연용 빌드 알림 텍스트")]
    private GameObject text_siyeon;

    [Tooltip("아무 키나 누르세요 텍스트")]
    public Image text_pressKey;


#if __SIYEON__
    private void Awake()
    {
        text_siyeon.SetActive(true);
    }

#endif
    public override void Init()
    {
        base.Init();
    }

    public void SetText_Load(string _text)
    {
        text_load.text = _text;
    }
    public void SetText_pressKey(bool _isActive)
    {
        text_pressKey.gameObject.SetActive(_isActive);
    }

    [Header("Press Any Key가 깜빡이는 속도")]
    public float blinkSpeed = 3f;
    private float runningTime = 0f;
    public IEnumerator ProcessWaitPressAnyKey()
    {
        while (true)
        {
            //아무 키나 눌렀을 경우
            if (Input.anyKeyDown)
            {
                break;
            }

            runningTime += Time.deltaTime * blinkSpeed;
            text_pressKey.color = new Color(1f, 1f, 1f, Mathf.PingPong(runningTime, 1f));
            yield return null;
        }
        text_pressKey.color = new Color(1f, 1f, 1f, 1f);
        yield break;
    }

    public override bool Open()
    {
        fadeDuration = 0.5f;
        StartCoroutine(ProcessOpen());
        return true;
    }
    public override bool Close()
    {
        fadeDuration = 1f;
        StartCoroutine(ProcessClose());
        return true;
    }

    //protected override IEnumerator ProcessOpen()
    //{
    //    return base.ProcessOpen();
    //}
    protected override IEnumerator ProcessClose()
    {
        return base.ProcessClose();
    }
}

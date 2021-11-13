using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{

    public Image backgroundImage;
    public Image loadingBarImage;

    [Space(5)]
    public RectTransform circleTransform;

    [Space(5)]
    public Text tooltipText;
    private IEnumerator RotateCircle;


    private void Start()
    {
        Init();
        //RegisterUIManager();
    }

    public void SetText(string _text)
    {
        tooltipText.text = _text;
    }
    public override void Init()
    {
        base.Init();
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        isOpen = false;
        fadeDuration = 0.3f;
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        Debug.Log("Close!");
        fadeDuration = 0.5f;
        StartCoroutine(ProcessClose());
        return true;
    }

    public IEnumerator OpenThis()
    {
        RotateCircle = ProcessRotateCircle();
        fadeDuration = 0.3f;
        StartCoroutine(RotateCircle);
        yield return StartCoroutine(ProcessOpen());
        isOpen = true;
    }

    protected override IEnumerator ProcessClose()
    {
        yield return StartCoroutine(base.ProcessClose());
        StopCoroutine(RotateCircle);
        SceneChanger.Instance.isLoading = false;
    }

    private IEnumerator ProcessRotateCircle()
    {
        float timer = 0f;
        Vector3 rot = new Vector3(0f, 0f, -5f);
        while (true)
        {
            timer += Time.fixedUnscaledDeltaTime;
            circleTransform.Rotate(rot);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
    }

}

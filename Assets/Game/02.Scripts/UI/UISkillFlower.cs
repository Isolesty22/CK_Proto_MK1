using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FlowerValue = UISkillGauge.FlowerValue;

public class UISkillFlower : MonoBehaviour
{
    private Image currentImage;
    public Image image_blueFlower;
    private RectTransform rect_blueFlower;
    public Image image_orngFlower;
    private RectTransform rect_orngFlower;

    private Action currentAnimAction;

    [Tooltip("�ִϸ��̼��� ���� ���ΰ�?")]
    public bool isPlaying;

    [HideInInspector]
    public FlowerValue flowerValue;


    public enum eState
    {
        blueOff,
        blueOn,
        orangeOn
    }

    /// <summary>
    /// �ִϸ��̼� ó���� ���� ť
    /// </summary>
    private Queue<Action> animQueue = new Queue<Action>();

    public eState currentState;

    private void Awake()
    {
        rect_blueFlower = image_blueFlower.GetComponent<RectTransform>();
        rect_orngFlower = image_orngFlower.GetComponent<RectTransform>();

        image_orngFlower.enabled = false;
        image_blueFlower.enabled = false;
    }

    private void Update()
    {
        if (animQueue.Count != 0)
        {
            UpdateAnimation();
        }
    }

    /// <summary>
    /// ���� �ǿ�ϴ�.
    /// </summary>
    public void On()
    {

        Debug.Log("On Flower");
        //if (isPlaying)
        //{
        animQueue.Enqueue(() => StartCoroutine(ProcessOn()));
        //}
        //else
        //{
        //    StartCoroutine(ProcessOn());
        //}
    }

    public void HighlightEnd()
    {
        animQueue.Enqueue(() => StartCoroutine(ProcessHighlightEnd()));
    }



    /// <summary>
    /// ������ ���� ����ϴ�.
    /// </summary>
    public void HighlightOn()
    {
        animQueue.Enqueue(() => StartCoroutine(ProcessHighlightOn()));
    }

    /// <summary>
    /// ������ �ɰ� �׳� ���� �Բ� ������ϴ�.
    /// </summary>
    public void HighlightOff()
    {
        animQueue.Enqueue(() => StartCoroutine(ProcessHighlightOff()));
    }


    /// <summary>
    /// 
    /// </summary>

    public void Off()
    {
        if (isPlaying)
        {
            animQueue.Enqueue(() => StartCoroutine(ProcessOff()));
        }
        else
        {
            StartCoroutine(ProcessOff());
        }
    }

    private void UpdateAnimation()
    {
        if (isPlaying)
        {
            return;
        }
        Debug.Log("UpdateANim");
        currentAnimAction = animQueue.Dequeue();
        currentAnimAction();
    }

    /// <summary>
    /// Ÿ�̸ӿ� ���α׷����� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void ClearTimer()
    {
        timer = 0f;
        progress = 0f;
    }

    #region Coroutine------------------

    private float timer = 0f;
    private float progress = 0f;

    private Color32 alphaZero = new Color32(255, 255, 255, 0);
    private Color32 alphaFull = new Color32(255, 255, 255, 255);

    private Vector2 smallSize = new Vector2(56f, 46f);
    private Vector2 bigSize = new Vector2(78, 65f);

    public IEnumerator ProcessOn()
    {
        Debug.Log("On!");
        isPlaying = true;
        currentState = eState.blueOn;

        ClearTimer();

        //�Ķ� �� �̹����� �Ѿ���
        rect_blueFlower.sizeDelta = smallSize;
        image_blueFlower.color = alphaZero;
        image_blueFlower.enabled = true;


        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flowerValue.time_fade;
            //rect_blueFlower.sizeDelta = Vector2.Lerp(smallScale, bigScale, progress);
            image_blueFlower.color = Color32.Lerp(alphaZero, alphaFull, progress);

            yield return null;
        }
        ClearTimer();

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = (timer / flowerValue.time_on);
            progress *= flowerValue.scaleCurve.Evaluate(progress);
            rect_blueFlower.sizeDelta = Vector2.Lerp(smallSize, bigSize, progress);
            //image_blueFlower.color = Color32.Lerp(alphaZero, alphaFull, progress);

            yield return null;
        }
        isPlaying = false;
    }

    public IEnumerator ProcessOff()
    {
        isPlaying = true;
        currentState = eState.blueOff;
        ClearTimer();

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = (timer / flowerValue.time_on);
            //progress *= flowerValue.scaleCurve.Evaluate(progress);
            rect_blueFlower.sizeDelta = Vector2.Lerp(bigSize, smallSize, progress);
            //image_blueFlower.color = Color32.Lerp(alphaZero, alphaFull, progress);

            yield return null;
        }

        ClearTimer();


        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flowerValue.time_fade;
            //rect_blueFlower.sizeDelta = Vector2.Lerp(smallScale, bigScale, progress);
            image_blueFlower.color = Color32.Lerp(alphaFull, alphaZero, progress);

            yield return null;
        }

        image_blueFlower.enabled = false;
        isPlaying = false;
    }

    public IEnumerator ProcessHighlightOn()
    {
        isPlaying = true;
        currentState = eState.orangeOn;
        ClearTimer();

        rect_orngFlower.sizeDelta = bigSize;
        image_orngFlower.color = alphaZero;
        image_orngFlower.enabled = true;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flowerValue.time_highlightOn;
            image_orngFlower.color = Color32.Lerp(alphaZero, alphaFull, progress);
            yield return null;
        }

        isPlaying = false;
    }
    public IEnumerator ProcessHighlightOff()
    {
        isPlaying = true;
        currentState = eState.blueOff;
        ClearTimer();

        rect_orngFlower.sizeDelta = bigSize;
        image_orngFlower.color = alphaFull;
        // image_orngFlower.enabled = true;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flowerValue.time_highlightOff;
            image_orngFlower.color = Color32.Lerp(alphaFull, alphaZero, progress);
            image_blueFlower.color = Color32.Lerp(alphaFull, alphaZero, progress);
            rect_blueFlower.sizeDelta = Vector2.Lerp(bigSize, smallSize, progress);
            rect_orngFlower.sizeDelta = Vector2.Lerp(bigSize, smallSize, progress);

            yield return null;
        }
        image_orngFlower.enabled = false;
        image_blueFlower.enabled = false;
        isPlaying = false;
    }

    public IEnumerator ProcessHighlightEnd()
    {
        isPlaying = true;
        currentState = eState.blueOff;
        ClearTimer();


        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flowerValue.time_fade;
            //rect_blueFlower.sizeDelta = Vector2.Lerp(smallScale, bigScale, progress);
            image_orngFlower.color = Color32.Lerp(alphaFull, alphaZero, progress);

            yield return null;
        }

        image_orngFlower.enabled = false;
        isPlaying = false;
    }

    #endregion

}

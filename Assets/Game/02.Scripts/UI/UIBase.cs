using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI들의 상위 클래스 
/// </summary>
public class UIBase : MonoBehaviour
{

    private readonly float fadeDuration = 0.2f;

    [Tooltip("열려있는 상태일 때 true를 반환함!")]
    public bool isOpen;

    #region definitions

    [Serializable]
    public class Components
    {
        [Tooltip("실제 이미지 등이 들어있는 게임 오브젝트입니다.")]
        public GameObject contentObject;

        public Canvas canvas;
        public CanvasGroup canvasGroup;

    }
    #endregion

    #region field

    [SerializeField] protected Components components = new Components();

    public Components Com => components;

    #endregion



    private void Start()
    {
        Init();
        RegisterUIManager();
    }

    public virtual void Init()
    {
        Com.canvas = GetComponent<Canvas>();
        Com.canvasGroup = GetComponent<CanvasGroup>();
        CheckOpen();
    }

    /// <summary>
    /// canvas의 활성화 상태에 따라 enabled와 isOpen의 값을 결정합니다.
    /// </summary>
    protected virtual void CheckOpen()
    {
        isOpen = Com.canvas.enabled ? true : false;
        this.enabled = isOpen;
    }

    public virtual bool Open()
    {
        Com.canvas.enabled = true;
        isOpen = true;

        this.enabled = true;

        return true;
    }
    public virtual bool Close()
    {
        Com.canvas.enabled = false;
        isOpen = false;

        this.enabled = false;

        return true;
    }

    protected IEnumerator ProcessOpen()
    {
        isOpen = false;

        float progress = 0f;
        float timer = 0f;

        //알파값 0으로 변경
        Com.canvasGroup.alpha = 0f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 인
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 1f;

        isOpen = Com.canvas.enabled;

        yield break;
    }

    protected IEnumerator ProcessClose()
    {
        isOpen = true;
        //  yield return new WaitUntil(() =>SceneChanger.Instance.);
        float progress = 0f;
        float timer = 0f;

        //알파값 1로 변경
        Com.canvasGroup.alpha = 1f;

        //캔버스 활성화
        Com.canvas.enabled = true;

        //페이드 아웃
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeDuration;

            Com.canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        Com.canvasGroup.alpha = 0f;
        Com.canvas.enabled = false;

        isOpen = Com.canvas.enabled;

        yield break;
    }





    /// <summary>
    /// UIManager에 해당 UI를 등록합니다.
    /// </summary>
    public virtual void RegisterUIManager()
    {
        UIManager.Instance.RegisterListThis(this);
    }

}

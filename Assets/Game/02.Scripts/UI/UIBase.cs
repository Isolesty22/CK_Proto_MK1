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

    #region definitions

    [Serializable]
    public class Components
    {
        public Canvas canvas;
        public CanvasGroup canvasGroup;
    }
    #endregion

    #region field

    [SerializeField] protected Components components = new Components();

    public Components Com => components;

    #endregion

    [Tooltip("열려있는 상태일 때 true를 반환함!")]
    public bool isOpen;

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
    protected void CheckOpen()
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

    /// <summary>
    /// UIManager에 해당 UI를 등록합니다.
    /// </summary>
    public virtual void RegisterUIManager()
    {
        UIManager.Instance.RegisterThis(this);
    }

}

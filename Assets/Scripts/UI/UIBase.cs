using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Components components = new Components();

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
    }

    public virtual void Open() { }
    public virtual void Close() { }

    /// <summary>
    /// UIManager에 해당 UI를 등록합니다.
    /// </summary>
    public virtual void RegisterUIManager()
    {
        UIManager.Instance.RegisterThis(this);
    }

}

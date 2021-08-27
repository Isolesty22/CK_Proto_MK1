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
    /// <summary>
    /// UIBase의 컴포넌트 모음.
    /// </summary>
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

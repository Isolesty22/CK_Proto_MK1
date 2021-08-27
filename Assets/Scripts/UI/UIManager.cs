using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI를 작동/관리하는 매니저 클래스
/// </summary>

public class UIManager : MonoBehaviour
{

    #region Instance
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    #endregion

    private List<UIBase> uiList = new List<UIBase>();


    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;

            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
        }
    }


    /// <summary>
    /// uiList에 해당 uiBase를 Add합니다.
    /// </summary>
    public void RegisterThis(UIBase _uiBase)
    {
        uiList.Add(_uiBase);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    #region Instance
    private static SceneChanger instance;
    public static SceneChanger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneChanger>();
            }
            return instance;
        }
    }
    #endregion

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
}

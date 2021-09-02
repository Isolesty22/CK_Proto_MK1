using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임 사운드 관리
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
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
    }
}

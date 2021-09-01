using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 입/출력 관리
/// </summary>
public class DataManager : MonoBehaviour
{
    #region Instance
    private static DataManager instance;
    public static DataManager Instance;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
        }
    }
}

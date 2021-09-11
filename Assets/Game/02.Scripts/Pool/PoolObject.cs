using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolObject
{
    [Header("스폰 이름")]
    [Tooltip("오브젝트를 Spawn할 때, 해당 값을 통해 \n오브젝트를 찾습니다.")]
    public string name;

    [Header("프리팹")]
    public GameObject gameObject;

    [Header("초기 생성 개수")]
    public int count;
}

[System.Serializable]
public class CustomPoolObject
{
    [Header("스폰 이름")]
    [Tooltip("오브젝트를 Spawn할 때, 해당 값을 통해 \n오브젝트를 찾습니다.")]
    public string name;

    [Header("프리팹")]
    public GameObject gameObject;

    [Header("초기 생성 개수")]
    public int count;
}
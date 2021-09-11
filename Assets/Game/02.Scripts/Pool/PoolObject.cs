using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

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
    [Header("타입 이름")]
    [Tooltip("클래스명입니다. 풀을 생성할 때, 해당 이름을 통해 딕셔너리를 초기화합니다.")]
    public string typeName;

    [Header("프리팹")]
    public GameObject gameObject;

    [Header("초기 생성 개수")]

    public int count;

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CustomPoolManager : MonoBehaviour
{
    #region Instance
    private static CustomPoolManager instance;
    public static CustomPoolManager Instance;
    #endregion


    public List<CustomPoolObject> poolObjectList;
    private Dictionary<string, CustomPoolObject> poolObjectDictionary;

    private Dictionary<string, object> poolDictionary = new Dictionary<string, object>();

    private void Awake()
    {
        #region Instance
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("이미 인스턴스가 존재합니다." + this);
            if (Instance != this) //나 자신이 인스턴스가 아닐 경우
            {
                Debug.Log(this + " : 더 이상, 이 세계선에서는 존재할 수 없을 것 같아... 안녕.");
                Destroy(this.gameObject);
            }

        }
        #endregion

        Init_Dict();
    }

    private void Init_Dict()
    {
        poolObjectDictionary = new Dictionary<string, CustomPoolObject>();

        for (int i = 0; i < poolObjectList.Count; i++)
        {
            poolObjectDictionary.Add(poolObjectList[i].typeName,poolObjectList[i]);
        }
    }

    /// <summary>
    /// [자주 사용하지 마세요] 해당 타입의 CustomPool을 반환합니다.
    /// </summary>
    /// <typeparam name="T">CustomPool의 지정된 타입</typeparam>
    public CustomPool<T> GetPool<T>() where T : MonoBehaviour
    {
        return poolDictionary[typeof(T).Name] as CustomPool<T>;
    }


    /// <summary>
    /// 지정한 타입의 CustomPool을 생성합니다.
    /// </summary>
    /// <typeparam name="T">CustomPool이 다룰 Type</typeparam>
    /// <returns>생성된 CustomPool을 반환합니다.</returns>
    public CustomPool<T> CreateCustomPool<T>() where T : MonoBehaviour
    {
        string typeName = typeof(T).Name;

        GameObject pool = new GameObject("CustomPool : " + typeof(T).Name);
        pool.transform.SetParent(this.transform);

        CustomPool<T> customPool = new CustomPool<T>();

        //딕셔너리에 넣기
        poolDictionary.Add(typeName, customPool);
        //pool.AddComponent<CustomPool<T>>();
        customPool.Init(poolObjectDictionary[typeName].gameObject, poolObjectDictionary[typeName].count, pool.transform);
        customPool.Init_Queue();
        return customPool;
    }



}

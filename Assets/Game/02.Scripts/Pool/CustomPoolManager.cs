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

    [Header("CustomPoolManager Transform")]
    public Transform myTransform;
    [Tooltip("풀 오브젝트 리스트"),Space(5)]
    public List<CustomPoolObject> poolObjectList;
    private Dictionary<string, CustomPoolObject> poolObjectDictionary;

    private Dictionary<string, object> poolDictionary = new Dictionary<string, object>();

    #region ClearHandler
    /// <summary>
    /// ClearPool
    /// </summary>
    private delegate void ClearHandler();
    ClearHandler clearHandler;
    #endregion

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
        Init_PoolObjectDictionary();
    }

    private void Init_PoolObjectDictionary()
    {
        poolObjectDictionary = new Dictionary<string, CustomPoolObject>();

        for (int i = 0; i < poolObjectList.Count; i++)
        {
            poolObjectDictionary.Add(poolObjectList[i].typeName, poolObjectList[i]);
        }
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
        Transform tempPoolTransform = pool.transform;
        tempPoolTransform.SetParent(myTransform);

        //CustomPool 생성
        CustomPool<T> customPool = new CustomPool<T>();

        //딕셔너리에 넣기
        poolDictionary.Add(typeName, customPool);

        customPool.Init(poolObjectDictionary[typeName].gameObject, poolObjectDictionary[typeName].count, tempPoolTransform);
        customPool.Init_Queue();

        //clearHandler에 해당 풀의 ClearPool 추가
        clearHandler += new ClearHandler(customPool.ClearPool);
        return customPool;
    }

    /// <summary>
    /// [너무 자주 사용하지 마세요] 해당 타입의 오브젝트를 스폰합니다.
    /// </summary>
    /// <typeparam name="T">CustomPool의 지정된 타입</typeparam>
    public T SpawnThis<T>(Vector3 _position, Vector3 _rotation, Transform _parent) where T : MonoBehaviour
    {
        CustomPool<T> customPool = poolDictionary[typeof(T).Name] as CustomPool<T>;
        return customPool.SpawnThis(_position, _rotation, _parent);
    }


    /// <summary>
    ///  [너무 자주 사용하지 마세요] 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="_object">반환할 오브젝트.</param>
    public void ReleaseThis<T>(T _object) where T : MonoBehaviour
    {
        CustomPool<T> customPool = poolDictionary[typeof(T).Name] as CustomPool<T>;
        customPool.ReleaseThis(_object);
    }


    /// <summary>
    /// [너무 자주 사용하지 마세요] 해당 타입의 CustomPool을 반환합니다.
    /// </summary>
    /// <typeparam name="T">CustomPool의 지정된 타입</typeparam>
    public CustomPool<T> GetPool<T>() where T : MonoBehaviour
    {
        return poolDictionary[typeof(T).Name] as CustomPool<T>;
    }

    public void ClearPool()
    {
        //만들어졌던 CustomPool들의 ClearPool() 호출
        clearHandler();

        //poolObjectDictionary.Clear();
        poolDictionary.Clear();
        clearHandler = null;
    }
}

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



    public GameObject arrow;
    public GameObject bullet;

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
    }

    private IEnumerator Start()
    {
        //풀 생성하기
        CustomPool<TestArrow> arrowPool = CreateCustomPool<TestArrow>(arrow, 5);
        CustomPool<TestBullet> bulletPool = CreateCustomPool<TestBullet>(bullet, 5);

        for (int i = 0; i < 6; i++)
        {
            bulletPool.SpawnThis(Vector3.zero, Vector3.zero, null);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }


    public CustomPool<T> GetPool<T>() where T : MonoBehaviour
    {
        return poolDictionary[typeof(T).Name] as CustomPool<T>;
    }

    /// <summary>
    /// 지정한 타입의 CustomPool을 생성합니다.
    /// </summary>
    /// <typeparam name="T">CustomPool이 다룰 Type</typeparam>
    /// <param name="_prefab">Clone될 오브젝트.</param>
    /// <param name="_count">초기 생성할 오브젝트 개수.</param>
    /// <returns>생성된 CustomPool을 반환합니다.</returns>
    public CustomPool<T> CreateCustomPool<T>(GameObject _prefab, int _count) where T : MonoBehaviour
    {
        GameObject pool = new GameObject("CustomPool : " + typeof(T).Name);
        pool.transform.SetParent(this.transform);

        CustomPool<T> customPool = new CustomPool<T>();

        //딕셔너리에 넣기
        poolDictionary.Add(typeof(T).Name, customPool);
        //pool.AddComponent<CustomPool<T>>();
        customPool.Init(_prefab, _count, pool.transform);
        customPool.Init_Queue();
        return customPool;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class PoolManager : MonoBehaviour
{
    #region Instance
    private static PoolManager instance;
    public static PoolManager Instance;
    #endregion

    [Header("PoolManager Transform")]
    public Transform myTransform;

    [Tooltip("풀 오브젝트 리스트"),Space(5)]
    public List<PoolObject> poolObjectList;

    private Dictionary<string, GameObject> poolObjectDictionary;

    private Dictionary<string, Transform> transformDictionary;

    private Dictionary<string, Queue<GameObject>> queueDictionary;

    private int listCount;

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

        listCount = poolObjectList.Count;

    }


    private readonly string str_pool = "Pool";

    public void Init_Transforms()
    {
        transformDictionary = new Dictionary<string, Transform>();

        string poolName = string.Empty;

        //리스트에 있는 것들로 풀 트랜스폼 딕셔너리 생성 
        for (int i = 0; i < listCount; i++)
        {
            //풀 오브젝트 이름 생성
            poolName = poolObjectList[i].name + str_pool;

            //게임 오브젝트 생성
            GameObject tempObject = new GameObject(poolName);

            //Parent 설정
            tempObject.transform.SetParent(myTransform);

            //딕셔너리에 추가
            transformDictionary.Add(poolObjectList[i].name, tempObject.transform);
        }
    }
    public void Init_PoolObjectDictionary()
    {
        poolObjectDictionary = new Dictionary<string, GameObject>();

        //리스트에 있는 것들을 딕셔너리에 넣기
        for (int i = 0; i < listCount; i++)
        {
            poolObjectDictionary.Add(poolObjectList[i].name, poolObjectList[i].gameObject);
        }
    }


    public void Init_Queue()
    {
        queueDictionary = new Dictionary<string, Queue<GameObject>>();

        for (int i = 0; i < listCount; i++)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queueDictionary.Add(poolObjectList[i].name, queue);
        }
    }

    public void InstantiatePoolObjects()
    {
        int tempCount = 0;
        string tempName = string.Empty;
        GameObject tempGameObject = null;
        Transform tempTransform = null;

        for (int i = 0; i < listCount; i++)
        {
            //생성할 애들 설정
            tempCount = poolObjectList[i].count;
            tempName = poolObjectList[i].name;
            tempTransform = transformDictionary[tempName];

            for (int j = 0; j < tempCount; j++)
            {
                tempGameObject = GameObject.Instantiate(
                    poolObjectList[i].gameObject, Vector3.zero,
                    Quaternion.identity, tempTransform);

                tempGameObject.SetActive(false);
                //큐에 넣기
                queueDictionary[tempName].Enqueue(tempGameObject);
            }
        }
    }

    private GameObject CreatePoolObject(string _name)
    {
        //오브젝트 생성
        GameObject tempGameObject = GameObject.Instantiate(
                        poolObjectDictionary[_name], Vector3.zero,
                        Quaternion.identity, transformDictionary[_name]);

        tempGameObject.SetActive(false);
        //큐에 넣기
        queueDictionary[_name].Enqueue(tempGameObject);
        return tempGameObject;
    }


    private GameObject tempObject = null;
    private Transform tempTransform = null;
    /// <summary>
    /// 오브젝트를 스폰합니다.
    /// </summary>
    /// <param name="_name">스폰할 오브젝트의 이름. 매니저에서 설정했던 name입니다.</param>
    /// <param name="_position">스폰할 오브젝트의 위치.</param>
    /// <param name="_rotation">스폰할 오브젝트의 회전값.</param>
    /// <param name="_parent">스폰할 오브젝트의 부모 오브젝트. 부모가 필요없다면 null을 넣습니다.</param>
    public GameObject SpawnThis(string _name, Vector3 _position, Vector3 _rotation, Transform _parent)
    {
        var tempQueue = queueDictionary[_name];


        lock (tempQueue)
        {
            if (tempQueue.Count == 0)
            {

                Debug.LogWarning("[ " + _name + "Pool ] 풀이 텅텅 비어있습니다. 오브젝트 하나를 새로 만듭니다.");
                CreatePoolObject(_name);
            }

            tempObject = tempQueue.Dequeue();

            tempTransform = tempObject.transform;

            //빼주기
            tempTransform.SetParent(null);

            //위치, 회전값 설정
            tempTransform.SetPositionAndRotation(_position, Quaternion.Euler(_rotation));

            //부모 설정
            tempTransform.SetParent(_parent);

            tempObject.SetActive(true);
            return tempObject;

        }
    }

    /// <summary>
    /// 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="_name">반환할 오브젝트의 이름. 매니저에서 설정했던 name입니다.</param>
    /// <param name="_gameObject">반환할 오브젝트 그 자체.</param>
    public void ReleaseThis(string _name, GameObject _gameObject)
    {
        _gameObject.SetActive(false);
        _gameObject.transform.SetParent(transformDictionary[_name]);
        queueDictionary[_name].Enqueue(_gameObject);
    }

    /// <summary>
    /// 모든걸 파괴시킵니다.
    /// </summary>
    public void ClearPool()
    {
        transformDictionary.Clear();
        queueDictionary.Clear();
        //poolObjectDictionary.Clear();

        foreach (Transform child in myTransform)
        {
            Destroy(child.gameObject);
        }
    }
}

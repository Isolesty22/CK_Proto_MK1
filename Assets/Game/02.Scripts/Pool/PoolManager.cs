using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Instance
    private static PoolManager instance;
    public static PoolManager Instance;
    #endregion

    [Header("PoolManager Transform")]
    public Transform myTransform;

    [Header("풀 오브젝트 리스트")]
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
        Init_Transforms();
        Init_PoolObjectDictionary();
        Init_Queue();
        InstantiatePoolObjects();
    }


    private readonly string str_pool = "Pool";

    private void Init_Transforms()
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
    private void Init_PoolObjectDictionary()
    {
        poolObjectDictionary = new Dictionary<string, GameObject>();

        //리스트에 있는 것들을 딕셔너리에 넣기
        for (int i = 0; i < listCount; i++)
        {
            poolObjectDictionary.Add(poolObjectList[i].name, poolObjectList[i].gameObject);
        }
    }


    private void Init_Queue()
    {
        for (int i = 0; i < listCount; i++)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queueDictionary.Add(poolObjectList[i].name, queue);
        }
    }

    private void InstantiatePoolObjects()
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
            }
        }
    }


}

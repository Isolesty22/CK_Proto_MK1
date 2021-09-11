using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePoolType
{
    CustomPool,
    GameObjectPool,
}
public class PoolTester : MonoBehaviour
{
    public ePoolType poolType;

    private PoolManager poolManager;
    private CustomPoolManager cPoolManager;
    private void Start()
    {
        poolManager = PoolManager.Instance;
        cPoolManager = CustomPoolManager.Instance;

        if (poolType == ePoolType.CustomPool)
        {
            StartCoroutine(Start_CustomPool());
            poolManager.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Start_GameObjectPool());
            cPoolManager.gameObject.SetActive(false);
        }
    }

    private IEnumerator Start_GameObjectPool()
    {
        //밑작업
        poolManager.Init_Transforms();
        poolManager.Init_PoolObjectDictionary();
        poolManager.Init_Queue();
        poolManager.InstantiatePoolObjects();

        for (int i = 0; i < 3; i++)
        {
            poolManager.SpawnThis("Arrow", Vector3.zero, Vector3.zero, null).GetComponent<TestArrow>().Shot();

            yield return new WaitForSecondsRealtime(0.2f);
        }


    }


    CustomPool<TestArrow> arrowPool;
    CustomPool<TestBullet> bulletPool;
    private IEnumerator Start_CustomPool()
    {

        //느림...하지만 간편
        cPoolManager.CreateCustomPool<TestArrow>();
        cPoolManager.CreateCustomPool<TestBullet>();

        int count = cPoolManager.GetPool<TestArrow>().count;
        for (int i = 0; i < count+1; i++)
        {
            cPoolManager.GetPool<TestArrow>().SpawnThis(Vector3.zero, Vector3.zero, null).Shot();
            yield return new WaitForSecondsRealtime(0.2f);
        }

        //빠름...하지만 변수를 만들어놓는게 필요
        //arrowPool = cPoolManager.CreateCustomPool<TestArrow>();
        //bulletPool = cPoolManager.CreateCustomPool<TestBullet>();
        //int count = arrowPool.count;
        //for (int i = 0; i < count; i++)
        //{
        //    arrowPool.SpawnThis(Vector3.zero, Vector3.zero, null).Shot();
        //    yield return new WaitForSecondsRealtime(0.2f);
        //}

    }

}



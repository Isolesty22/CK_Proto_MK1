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
        //사용 전 초기화
        poolManager.Init_Transforms();
        poolManager.Init_PoolObjectDictionary();
        poolManager.Init_Queue();
        poolManager.InstantiatePoolObjects();

        for (int i = 0; i < 3; i++)
        {
            poolManager.SpawnThis("Arrow", Vector3.zero, Vector3.zero, null);

            yield return new WaitForSecondsRealtime(0.2f);
        }

        yield return new WaitForSecondsRealtime(5f);
        Debug.Log("내용물을 지웠습니다.");

        //풀 클리어
        poolManager.ClearPool();
        //하이어라키에서 지정해놓은 리스트들과, 그걸 바탕으로 만든 딕셔너리 등은 지워지지 않습니다.

        //Clear 후 재사용
        //poolManager.Init_Transforms();
        //poolManager.Init_Queue();
        //poolManager.InstantiatePoolObjects();
        //Init_PoolObjectDictionary는 하지 않아도 무방

    }


    CustomPool<TestArrow> arrowPool;
    CustomPool<TestBullet> bulletPool;
    private IEnumerator Start_CustomPool()
    {

        //느림...하지만 간편하다
        //cPoolManager.CreateCustomPool<TestArrow>();
        //cPoolManager.CreateCustomPool<TestBullet>();

        //int count = cPoolManager.GetPool<TestArrow>().count;
        //for (int i = 0; i < count + 1; i++)
        //{
        //    cPoolManager.GetPool<TestArrow>().SpawnThis(Vector3.zero, Vector3.zero, null).Shot();
        //    yield return new WaitForSecondsRealtime(0.2f);
        //}

        //빠름...하지만 변수를 만들어놓는게 필요
        arrowPool = cPoolManager.CreateCustomPool<TestArrow>();
        bulletPool = cPoolManager.CreateCustomPool<TestBullet>();
        int count = arrowPool.count;
        for (int i = 0; i < count+1; i++)
        {
            arrowPool.SpawnThis(Vector3.zero, Vector3.zero, null).Shot();
            yield return new WaitForSecondsRealtime(0.2f);
        }

        yield return new WaitForSecondsRealtime(5f);
        Debug.Log("내용물을 지웠습니다.");

        //풀 클리어
        cPoolManager.ClearPool();
        //하이어라키에서 지정해놓은 리스트들과, 그걸 바탕으로 만든 딕셔너리 등은 지워지지 않습니다.

        //Clear 후 재사용
        //cPoolManager.CreateCustomPool<TestArrow>();
        //cPoolManager.CreateCustomPool<TestBullet>();
        //arrowPool = cPoolManager.CreateCustomPool<TestArrow>();
        //bulletPool = cPoolManager.CreateCustomPool<TestBullet>();
        //사용할 때랑 똑같이 하면 됨
    }

}



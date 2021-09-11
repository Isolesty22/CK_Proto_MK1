using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    CustomPoolManager cPoolManager;
    PoolManager poolManager;

    private void Start()
    {
        cPoolManager = CustomPoolManager.Instance;
        poolManager = PoolManager.Instance;
    }

    public void Shot()
    {
        StartCoroutine(ProcessShot());
    }

    private IEnumerator ProcessShot()
    {
        Debug.Log("|         <= ");

        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("|<= ");

        //Pool·Î ¹ÝÈ¯

        if (cPoolManager.gameObject.activeInHierarchy)
        {
            cPoolManager.GetPool<TestBullet>().ReleaseThis(this);
        }
        else
        {
            poolManager.ReleaseThis("Bullet", this.gameObject);
        }

    }
}

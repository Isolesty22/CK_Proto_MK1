using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrow : MonoBehaviour
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
        Debug.Log("|             <-----= ");

        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("|<-----= ");

        if (cPoolManager.gameObject.activeInHierarchy)
        {
            cPoolManager.GetPool<TestArrow>().ReleaseThis(this);
        }
        else
        {
            poolManager.ReleaseThis("Arrow", this.gameObject);
        }
    }
}

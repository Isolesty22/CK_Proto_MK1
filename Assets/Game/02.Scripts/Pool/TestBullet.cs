using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    CustomPoolManager cPoolManager;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(2f);
        cPoolManager = CustomPoolManager.Instance;

        cPoolManager.GetPool<TestBullet>().ReleaseThis(this);
    }
}

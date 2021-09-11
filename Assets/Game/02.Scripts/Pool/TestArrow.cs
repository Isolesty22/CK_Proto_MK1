using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrow : MonoBehaviour
{
    CustomPoolManager cPoolManager;

    private IEnumerator Start()
    {
        cPoolManager = CustomPoolManager.Instance;
        yield return new WaitForSecondsRealtime(2f);

        //Pool�� ��ȯ
        cPoolManager.GetPool<TestArrow>().ReleaseThis(this);
    }
}

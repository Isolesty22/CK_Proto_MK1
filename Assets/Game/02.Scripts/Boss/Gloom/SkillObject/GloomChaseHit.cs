using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomChaseHit : MonoBehaviour
{
    private WaitForSeconds waitSec = new WaitForSeconds(1f);
    private void Start()
    {
        StartCoroutine(ProcessDespawn());
    }

    private IEnumerator ProcessDespawn()
    {
        yield return waitSec;
        CustomPoolManager.Instance.GetPool<GloomChaseHit>().ReleaseThis(this);
    }
}

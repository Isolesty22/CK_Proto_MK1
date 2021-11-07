using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomChaseHit : MonoBehaviour
{
    private CustomPool<GloomChaseHit> hitPool = new CustomPool<GloomChaseHit>();

    private void Awake()
    {
        hitPool = CustomPoolManager.Instance.GetPool<GloomChaseHit>();
    }
    private WaitForSeconds waitSec = new WaitForSeconds(1f);
    private void Start()
    {
        StartCoroutine(ProcessDespawn());
    }

    private IEnumerator ProcessDespawn()
    {
        yield return waitSec;
        hitPool.ReleaseThis(this);
    }
}

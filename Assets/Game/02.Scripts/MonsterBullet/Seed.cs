using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float fireSpeed;
    public float range;
    public Vector3 fireDir;
    public Vector3 firePos;

    private void FixedUpdate()
    {
        if((firePos - transform.position).magnitude < range)
        {
            transform.Translate(fireDir * fireSpeed * Time.fixedDeltaTime);
        }
        else
        {
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var despawn = Despawn();
            StartCoroutine(despawn);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }

    public IEnumerator Despawn()
    {
        yield return null;

        CustomPoolManager.Instance.ReleaseThis(this);
    }
}

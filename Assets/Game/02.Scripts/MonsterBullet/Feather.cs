using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public bool isActive;

    public IEnumerator Shot(Vector3 firepos, float shotSpeed, float range )
    {
        while(isActive)
        {
            if ((firepos - transform.position).sqrMagnitude < range)
            {
                transform.Translate(Vector3.forward * shotSpeed * Time.fixedDeltaTime);

                yield return null;
            }
            else
            {
                isActive = false;
                CustomPoolManager.Instance.ReleaseThis(this);
            }
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
            isActive = false;
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }

    public IEnumerator Despawn()
    {
        yield return null;

        isActive = false;
        CustomPoolManager.Instance.ReleaseThis(this);
    }
}

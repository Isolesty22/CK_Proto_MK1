using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public bool isActive;

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

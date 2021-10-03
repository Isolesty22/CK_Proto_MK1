using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{
    public int damage = 1;

    public bool isActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (other.GetComponent<MonsterController>())
            {
                if (!other.GetComponent<MonsterController>().Stat.isAlive)
                    return;
            }
        }

        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().Hit(damage);
            isActive = false;
            CustomPoolManager.Instance.ReleaseThis(this);

            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isActive = false;
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }
}

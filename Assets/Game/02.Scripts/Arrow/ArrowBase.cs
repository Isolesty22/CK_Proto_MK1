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
                else
                {
                    other.GetComponent<MonsterController>().Hit(damage);

                    var hit = CustomPoolManager.Instance.arrowHitPool.SpawnThis(transform.position, transform.eulerAngles, null);
                    hit.Play();

                    isActive = false;
                    CustomPoolManager.Instance.ReleaseThis(this);
                }
            }
            else
            {
                return;
            }

            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isActive = false;
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{
    public int damage = 1;

    public bool isActive;

    private MonsterController currentMonster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (currentMonster = other.GetComponent<MonsterController>())
            {
                if (!currentMonster.Stat.isAlive)
                    return;
                else
                {
                    currentMonster.Hit(damage);

                    var hit = CustomPoolManager.Instance.arrowHitPool.SpawnThis(transform.position, transform.eulerAngles, null);
                    hit.Play();

                    isActive = false;
                    CustomPoolManager.Instance.ReleaseThis(this);
                }
            }
            else
            {
                var hit = CustomPoolManager.Instance.arrowHitPool.SpawnThis(transform.position, transform.eulerAngles, null);
                hit.Play();

                isActive = false;
                CustomPoolManager.Instance.ReleaseThis(this);
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

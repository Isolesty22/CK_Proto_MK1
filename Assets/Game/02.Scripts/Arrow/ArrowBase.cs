using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{
    public int damage = 1;

    public bool isAlive;
    public bool isCounter;

    private void OnTriggerEnter(Collider other)
    {
        if(!isCounter)
        {
            if (other.CompareTag("Monster"))
            {
                other.GetComponent<MonsterController>().Hit(damage);
                isAlive = false;
                ArrowPool.instance.Despawn(this.gameObject);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isAlive = false;
                ArrowPool.instance.Despawn(this.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Monster"))
            {
                other.GetComponent<MonsterController>().Hit(damage);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isAlive = false;
                ArrowPool.instance.Despawn(this.gameObject);
            }
        }
    }
}

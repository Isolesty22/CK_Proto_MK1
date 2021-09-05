using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{
    public bool isAlive;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().Hit();
            isAlive = false;
            ArrowPool.instance.Despawn(this.gameObject);
        }
    }
}

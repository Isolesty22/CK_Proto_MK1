using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

            if (other.CompareTag("Player"))
            {
                //other.GetComponent<MonsterController>().Hit(damage);
                //isAlive = false;
                ArrowPool.instance.Despawn(this.gameObject);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //isAlive = false;
                ArrowPool.instance.Despawn(this.gameObject);
            } 
    }
}

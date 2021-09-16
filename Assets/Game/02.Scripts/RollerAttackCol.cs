using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerAttackCol : MonsterAttackCol
{
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.transform.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAttackCol : MonsterAttackCol
{
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.ChangeState(MonsterController.MonsterState.IDLE);
        }
    }
}

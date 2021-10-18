using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitterAttackCol : MonsterAttackCol
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.inAttackCol = true;

            if (monsterController.Stat.isAlive == true)
                monsterController.ChangeState(MonsterController.MonsterState.ATTACK);
        }
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

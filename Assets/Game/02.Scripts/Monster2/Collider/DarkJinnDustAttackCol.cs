using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkJinnDustAttackCol : MonsterAttackCol
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.inAttackCol = true;

            if (monsterController.Stat.isAlive == true)
            {
                monsterController.Com.monsterModel.SetActive(true);
                monsterController.ChangeState(MonsterController.MonsterState.ATTACK);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoomSearchCol : MonoBehaviour
{
    public MonsterController monsterController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.playerOutOfRange = false;

            if (monsterController.Stat.isAlive == false)
                return;

            if (monsterController.state == MonsterController.MonsterState.WAIT)
            {
                monsterController.ChangeState(MonsterController.MonsterState.IDLE);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.playerOutOfRange = true;
        }
    }

}

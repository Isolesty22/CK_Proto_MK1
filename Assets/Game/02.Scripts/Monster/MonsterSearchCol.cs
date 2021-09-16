using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchCol : MonoBehaviour
{
    public MonsterController monsterController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            monsterController.Com.monsterModel.SetActive(true);
            monsterController.ChangeState(MonsterController.MonsterState.IDLE);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }
}
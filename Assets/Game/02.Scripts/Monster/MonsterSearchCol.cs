using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchCol : MonoBehaviour
{
    public GameObject originMonster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            if (originMonster.GetComponent<MonsterController>().state == MonsterController.MonsterState.IDLE)
            {
                originMonster.transform.SendMessage("ChangeState", "DETECT");
            }

            else if (originMonster.GetComponent<MonsterController>().state == MonsterController.MonsterState.MOVE)
            {
                originMonster.transform.SendMessage("ChangeState", "DETECT");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.transform.CompareTag("Player"))
        //{
        //    if (originMonster.GetComponent<MonsterController>().state != MonsterController.MonsterState.IDLE)
        //    {
        //        originMonster.transform.SendMessage("ChangeState", "IDLE");
        //    }
        //}
    }
}
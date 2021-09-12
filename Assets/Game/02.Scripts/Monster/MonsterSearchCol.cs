using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchCol : MonoBehaviour
{
    private MonsterController monsterController;

    private void Awake()
    {
        monsterController = transform.parent.GetComponent<MonsterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            monsterController.Com.monsterModel.SetActive(true);
            monsterController.active = true;
            monsterController.ChangeState("IDLE");
        }

        //if (gameObject.transform.parent.GetComponent<MonsterController>().active == false)
        //{
        //    monsterModel.SetActive(true);
        //    gameObject.transform.parent.GetComponent<MonsterController>().active = true;
        //    gameObject.transform.parent.SendMessage("ChangeState", "IDLE");
        //}
    }
}
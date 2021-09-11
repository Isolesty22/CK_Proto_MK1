using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchCol : MonoBehaviour
{
    public GameObject monsterModel;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.transform.parent.GetComponent<MonsterController>().active == false)
        {
            monsterModel.SetActive(true);
            gameObject.transform.parent.GetComponent<MonsterController>().active = true;
            gameObject.transform.parent.SendMessage("ChangeState", "IDLE");
        }
    }
}
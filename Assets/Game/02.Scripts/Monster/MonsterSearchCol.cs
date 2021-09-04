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
                originMonster.transform.SendMessage("ChangeState", "Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
                originMonster.transform.SendMessage("ChangeState", "Search");
        }
    }
}
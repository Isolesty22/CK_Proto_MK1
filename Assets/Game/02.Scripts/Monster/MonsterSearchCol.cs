using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            Debug.Log("Detect Player");
            transform.parent.SendMessage("ChangeState", "Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            Debug.Log("Detect Player");
            transform.parent.SendMessage("ChangeState", "Search");
        }
    }
}

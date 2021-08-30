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
            gameObject.transform.parent.SendMessage("ChangeState", "Attack");
            //gameObject.transform.parent.gameObject.transform.GetComponent<LarvaController>().ChangeState("Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            Debug.Log("Detect Player");
            gameObject.transform.parent.SendMessage("ChangeState", "Search");
            //gameObject.transform.parent.gameObject.transform.GetComponent<LarvaController>().ChangeState("Search");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackCol : MonoBehaviour
{
    public MonsterController monsterController;

    public virtual void Awake()
    {
        monsterController = transform.parent.GetComponent<MonsterController>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.ChangeState("DETECT");
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAttackCol : MonsterAttackCol
{
    SpitterController spitterController;

    private void Start()
    {
        spitterController = monsterController.GetComponent<SpitterController>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            spitterController.Stat2.isPlayerInCol = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.transform.CompareTag("Player"))
        {
            spitterController.Stat2.isPlayerInCol = false;
        }
    }
}

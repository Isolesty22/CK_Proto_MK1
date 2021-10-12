using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttackCol : MonsterAttackCol
{
    SpiderController spiderController;

    private void Start()
    {
        spiderController = monsterController.GetComponent<SpiderController>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.inAttackCol = true;
            spiderController.Stat2.isPlayerInCol = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.transform.CompareTag("Player"))
        {
            spiderController.Stat2.isPlayerInCol = false;
        }
    }
}

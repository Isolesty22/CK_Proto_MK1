using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAttackCol : MonsterAttackCol
{
    public FlyerController flyerController;

    public override void Awake()
    {
        base.Awake();
        flyerController = transform.parent.GetComponent<FlyerController>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //StopCoroutine(flyerController.move);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.transform.CompareTag("Player"))
        {
            monsterController.ChangeState("IDLE");
        }
    }
}

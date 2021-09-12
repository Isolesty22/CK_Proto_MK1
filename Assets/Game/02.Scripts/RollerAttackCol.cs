using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerAttackCol : MonsterAttackCol
{
    public RollerController rollerController;

    public override void Awake()
    {
        base.Awake();
        rollerController = transform.parent.GetComponent<RollerController>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(!rollerController.isAttack)
        {
            base.OnTriggerEnter(other);
        }
    }

}

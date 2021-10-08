using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitController : MonsterController
{
    private BearController bearController;
    public override void Awake()
    {
        bearController = GetComponent<BearController>();
    }

    public override void ChangeState(MonsterState stateName)
    {

    }
    public override void Hit(int damage)
    {
        bearController.hp -= damage;
    }

    public override void Initialize()
    {

    }

    public override void Start()
    {

    }

    public override void State(MonsterState state)
    {

    }


    public override void Update()
    {

    }

    protected override void Attack()
    {

    }

    protected override void Death()
    {

    }

    protected override void Detect()
    {

    }

    protected override void HandleAnimation()
    {

    }

    protected override void Idle()
    {

    }

    protected override void Move()
    {

    }

    protected override void Search()
    {

    }
}

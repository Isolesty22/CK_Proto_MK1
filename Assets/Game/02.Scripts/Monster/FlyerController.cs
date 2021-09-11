using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerController : MonsterController
{
    #region
    #endregion

    void Start()
    {

    }

    void Update()
    {
        State(state);
    }

    private void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arrow"))
            ChangeState("HIT");
    }

    public override void State(MonsterState state)
    {
        base.State(state);
    }

    public override void ChangeState(string functionName)
    {
        base.ChangeState(functionName);
    }
    protected override void Idle()
    {
        base.Idle();
        ChangeState("MOVE");
    }

    protected override void Detect()
    {
        base.Detect();

        ChangeState("ATTACK");
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Attack()
    {
        base.Attack();

        int attackType;
        attackType = Random.Range(0, 11);
        if(attackType >= 0 && attackType < 7)
        {
            // Normal Attack

        }
        else
        {
            // Triple Attack

        }


        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.transform.position) > 30)
        {
            ChangeState("DEATH");
        }
    }
    public override void Hit()
    {
        if (Stat.hp <= 1)
            ChangeState("DEATH");

        Stat.hp -= damage;
        if (prevState == MonsterState.IDLE)
            ChangeState("IDLE");
        else if (prevState == MonsterState.DETECT)
            ChangeState("IDLE");
        else if (prevState == MonsterState.ATTACK)
            ChangeState("ATTACK");
        else if (prevState == MonsterState.MOVE)
            ChangeState("IDLE");
    }

    protected override void Death()
    {
        base.Death();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : MonsterController
{
    #region
    public Vector3 moveDirection;
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
        if (state == MonsterState.MOVE)
        {

            Vector3 frontVector = Vector3.zero;

            if(gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
                frontVector = new Vector3(Com.rigidbody.position.x - Stat.move_Speed * 0.5f, Com.rigidbody.position.y, Com.rigidbody.position.z);
            else
                frontVector = new Vector3(Com.rigidbody.position.x + Stat.move_Speed * 0.5f, Com.rigidbody.position.y, Com.rigidbody.position.z);

            if (Physics.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Ground")) == false)
            {
                if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else
                    gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
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
        if (gameObject.transform.position.x > GameManager.instance.playerController.transform.position.x)
            moveDirection = new Vector3(-1, 0, 0);
        else
            moveDirection = new Vector3(1, 0, 0);
        ChangeState("ATTACK");
    }

    protected override void Move()
    {
        base.Move();
        if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
        {
            Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        }
        else
        {
            Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        if (moveDirection.x < 0)
            Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        else
            Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);

        if(Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.transform.position) > 30)
        {
            ChangeState("DEATH");
        }
    }
    public override void Hit()
    {
        if (Stat.hp <= 1)
            ChangeState("DEATH");

        Stat.hp -= damage;
        if(prevState == MonsterState.IDLE)
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

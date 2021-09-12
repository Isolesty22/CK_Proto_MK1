using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonsterController
{
    #region
    public float upDownSpeed;
    public float upDelay;
    public float downRange;
    #endregion

    private Vector3 pos;
    private Vector3 destPos;

    public enum AttackState
    {
        Wait,
        Down,
        Up
    }

    public AttackState attackState = AttackState.Wait;

    void Start()
    {
        pos = gameObject.transform.position;
        destPos = gameObject.transform.position - new Vector3(0, downRange, 0);
    }

    public override void Update()
    {
        base.Update();
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
    }

    protected override void Detect()
    {
        base.Detect();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Attack()
    {
        base.Attack();
        if(gameObject.transform.position.y == pos.y)
        {
            if(isRunninCo == false)
                StartCoroutine(UpDownDelay(0));
        }
        else if(gameObject.transform.position.y == destPos.y)
        {
            if (isRunninCo == false)
                StartCoroutine(UpDownDelay(1));
        }

        switch (attackState)
        {
            case AttackState.Wait:
                break;
            case AttackState.Down:
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destPos, upDownSpeed * 0.1f);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destPos, upDownSpeed * Time.deltaTime);
                isRunninCo = false;
                break;
            case AttackState.Up:
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pos, upDownSpeed * 0.1f);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, upDownSpeed * Time.deltaTime);
                isRunninCo = false;
                break;
            default:
                break;
        }
    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
    }

    private IEnumerator UpDownDelay(int n)
    {
        isRunninCo = true;
        attackState = AttackState.Wait;
        yield return new WaitForSeconds(upDelay);
        if (n == 0)
            attackState = AttackState.Down;
        else
            attackState = AttackState.Up;
    }

}

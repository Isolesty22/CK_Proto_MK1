using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonsterController
{
    #region
    public float nextActDelay;
    public int nextActNum;

    public float jumpPower;
    public int jumpProbability;
    #endregion

    private float timer;
    private int randomJump;

    void Start()
    {
        
    }

    void Update()
    {
        State(state);
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
        if (timer > 0)
        {
            switch (nextActNum)
            {
                case 0:
                    break;
                case 1:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);
                    break;
                default:
                    break;
            }
        }
        else
        {
            nextActNum = Random.Range(0, 3);
            timer = nextActDelay;
        }

        if (isRunninCo == false)
            StartCoroutine(RandomJump());
        timer -= Time.deltaTime;
    }
    public override void Hit()
    {
        base.Hit();
    }

    protected override void Death()
    {
        base.Death();

    }

    private IEnumerator RandomJump()
    {
        isRunninCo = true;
        randomJump = Random.Range(0, jumpProbability);
        if(randomJump == 0)
            Com.rigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        isRunninCo = false;
    }
}

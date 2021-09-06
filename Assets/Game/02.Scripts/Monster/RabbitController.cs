using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonsterController
{
    #region
    public MonsterState state = MonsterState.Search;

    public bool isRunninCo;

    public float nextActDelay;
    public int nextActNum;

    public float jumpPower;
    public float moveSpeed;
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

    public void ChangeState(string functionName)
    {
        if (functionName == "Search")
        {
            state = MonsterState.Search;
        }
        else if (functionName == "Chase")
        {
            state = MonsterState.Chase;
        }
        else if (functionName == "Attack")
        {
            state = MonsterState.Attack;
        }
        else if (functionName == "Dead")
        {
            state = MonsterState.Dead;
        }
    }
    public void State(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Search:
                Search();
                break;

            case MonsterState.Chase:
                Chase();
                break;

            case MonsterState.Attack:
                Attack();
                break;

            case MonsterState.Dead:
                Dead();
                break;

            default:
                break;
        }
    }

    protected override void Search()
    {
        base.Search();
    }

    protected override void Chase()
    {
        base.Chase();
    }

    protected override void Attack()
    {
        base.Attack();
        float gravity = Com.rigidbody.velocity.y;
        if (timer > 0)
        {
            switch (nextActNum)
            {
                case 0:
                    break;
                case 1:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    Com.rigidbody.velocity = new Vector3(-moveSpeed, gravity, 0);
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    Com.rigidbody.velocity = new Vector3(moveSpeed, gravity, 0);
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
    protected override void Dead()
    {
        base.Dead();

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

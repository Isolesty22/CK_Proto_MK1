using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowBugController : MonsterController
{
    public float moveSpeed;
    public float stunTime;

    void Start()
    {
        
    }

    void Update()
    {
        State(state);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Wall"))
        {
            ChangeState("Dead");
        }
    }

    public void Hitted()
    {
        if (Stat.hp > 1)
            Stat.hp--;
        else
            ChangeState("Dead");
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
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
    }
    protected override void Dead()
    {
        if(isRunninCo == false)
            StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        isRunninCo = true;
        yield return new WaitForSeconds(stunTime);
        base.Dead();
        isRunninCo = false;
    }
}

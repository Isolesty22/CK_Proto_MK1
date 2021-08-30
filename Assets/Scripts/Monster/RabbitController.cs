using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonsterController
{
    #region
    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;
    public MonsterState state = MonsterState.Search;

    public bool isRunninCo;

    public float nextActDelay;
    public int nextActNum;

    public float jumpPower;
    public float moveSpeed;
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        State(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            if (Stat.hp > 1)
                Stat.hp--;
            else
                ChangeState("Dead");
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
        if (isRunninCo == false)
            StartCoroutine(SelectNextAct());
    }

    protected override void Chase()
    {
        base.Chase();
    }

    protected override void Attack()
    {
        base.Attack();
        if (isRunninCo == false)
            StartCoroutine(SelectNextAct());
    }
    protected override void Dead()
    {
        base.Dead();
    }

    private IEnumerator SelectNextAct()
    {
        isRunninCo = true;
        nextActNum = Random.Range(0, 4);
        yield return new WaitForSeconds(nextActDelay);
        switch (nextActNum)
        {
            case 0:
                break;
            case 1:
                Com.rigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
                break;
            case 2:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, -90, 0));
                Com.rigidbody.AddForce(new Vector3(-moveSpeed, jumpPower, 0), ForceMode.Impulse);
                break;
            case 3:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, 90, 0));
                Com.rigidbody.AddForce(new Vector3(moveSpeed, jumpPower, 0), ForceMode.Impulse);
                break;
            default:
                break;
        }
        isRunninCo = false;
    }

}

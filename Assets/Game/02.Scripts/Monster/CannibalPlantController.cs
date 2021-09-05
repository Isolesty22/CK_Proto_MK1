using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalPlantController : MonsterController
{
    #region
    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;
    public MonsterState state = MonsterState.Search;

    public bool isRunninCo;
    public float moveSpeed;

    public Mesh changeMesh;
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        State(state);
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
        gameObject.transform.GetComponent<MeshFilter>().sharedMesh = changeMesh;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, GameManager.instance.playerController.gameObject.transform.position, moveSpeed * 0.1f);
    }
    protected override void Dead()
    {
        base.Dead();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShootingPlantController : MonsterController
{
    #region
    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;
    public MonsterState state = MonsterState.Search;

    public List<GameObject> seeds = new List<GameObject>();

    private static int bulletCount;

    public float shootDelay;
    public float seedSpeed;

    public bool isRunninCo;
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
        if (isRunninCo == false)
            StartCoroutine(ShootSeed());
    }
    protected override void Dead()
    {
        base.Dead();
    }
    IEnumerator ShootSeed()
    {
        isRunninCo = true;
        yield return new WaitForSeconds(shootDelay);
        seeds[bulletCount].GetComponent<Bullet>().moveSpeed = seedSpeed;
        seeds[bulletCount].transform.position = gameObject.transform.position;
        seeds[bulletCount].gameObject.SetActive(true);
        if(gameObject.transform.position.x - GameManager.instance.transform.position.x <= 0)
            seeds[bulletCount].GetComponent<Bullet>().moveDir = 1;
        else
            seeds[bulletCount].GetComponent<Bullet>().moveDir = 2;

        if (bulletCount < 2)
            bulletCount++;
        else if (bulletCount == 2)
            bulletCount = 0;
        isRunninCo = false;
    }

}

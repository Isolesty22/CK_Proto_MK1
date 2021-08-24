using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomShootingPlantController : MonsterController
{
    #region
    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;
    public MonsterState state = MonsterState.Search;

    public List<GameObject> venoms = new List<GameObject>();
    private int bulletCount;

    public float shootDelay;

    public bool isRunninCo;
    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        State(state);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
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
        if(isRunninCo == false)
            StartCoroutine(ShootVenom());
    }
    protected override void Dead()
    {
        base.Dead();
    }

    IEnumerator ShootVenom()
    {
        isRunninCo = true;
        yield return new WaitForSeconds(shootDelay);
        venoms[bulletCount].transform.position = gameObject.transform.position;
        venoms[bulletCount].GetComponent<VenomBullet>().target = GameManager.instance.playerController.transform;
        StartCoroutine(venoms[bulletCount].GetComponent<VenomBullet>().ParabolaShoot());
        if (bulletCount < 3)
            bulletCount++;
        else if (bulletCount == 2)
            bulletCount = 0;
        isRunninCo = false;
    }
}

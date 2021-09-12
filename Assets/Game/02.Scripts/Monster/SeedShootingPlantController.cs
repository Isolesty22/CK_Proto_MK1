using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShootingPlantController : MonsterController
{
    #region
    public List<GameObject> seeds = new List<GameObject>();
    private static int bulletCount;

    public float shootDelay;
    public float seedSpeed;
    #endregion
    void Start()
    {
        
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
        if (isRunninCo == false)
            StartCoroutine(ShootSeed());
    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
    }
    IEnumerator ShootSeed()
    {
        isRunninCo = true;
        yield return new WaitForSeconds(shootDelay);
        seeds[bulletCount].GetComponent<Bullet>().moveSpeed = seedSpeed;
        seeds[bulletCount].transform.position = gameObject.transform.position;
        seeds[bulletCount].gameObject.SetActive(true);
        if (gameObject.transform.position.x - GameManager.instance.transform.position.x <= 0)
            seeds[bulletCount].GetComponent<Bullet>().moveDir = 1;
        else
            seeds[bulletCount].GetComponent<Bullet>().moveDir = 2;

        seeds[bulletCount].GetComponent<Bullet>().Move();
        if (bulletCount < 2)
            bulletCount++;
        else if (bulletCount == 2)
            bulletCount = 0;
        isRunninCo = false;
    }

}

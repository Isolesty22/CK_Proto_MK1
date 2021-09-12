using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomShootingPlantController : MonsterController
{
    #region
    public List<GameObject> venoms = new List<GameObject>();
    private int bulletCount;

    public float shootDelay;
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
        if(isRunninCo == false)
            StartCoroutine(ShootVenom());
    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
    }

    IEnumerator ShootVenom()
    {
        isRunninCo = true;
        yield return new WaitForSeconds(shootDelay);
        venoms[bulletCount].transform.position = gameObject.transform.position;
        venoms[bulletCount].gameObject.SetActive(true);
        venoms[bulletCount].GetComponent<CurveBullet>().target = GameManager.instance.playerController.transform.position;
        StartCoroutine(venoms[bulletCount].GetComponent<CurveBullet>().ParabolaShoot());
        if (bulletCount < 2)
            bulletCount++;
        else if (bulletCount == 2)
            bulletCount = 0;
        isRunninCo = false;
    }
}

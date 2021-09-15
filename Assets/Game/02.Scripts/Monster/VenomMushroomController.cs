//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class VenomMushroomController : MonsterController
//{
//    #region
//    public ParticleSystem venomSpore;
//    public float poisonTime;
//    public float sporeAreaActiveTime;
//    public GameObject sporeArea;
//    #endregion
//    void Start()
//    {

//    }

//    public override void Update()
//    {
//        base.Update();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.transform.CompareTag("Arrow"))
//            ChangeState("HIT");
//    }

//    public override void State(MonsterState state)
//    {
//        base.State(state);
//    }

//    public override void ChangeState(string functionName)
//    {
//        base.ChangeState(functionName);
//    }
//    protected override void Idle()
//    {
//        base.Idle();
//    }

//    protected override void Detect()
//    {
//        base.Detect();
//    }

//    protected override void Move()
//    {
//        base.Move();
//    }
//    protected override void Attack()
//    {
//        base.Attack();
//        if (isRunninCo == false)
//        {
//            venomSpore.Play();
//            StartCoroutine(activeSpore());
//        }
//    }
//    public override void Hit(int damage)
//    {
//        base.Hit(damage);
//    }

//    protected override void Death()
//    {
//        base.Death();
//    }

//    private IEnumerator activeSpore()
//    {
//        isRunninCo = true;
//        sporeArea.SetActive(true);
//        yield return new WaitForSeconds(sporeAreaActiveTime);
//        sporeArea.SetActive(false);
//        isRunninCo = false;
//    }

//}

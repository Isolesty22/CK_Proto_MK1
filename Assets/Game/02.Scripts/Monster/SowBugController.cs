//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SowBugController : MonsterController
//{
//    #region
//    public float stunTime;
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
//        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Wall"))
//        {
//            ChangeState("DEATH");
//        }
//    }

//    public override void State(MonsterState state)
//    {
//        base.State(state);
//    }

//    //public override void ChangeState(string functionName)
//    //{
//    //    base.ChangeState(functionName);
//    //}
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
//        transform.position += new Vector3(-Stat.move_Speed * Time.deltaTime, 0, 0);
//    }
//    public override void Hit(int damage)
//    {
//        base.Hit(damage);
//    }

//    protected override void Death()
//    {
//        if(isRunninCo == false)
//            StartCoroutine(Stun());
//    }

//    private IEnumerator Stun()
//    {
//        isRunninCo = true;
//        yield return new WaitForSeconds(stunTime);
//        base.Death();
//        isRunninCo = false;
//    }
//}

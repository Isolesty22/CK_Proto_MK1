//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BirdController : MonsterController
//{
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

//    //protected override void Attack()
//    //{
//    //    base.Attack();
//    //    transform.Translate(Vector3.left * Time.deltaTime * Stat.move_Speed);
//    //}
//    protected override void Death()
//    {
//        base.Death();
//    }

//    public override void Hit(int damage)
//    {
//        base.Hit(damage);
//    }
//}

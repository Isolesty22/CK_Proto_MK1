using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalPlantController : MonsterController
{
    #region
    public Mesh changeMesh;
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
        gameObject.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = changeMesh;
        if(gameObject.transform.position.x > GameManager.instance.playerController.gameObject.transform.position.x)
            transform.position += new Vector3(-Stat.move_Speed * Time.deltaTime, 0, 0);
        else
            transform.position += new Vector3(Stat.move_Speed * Time.deltaTime, 0, 0);

    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
    }


    protected override void Death()
    {
        base.Death();
    }

}

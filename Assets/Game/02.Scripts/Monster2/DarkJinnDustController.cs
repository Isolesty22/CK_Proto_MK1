using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DarkJinnDustController : MonsterController
{
    #region
    [Serializable]
    public class DarkJinnDustStatus
    {
        public float blinkTime;
    }

    [Serializable]
    public class DarkJinnDustComponents
    {

    }

    [SerializeField] private DarkJinnDustStatus darkJinnDustStatus = new DarkJinnDustStatus();
    [SerializeField] private DarkJinnDustComponents darkJinnDustComponents = new DarkJinnDustComponents();

    public DarkJinnDustStatus Stat2 => darkJinnDustStatus;
    public DarkJinnDustComponents Com2 => darkJinnDustComponents;

    private Vector3 shootDir;
    private bool isRunCo;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
        isRunCo = false;
        Com.animator.SetBool("isDeath", false);
        Com.rigidbody.velocity = Vector3.zero;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.transform.position) > 30 && state == MonsterState.ATTACK)
        {
            ChangeState(MonsterState.DEATH);
        }
    }

    public override void State(MonsterState state)
    {
        base.State(state);
    }

    public override void ChangeState(MonsterState state)
    {
        base.ChangeState(state);
    }

    protected override void Idle()
    {
        base.Idle();
        ChangeState(MonsterState.MOVE);
    }

    protected override void Move()
    {
        base.Move();
    }
    protected override void Detect()
    {
        base.Detect();
    }
    protected override void Attack()
    {
        base.Attack();
        if (!isRunCo)
        {
            var shoot = Shoot();
            StartCoroutine(shoot);
        }
            
    }

    IEnumerator Shoot()
    {
        isRunCo = true;
        //blink Particle Play
        yield return new WaitForSeconds(Stat2.blinkTime);
        //Shoot Particle Play
        shootDir = GameManager.instance.playerController.transform.position - gameObject.transform.position;

        Debug.Log(shootDir);

        while (state != MonsterState.DEATH)
        {
            Com.rigidbody.velocity = shootDir * Stat.moveSpeed * Time.deltaTime;
            yield return null;
        }

        isRunCo = false;
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        Com.animator.SetBool("isDeath", true);
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ChangeState(MonsterState.DEATH);
        }
    }
}
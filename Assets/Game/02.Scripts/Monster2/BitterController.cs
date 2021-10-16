using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BitterController : MonsterController
{
    #region
    [Serializable]
    public class BitterStatus
    {
        public float upRange;
        public float upDownSpeed;
    }

    [Serializable]
    public class BitterComponents
    {
    }

    [SerializeField] private BitterStatus bitterStatus = new BitterStatus();
    [SerializeField] private BitterComponents bitterComponents = new BitterComponents();

    public BitterStatus Stat2 => bitterStatus;
    public BitterComponents Com2 => bitterComponents;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
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
        ChangeState(MonsterState.ATTACK);
    }
    protected override void Attack()
    {
        base.Attack();

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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpitterController : MonsterController
{
    #region
    [Serializable]
    public class SpitterStatus
    {

        //[Header("Sub Status")]
    }

    [Serializable]
    public class SpitterComponents
    {
    }

    [SerializeField] private SpitterStatus spitterStatus = new SpitterStatus();
    [SerializeField] private SpitterComponents spitterComponents = new SpitterComponents();

    public SpitterStatus Stat2 => spitterStatus;
    public SpitterComponents Com2 => spitterComponents;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
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
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }
}
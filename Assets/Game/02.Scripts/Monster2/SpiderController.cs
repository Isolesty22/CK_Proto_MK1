using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpiderController : MonsterController
{
    #region
    [Serializable]
    public class SpiderStatus
    {
        public float upDownSpeed;
        public float upDownRange;

        //[Header("Sub Status")]
    }

    [Serializable]
    public class SpiderComponents
    {
        public CapsuleCollider capsuleCollider;
    }

    [SerializeField] private SpiderStatus spiderStatus = new SpiderStatus();
    [SerializeField] private SpiderComponents spiderComponents = new SpiderComponents();

    public SpiderStatus Stat2 => spiderStatus;
    public SpiderComponents Com2 => spiderComponents;

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

    private IEnumerator UpDown()
    {
        yield return null;
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
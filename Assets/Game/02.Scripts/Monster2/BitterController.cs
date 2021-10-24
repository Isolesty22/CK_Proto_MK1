using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class BitterController : MonsterController
{
    #region
    [Serializable]
    public class BitterStatus
    {
        public float upRange;
        public float upDownSpeed;

        public int attackAnimNum;
    }

    [Serializable]
    public class BitterComponents
    {
    }

    [SerializeField] private BitterStatus bitterStatus = new BitterStatus();
    [SerializeField] private BitterComponents bitterComponents = new BitterComponents();

    public BitterStatus Stat2 => bitterStatus;
    public BitterComponents Com2 => bitterComponents;

    public Tween tween;

    #endregion
    public override void Initialize()
    {
        base.Initialize();
        Utility.KillTween(tween);
        Com.animator.SetInteger("attackNum", Stat2.attackAnimNum);
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

        if(gameObject.transform.position.y != Com.spawnPos.y)
        {
            Com.animator.SetBool("isAttack", false);
            Utility.KillTween(tween);
            tween = transform.DOMove(Com.spawnPos, Stat2.upDownSpeed).SetEase(Ease.InCubic);
            tween.Play();
        }
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

        if (gameObject.transform.position.y == Com.spawnPos.y)
        {
            Com.animator.SetBool("isAttack", true);
            Utility.KillTween(tween);
            tween = transform.DOMove(new Vector3(Com.spawnPos.x, Com.spawnPos.y + Stat2.upRange, Com.spawnPos.z), Stat2.upDownSpeed).SetEase(Ease.OutCubic);
            tween.Play();
        }
        else if(gameObject.transform.position.y == Com.spawnPos.y + Stat2.upRange)
        {
            Com.animator.SetBool("isAttack", false);
            Utility.KillTween(tween);
            tween = transform.DOMove(Com.spawnPos, Stat2.upDownSpeed).SetEase(Ease.InCubic);
            tween.Play();
        }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmadiloController : MonsterController
{
    #region
    [Serializable]
    public class ArmadiloStatus
    {
        public float overturnTime;
    }

    [Serializable]
    public class ArmadiloComponents
    {
    }

    enum ArmadiloState
    {
        Normal,
        Defence,
        Overturn
    }

    [SerializeField] private ArmadiloStatus armadiloStatus = new ArmadiloStatus();
    [SerializeField] private ArmadiloComponents armadiloComponents = new ArmadiloComponents();

    public ArmadiloStatus Stat2 => armadiloStatus;
    public ArmadiloComponents Com2 => armadiloComponents;

    [SerializeField]
    private ArmadiloState armaState;
    #endregion
    public override void Initialize()
    {
        ChangeToNormal();
        base.Initialize();
        Com.animator.SetBool("isDefence", false);
        Com.animator.SetBool("isOverturn", false);
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
    }
    protected override void Attack()
    {
        base.Attack();
    }

    public override void Hit(int damage)
    {
        if(damage != 1)
        {
            base.Hit(damage);
            return;
        }

        if (armaState == ArmadiloState.Overturn)
        {
            base.Hit(damage);
        }
        else if(armaState == ArmadiloState.Normal)
        {
            ChangeToDefenceImmedeately();
        }
        else
        {
            var changeSound = ChangeHitSound();
            StartCoroutine(changeSound);
            return;
        }
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

    public void ChangeToOverturn()
    {
        armaState = ArmadiloState.Overturn;
        Com.animator.SetBool("isOverturn", true);
        Com.animator.SetBool("isDefence", false);

        var changeToDefence = ChangeToDefence();
        StartCoroutine(changeToDefence);
    }

    IEnumerator ChangeToDefence()
    {
        yield return new WaitForSeconds(Stat2.overturnTime);
        armaState = ArmadiloState.Defence;
        Com.animator.SetBool("isDefence", true);
        Com.animator.SetBool("isOverturn", false);
    }

    private void ChangeToDefenceImmedeately()
    {
        armaState = ArmadiloState.Defence;
        Com.animator.SetBool("isDefence", true);
        Com.animator.SetBool("isOverturn", false);
    }


    private void ChangeToNormal()
    {
        armaState = ArmadiloState.Normal;
        Com.animator.SetBool("isDefence", false);
        Com.animator.SetBool("isOverturn", false);
    }

    IEnumerator ChangeHitSound()
    {
        AudioClip temp = AudioManager.Instance.clips.arrowHitMon;
        AudioManager.Instance.clips.arrowHitMon = AudioManager.Instance.clips.arrowHitArmadiloDefence;
        yield return null;
        AudioManager.Instance.clips.arrowHitMon = temp;
    }
}
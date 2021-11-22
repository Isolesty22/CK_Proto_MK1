using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class FlyerController : MonsterController
{
    #region
    [Serializable]
    public class FlyerStatus
    {
        public float patrolRange;
        public float patrolTime = 2f;
        public float nextPatrolDelay;
        public bool isPatrol;
        [Header("Sub Status")]
        [HideInInspector] public Vector3 patrolPos1;
        [HideInInspector] public Vector3 patrolPos2;
    }

    [Serializable]
    public class FlyerComponents
    {


    }

    [SerializeField] private FlyerStatus flyerStatus = new FlyerStatus();
    [SerializeField] private FlyerComponents flyerComponents = new FlyerComponents();
    public FlyerStatus Stat2 => flyerStatus;
    public FlyerComponents Com2 => flyerComponents;

    public Tween tween;
    private bool isRunCo;
    #endregion

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Utility.KillTween(tween);
        Com.rigidbody.useGravity = false;
        Com.rigidbody.velocity = Vector3.zero;
        Com.animator.SetBool("isAttack", false);
        Com.animator.SetBool("isMove", false);
        Com.animator.SetBool("isDead", false);
        Stat2.patrolPos1 = Com.spawnPos;
        Stat2.patrolPos2 = Com.spawnPos + Vector3.left * Stat2.patrolRange;
        isRunCo = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void State(MonsterState state)
    {
        base.State(state);
    }

    public override void ChangeState(MonsterState functionName)
    {
        base.ChangeState(functionName);
    }

    protected override void Idle()
    {
        base.Idle();
        Com.audio.Stop();
        ChangeState(MonsterState.MOVE);
    }

    protected override void Move()
    {
        base.Move();
        Com.animator.SetBool("isMove", true);

        if (!Com.audio.isPlaying && Time.timeScale != 0)
        {
            Com.audio.loop = true;
            Com.audio.Play();
        }

        if (Stat2.isPatrol)
        {
            if (!isRunCo)
            {
                var patrolMove = PatrolMove();
                StartCoroutine(patrolMove);
            }

        }

        else
        {
            Com.rigidbody.velocity = new Vector3(-Stat.moveSpeed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = Vector3.zero;
        }
    }

    IEnumerator PatrolMove()
    {
        isRunCo = true;
        if (transform.eulerAngles == Vector3.zero)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(Stat2.patrolPos2, Stat2.patrolTime).SetEase(Ease.InOutCubic);
            tween.Play();
        }
        else
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(Stat2.patrolPos1, Stat2.patrolTime).SetEase(Ease.InOutCubic);
            tween.Play();
        }

        yield return new WaitForSeconds(Stat2.patrolTime + Stat2.nextPatrolDelay);
        if (transform.eulerAngles == Vector3.zero)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        isRunCo = false;
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
        Utility.KillTween(tween);
        Com.animator.SetBool("isDead", true);
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }
}

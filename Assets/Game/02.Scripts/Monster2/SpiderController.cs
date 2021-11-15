using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SpiderController : MonsterController
{
    #region
    [Serializable]
    public class SpiderStatus
    {
        public float downTime;
        public float upTime;
        public float upDownRange;

        [Header("Sub Status")]
        public bool isPlayerInCol;
        [HideInInspector] public Vector3 upPos; 
        [HideInInspector] public Vector3 downPos;
    }

    [Serializable]
    public class SpiderComponents
    {
        public GameObject spiderWeb;
    }

    [SerializeField] private SpiderStatus spiderStatus = new SpiderStatus();
    [SerializeField] private SpiderComponents spiderComponents = new SpiderComponents();

    public SpiderStatus Stat2 => spiderStatus;
    public SpiderComponents Com2 => spiderComponents;

    public Tween tween;
    private bool moveTrigger;
    private Vector3 originColSize;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
        Com.animator.SetBool("isAttack", false);
        Com.animator.SetBool("isDeath", false);
        Com2.spiderWeb.SetActive(true);
        Com.collider.GetComponent<BoxCollider>().size = originColSize;
        Com.rigidbody.useGravity = false;
        Utility.KillTween(tween);
        Stat2.isPlayerInCol = false;
        Stat2.upPos = transform.position;
        Stat2.downPos = transform.position + Vector3.down * Stat2.upDownRange;
        moveTrigger = true;
    }

    public override void Awake()
    {
        originColSize = Com.collider.GetComponent<BoxCollider>().size;
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

        if(transform.position == Stat2.upPos)
        {
            if (Stat2.isPlayerInCol)
            {
                moveTrigger = true;
                Com.animator.SetBool("isAttack", true);
                if (!Com.audio.isPlaying)
                {
                    Com.audio.Play();
                }
                ChangeState(MonsterState.ATTACK);
            }
        }

        else if(transform.position == Stat2.downPos)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(Stat2.upPos, Stat2.upTime).SetEase(Ease.InOutCubic);
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
        if (moveTrigger)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(Stat2.downPos, Stat2.downTime).SetEase(Ease.InOutCubic);
            tween.Play();
            moveTrigger = false;
        }

        if(transform.position == Stat2.downPos)
        {
            Com.animator.SetBool("isAttack", false);
            ChangeState(MonsterState.IDLE);
        }
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        Utility.KillTween(tween);

        if (Com.collider.GetComponent<BoxCollider>().size == originColSize)
            Com.collider.GetComponent<BoxCollider>().size = new Vector3(Com.collider.GetComponent<BoxCollider>().size.y, Com.collider.GetComponent<BoxCollider>().size.z, Com.collider.GetComponent<BoxCollider>().size.x);

        Com.animator.SetBool("isDeath", true);
        Com2.spiderWeb.SetActive(false);
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }
}
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
        public float shootDelay;
        [Header("Sub Status")]
        public bool isPlayerInCol;
        public float venomHeight;
        public float venomSpeed;
    }

    [Serializable]
    public class SpitterComponents
    {
    }

    [SerializeField] private SpitterStatus spitterStatus = new SpitterStatus();
    [SerializeField] private SpitterComponents spitterComponents = new SpitterComponents();

    public SpitterStatus Stat2 => spitterStatus;
    public SpitterComponents Com2 => spitterComponents;

    private bool isRunCo;

    #endregion
    public override void Initialize()
    {
        Com.animator.SetBool("isDeath", false);
        Com.rigidbody.useGravity = false;
        isRunCo = false;
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
        if (Stat2.isPlayerInCol)
        {
            //if (GameManager.instance.playerController.transform.position.x <= transform.position.x) 
            //{
            //    transform.localEulerAngles = Vector3.zero;
            //}

            //else
            //{
            //    transform.localEulerAngles = new Vector3(0, 180, 0);
            //}

            ChangeState(MonsterState.ATTACK);
        }

        else
            return;
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
        var shoot = Shoot();

        if (transform.localEulerAngles == Vector3.zero) // look left
        {
            if (GameManager.instance.playerController.transform.position.x <= transform.position.x) // player left
            {
                if (isRunCo == false)
                    StartCoroutine(shoot);
            }

            else
            {
                if(state != MonsterState.DEATH)
                    ChangeState(MonsterState.IDLE);
            }
        }
        else
        {
            if (GameManager.instance.playerController.transform.position.x > transform.position.x) // player right
            {
                if (isRunCo == false)
                    StartCoroutine(shoot);
            }

            else
            {
                if (state != MonsterState.DEATH)
                    ChangeState(MonsterState.IDLE);
            }
        }

    }

    private IEnumerator Shoot()
    {
        Com.animator.SetTrigger("isAttack");
        Com.audio.PlayOneShot(Com.audio.clip);
        isRunCo = true;
        yield return new WaitForSeconds(0.8f);
        var venom = CustomPoolManager.Instance.curveBulletPool.SpawnThis(transform.position, Vector3.zero, null);
        venom.startPos = transform.position;
        venom.endPos = GameManager.instance.playerController.transform.position + new Vector3(0,-1,0);
        venom.duration = 10 - Stat2.venomSpeed;
        venom.height = Stat2.venomHeight;
        venom.Initialize();
        venom.isRun = true;
        yield return new WaitForSeconds(Stat2.shootDelay);
        if (state != MonsterState.DEATH)
            ChangeState(MonsterState.IDLE);
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
}
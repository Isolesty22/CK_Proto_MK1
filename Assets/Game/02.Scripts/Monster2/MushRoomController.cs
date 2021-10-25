using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MushRoomController : MonsterController
{
    #region
    [Serializable]
    public class MushRoomStatus
    {

    }

    [Serializable]
    public class MushRoomComponents
    {
    }

    [SerializeField] private MushRoomStatus mushRoomStatus = new MushRoomStatus();
    [SerializeField] private MushRoomComponents mushRoomComponents = new MushRoomComponents();

    public MushRoomStatus Stat2 => mushRoomStatus;
    public MushRoomComponents Com2 => mushRoomComponents;

    private Vector3 firstLookDir;
    private Vector3 moveDir;
    private Vector3 layDir;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
        Com.monsterModel.SetActive(true);
        Com.animator.SetBool("isDeath", false);
        Com.rigidbody.velocity = Vector3.zero;
        transform.localEulerAngles = firstLookDir;
    }

    public override void Awake()
    {
        firstLookDir = transform.localEulerAngles;
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
        if(GameManager.instance.playerController.transform.position.x < gameObject.transform.position.x)
        {
            moveDir = Vector3.left;
        }
        else
        {
            moveDir = Vector3.right;
        }
        ChangeState(MonsterState.MOVE);
    }

    protected override void Move()
    {
        base.Move();

        if(moveDir == Vector3.left)
        {
            Com.animator.SetBool("isMove", true);
            Com.rigidbody.velocity = new Vector3(-Stat.moveSpeed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = Vector3.zero;
            layDir = Vector3.left;
        }

        else
        {
            Com.animator.SetBool("isMove", true);
            Com.rigidbody.velocity = new Vector3(Stat.moveSpeed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = new Vector3(0, 180, 0);
            layDir = Vector3.right;
        }

        if (Physics.Raycast(transform.position, layDir, Com.collider.GetComponent<CapsuleCollider>().radius + 0.1f, LayerMask.GetMask("Ground")))
        {
            if (moveDir.x < 0)
            {
                moveDir = new Vector3(1, 0, 0);
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                moveDir = new Vector3(-1, 0, 0);
                transform.localEulerAngles = Vector3.zero;
            }
        }

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
        Com.animator.SetBool("isDeath", true);
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }
}
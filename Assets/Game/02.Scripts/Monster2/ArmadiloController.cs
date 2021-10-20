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
        public GameObject normalModel;
        public GameObject defenceModel;
        public GameObject overturnModel;
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
        if(GameManager.instance.playerController.transform.position.x < transform.position.x)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
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
        Com2.normalModel.SetActive(false);
        Com2.defenceModel.SetActive(false);
        Com2.overturnModel.SetActive(true);
        Com.monsterModel = Com2.overturnModel;
        Com.renderer = Com2.overturnModel.GetComponent<Renderer>();

        var changeToDefence = ChangeToDefence();
        StartCoroutine(changeToDefence);
    }

    IEnumerator ChangeToDefence()
    {
        yield return new WaitForSeconds(Stat2.overturnTime);
        armaState = ArmadiloState.Defence;
        Com2.overturnModel.SetActive(false);
        Com2.normalModel.SetActive(false);
        Com2.defenceModel.SetActive(true);
        Com.monsterModel = Com2.defenceModel;
        Com.renderer = Com2.defenceModel.GetComponent<Renderer>();
    }

    private void ChangeToDefenceImmedeately()
    {
        armaState = ArmadiloState.Defence;
        Com2.overturnModel.SetActive(false);
        Com2.normalModel.SetActive(false);
        Com2.defenceModel.SetActive(true);
        Com.monsterModel = Com2.defenceModel;
        Com.renderer = Com2.defenceModel.GetComponent<Renderer>();
    }


    private void ChangeToNormal()
    {
        armaState = ArmadiloState.Normal;
        Com2.overturnModel.SetActive(false);
        Com2.defenceModel.SetActive(false);
        Com2.normalModel.SetActive(true);
        Com.monsterModel = Com2.normalModel;
        Com.renderer = Com2.normalModel.GetComponent<Renderer>();
    }
}
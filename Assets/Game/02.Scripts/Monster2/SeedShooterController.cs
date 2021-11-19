using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedShooterController : MonsterController
{
    #region
    [Serializable]
    public class SeedShooterStatus
    {
        public Transform firePos;
        public float fireSpeed;
        public float fireRange;
        public float fireCoolTime;

        //public float tripleFireDelay;
        //[Header("Sub Status")]
        [HideInInspector] public bool isAttack;
        [HideInInspector] public Vector3 fireDir;
    }

    [Serializable]
    public class SeedShooterComponents
    {


    }

    [SerializeField] private SeedShooterStatus seedShooterStatus = new SeedShooterStatus();
    [SerializeField] private SeedShooterComponents seedShooterComponents = new SeedShooterComponents();
    public SeedShooterStatus Stat2 => seedShooterStatus;
    public SeedShooterComponents Com2 => seedShooterComponents;

    private float cooltime;

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
        Com.animator.SetBool("isDeath", false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cooltime = 10f;
        Stat2.fireDir = Vector3.left;
    }

    public override void Update()
    {
        base.Update();

        //rotate
        //if (transform.position.x > GameManager.instance.playerController.transform.position.x)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, 0);
        //    Stat2.fireDir = Vector3.left;
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //    Stat2.fireDir = Vector3.right;
        //}
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
    }

    protected override void Detect()
    {
        ChangeState(MonsterState.ATTACK);
    }

    protected override void Attack()
    {
        base.Attack();

        //attack
        cooltime += Time.deltaTime;

        if (cooltime > Stat2.fireCoolTime)
        {
            Com.animator.SetTrigger("isAttack");
            Com.audio.PlayOneShot(Com.audio.clip);
            var seed = CustomPoolManager.Instance.seedPool.SpawnThis(Stat2.firePos.position, new Vector3(0, 0, 0), null);
            seed.firePos = Stat2.firePos.position;
            seed.fireDir = Stat2.fireDir;
            seed.range = Stat2.fireRange;
            seed.fireSpeed = Stat2.fireSpeed;

            cooltime = 0f;
        }

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
}

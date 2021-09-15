using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RollerController : MonsterController
{
    #region
    [Serializable]
    public class RollerStatus
    {
        public float jumpPower;
        public float aclrt; // °¡¼Óµµ
        public float maxSpeed;

        [Header("Sub Status")]
        public float moveChangeTime;
        public float changeDelay;
    }

    [Serializable]
    public class RollerComponents
    {
        //instance
        public GameObject rollingModel;
        public ParticleSystem particle;

        public ConstantForce constantForce;
        public SphereCollider sphereCollider;
        public CapsuleCollider capsuleCollider;
    }

    [SerializeField] private RollerStatus rollerStatus = new RollerStatus();
    [SerializeField] private RollerComponents rollerComponents = new RollerComponents();

    public RollerStatus Stat2 => rollerStatus;
    public RollerComponents Com2 => rollerComponents;

    private IEnumerator modeChange;
    private float movePatternTime;
    private int random;
    private Vector3 moveDir;
    private float currentSpeed;

    #endregion
    public override void Initialize()
    {
        base.Initialize();

        movePatternTime = 10f;
        Com2.sphereCollider.enabled = false;
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

        movePatternTime += Time.deltaTime;



        if(movePatternTime > Stat2.moveChangeTime)
        {
            random = UnityEngine.Random.Range(0, 3);
            movePatternTime = 0f;
        }

        if (random == 0f)
        {
            return;
        }
        else if (random == 1)
        {
            Com.rigidbody.velocity = new Vector3(-Stat.moveSpeed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = Vector3.zero;
        }
        else if (random == 2)
        {
            Com.rigidbody.velocity = new Vector3(Stat.moveSpeed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        if (!Physics.Raycast(transform.position - Vector3.left * Com2.capsuleCollider.height / 2, Vector3.down, 1, LayerMask.GetMask("Ground")))
        {
            if(random == 1)
            {
                random = 2;
                movePatternTime = 0f;
            }
            else if(random == 2)
            {
                random = 1;
                movePatternTime = 0f;
            }
        }
    }

    protected override void Detect()
    {
        base.Detect();

        Com.rigidbody.velocity = Vector3.zero;

        var changeMode = ChangeMode();
        StartCoroutine(changeMode);

        ChangeState(MonsterState.ATTACK);
    }

    IEnumerator ChangeMode()
    {
        moveDir = new Vector3();

        if (transform.position.x > GameManager.instance.playerController.transform.position.x)
        {
            moveDir = new Vector3(-1, 0, 0);
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            moveDir = new Vector3(1, 0, 0);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        Com.rigidbody.AddForce(Vector3.up * Stat2.jumpPower, ForceMode.Impulse);

        yield return new WaitForSeconds(Stat2.changeDelay);

        //change mode

        //instance
        Com2.particle.Play();
        Com.monsterModel.SetActive(false);
        Com2.rollingModel.SetActive(true);
        Com.monsterModel = Com2.rollingModel;

        Com.collider.enabled = false;
        Com2.sphereCollider.enabled = true;
    }

    protected override void Attack()
    {
        base.Attack();

        currentSpeed = Mathf.Clamp(currentSpeed += Stat2.aclrt * Time.deltaTime, 0f, Stat2.maxSpeed);

        var layDir = new Vector3();

        if (moveDir.x < 0)
        {
            Com.rigidbody.velocity = new Vector3(-currentSpeed, Com.rigidbody.velocity.y, 0);
            layDir = Vector3.left;
        }
        else
        {
            Com.rigidbody.velocity = new Vector3(currentSpeed, Com.rigidbody.velocity.y, 0);
            layDir = Vector3.right;
        }



        if (Physics.Raycast(transform.position, layDir, Com2.sphereCollider.radius + 0.1f, LayerMask.GetMask("Ground")))
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

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
        Com2.sphereCollider.enabled = false;
        Com2.constantForce.enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : MonsterController
{
    #region
    public Vector3 moveDirection;
    public GameObject rollingModel;
    public ParticleSystem particle;
    public Collider sphereCollider;


    public float cooltime;
    public int random;


    public float currentSpeed;
    public float maxSpeed;
    public float aclrt; // °¡¼Óµµ
    public float jumpPower;
    #endregion

    public override void Awake()
    {
        base.Awake();
        cooltime = 10f;
        Com.collider.enabled = true;
        sphereCollider.enabled = false;
        currentSpeed = 0;
    }

    public override void Update()
    {
        base.Update();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arrow"))
            ChangeState("HIT");
    }

    public override void State(MonsterState state)
    {
        base.State(state);
    }

    public override void ChangeState(string functionName)
    {
        base.ChangeState(functionName);
    }

    protected override void Idle()
    {
        base.Idle();
        ChangeState("MOVE");
    }

    protected override void Detect()
    {
        base.Detect();

        Com.rigidbody.velocity = Vector3.zero;

        if (gameObject.transform.position.x > GameManager.instance.playerController.transform.position.x)
            moveDirection = new Vector3(-1, 0, 0);
        else
            moveDirection = new Vector3(1, 0, 0);

        if (moveDirection.x < 0)
            transform.localEulerAngles = Vector3.zero;
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);


        var changeMode = ChangeMode();
        StartCoroutine(changeMode);

        //if(isRunninCo == false)
        //    StartCoroutine(ChangeMode());
    }

    protected override void Move()
    {
        base.Move();

        cooltime += Time.deltaTime;

        if(cooltime > 1f)
        {
            random = Random.Range(0, 3);
            cooltime = 0f;
        }

        if (random == 0f)
        {
            return;
        }
        else if (random == 1)
        {
            Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = Vector3.zero;
        }
        else if (random == 2)
        {
            Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        //if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
        //{
        //    Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        //}
        //else
        //{
        //    Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        //}
    }

    protected override void Attack()
    {
        base.Attack();

        //isRunninCo = false;
        currentSpeed = Mathf.Clamp(currentSpeed += aclrt * Time.deltaTime, 0f, maxSpeed);

        if (moveDirection.x < 0)
            Com.rigidbody.velocity = new Vector3(-currentSpeed, Com.rigidbody.velocity.y, 0);
        else
            Com.rigidbody.velocity = new Vector3(currentSpeed, Com.rigidbody.velocity.y, 0);

    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        sphereCollider.enabled = false;
        base.Death();
    }

    IEnumerator ChangeMode()
    {
        //isRunninCo = true;
        Com.rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        particle.Play();


        yield return new WaitForSeconds(0.3f);
        Com.monsterModel.SetActive(false);
        rollingModel.SetActive(true);
        Com.monsterModel = rollingModel;

        Com.collider.enabled = false;
        sphereCollider.enabled = true;

        ChangeState("ATTACK");
    }
}
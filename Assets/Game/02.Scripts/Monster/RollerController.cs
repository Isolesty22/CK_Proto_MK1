using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : MonsterController
{
    #region
    public Vector3 moveDirection;
    public GameObject rollingModel;
    public ParticleSystem particle;

    public float currentSpeed;
    public float maxSpeed;
    public float aclrt; // °¡¼Óµµ
    public float jumpPower;
    #endregion

    void Start()
    {

    }

    void Update()
    {
        if(active == true)
            State(state);
    }

    private void FixedUpdate()
    {
        if (state == MonsterState.MOVE)
        {

            Vector3 frontVector = Vector3.zero;

            if(gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
                frontVector = new Vector3(Com.rigidbody.position.x - Stat.move_Speed * 0.5f, Com.rigidbody.position.y, Com.rigidbody.position.z);
            else
                frontVector = new Vector3(Com.rigidbody.position.x + Stat.move_Speed * 0.5f, Com.rigidbody.position.y, Com.rigidbody.position.z);

            if (Physics.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Ground")) == false)
            {
                if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else
                    gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
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
        if (gameObject.transform.position.x > GameManager.instance.playerController.transform.position.x)
            moveDirection = new Vector3(-1, 0, 0);
        else
            moveDirection = new Vector3(1, 0, 0);

        currentSpeed = 0;
        if(isRunninCo == false)
            StartCoroutine(ChangeModel());
    }

    protected override void Move()
    {
        base.Move();
        if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
        {
            Com.rigidbody.velocity = new Vector3(-Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        }
        else
        {
            Com.rigidbody.velocity = new Vector3(Stat.move_Speed, Com.rigidbody.velocity.y, 0);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        isRunninCo = false;
        currentSpeed = Mathf.Clamp(currentSpeed += aclrt * Time.deltaTime, 0f, maxSpeed);

        if (moveDirection.x < 0)
            Com.rigidbody.velocity = new Vector3(-currentSpeed, Com.rigidbody.velocity.y, 0);
        else
            Com.rigidbody.velocity = new Vector3(currentSpeed, Com.rigidbody.velocity.y, 0);

    }

    public override void Hit()
    {
        base.Hit();
    }

    protected override void Death()
    {
        base.Death();
    }

    IEnumerator ChangeModel()
    {
        isRunninCo = true;
        Com.rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        particle.Play();
        yield return new WaitForSeconds(0.3f);
        Com.monsterModel.SetActive(false);
        rollingModel.SetActive(true);
        Com.monsterModel = rollingModel;
        ChangeState("ATTACK");
    }
}
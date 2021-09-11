using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerController : MonsterController
{
    #region
    public List<GameObject> curveBullets = new List<GameObject>();
    public int shootingCount;
    public float shootDelay;
    public Transform currentPlayerPos;

    public float currentSpeed;
    public float maxSpeed;
    public float aclrt; // °¡¼Óµµ

    public float glidingHeight;
    RaycastHit hit;
    #endregion

    void Start()
    {

    }

    void Update()
    {
        State(state);
        Gliding();
    }

    private void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arrow"))
            ChangeState("HIT");
    }

    private void Gliding()
    {
        Debug.DrawRay(transform.position, Vector3.down * glidingHeight, Color.red);
        if(Physics.Raycast(gameObject.transform.position, Vector3.down , out hit, glidingHeight, LayerMask.GetMask("Ground")))
        {
            gameObject.transform.position += new Vector3(0, Stat.move_Speed * Time.deltaTime, 0);
        }

        if(Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, glidingHeight + 0.5f, LayerMask.GetMask("Ground")) == false)
        {
            gameObject.transform.position -= new Vector3(0, Stat.move_Speed * Time.deltaTime, 0);
        }
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

        ChangeState("ATTACK");
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

        if (gameObject.transform.position.x > GameManager.instance.playerController.transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.transform.position) > 5f)
        {
            currentSpeed = Mathf.Clamp(currentSpeed += aclrt * Time.deltaTime, 0f, maxSpeed);

            if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
                Com.rigidbody.velocity = new Vector3(-currentSpeed, 0, 0);
            else
                Com.rigidbody.velocity = new Vector3(currentSpeed, 0, 0);
        }
        else
        {
            Com.rigidbody.velocity = Vector3.Lerp(Com.rigidbody.velocity, Vector3.zero, Time.deltaTime * 2f);

            if (isRunninCo == false)
            {
                currentPlayerPos = GameManager.instance.playerController.transform;
                int attackType;
                attackType = Random.Range(0, 11);
                if (attackType >= 0 && attackType < 7)
                {
                    // Normal Attack
                    StartCoroutine(NormalShot());
                }
                else
                {
                    // Triple Attack
                    StartCoroutine(TripleShot());
                }
            }
        }
    }
    public override void Hit()
    {
        base.Hit();
    }

    protected override void Death()
    {
        base.Death();
    }

    IEnumerator NormalShot()
    {
        isRunninCo = true;
        curveBullets[shootingCount].gameObject.transform.position = gameObject.transform.position;
        curveBullets[shootingCount].gameObject.SetActive(true);
        curveBullets[shootingCount].GetComponent<CurveBullet>().target = currentPlayerPos.position;
        StartCoroutine(curveBullets[shootingCount].GetComponent<CurveBullet>().ParabolaShoot());
        yield return new WaitForSeconds(shootDelay);
        curveBullets[shootingCount].gameObject.SetActive(false);
        if (shootingCount == curveBullets.Count - 1)
            shootingCount = 0;
        else
            shootingCount++;
        isRunninCo = false;
    }
    IEnumerator TripleShot()
    {
        curveBullets[0].gameObject.SetActive(false);
        curveBullets[1].gameObject.SetActive(false);
        curveBullets[2].gameObject.SetActive(false);
        isRunninCo = true;
        curveBullets[0].gameObject.transform.position = gameObject.transform.position;
        curveBullets[0].gameObject.SetActive(true);
        curveBullets[0].GetComponent<CurveBullet>().target = currentPlayerPos.position;
        curveBullets[1].gameObject.transform.position = gameObject.transform.position;
        curveBullets[1].gameObject.SetActive(true);
        curveBullets[1].GetComponent<CurveBullet>().target = currentPlayerPos.position + new Vector3(-1,0,0);
        curveBullets[2].gameObject.transform.position = gameObject.transform.position;
        curveBullets[2].gameObject.SetActive(true);
        curveBullets[2].GetComponent<CurveBullet>().target = currentPlayerPos.position + new Vector3(1,0,0);
        StartCoroutine(curveBullets[shootingCount].GetComponent<CurveBullet>().ParabolaShoot());
        StartCoroutine(curveBullets[shootingCount+1].GetComponent<CurveBullet>().ParabolaShoot());
        StartCoroutine(curveBullets[shootingCount+2].GetComponent<CurveBullet>().ParabolaShoot());
        yield return new WaitForSeconds(shootDelay);
        curveBullets[0].gameObject.SetActive(false);
        curveBullets[1].gameObject.SetActive(false);
        curveBullets[2].gameObject.SetActive(false);
        shootingCount = 0;
        isRunninCo = false;
    }

}

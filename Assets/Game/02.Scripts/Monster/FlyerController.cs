using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyerController : MonsterController
{
    #region
    public Transform feather;
    public List<GameObject> featherList = new List<GameObject>();
    public int shootingCount;
    public float shootDelay;
    public Transform currentPlayerPos;

    public Vector3 patrolPos1;
    public Vector3 patrolPos2;
    public float patrolRange;
    public float patrolTime = 2f;

    private Tween  tween;
    bool trigger = true;

    public float attackDelay;
    private float attackCooltime;

    public float shotSpeed;
    public float featherRange;

    /////
    public float currentSpeed;
    public float maxSpeed;
    public float aclrt; // °¡¼Óµµ

    public float glidingHeight;
    RaycastHit hit;
    #endregion

    public override void Awake()
    {
        base.Awake();
        patrolPos1 = transform.position;
        patrolPos2 = transform.position + Vector3.left * patrolRange;
        trigger = true;
        attackCooltime = 10f;

        for(int i =0; i< feather.childCount; i++)
        {
            featherList.Add(feather.GetChild(i).gameObject);
        }


        shootingCount = 0;
    }

    void Start()
    {

    }

    public override void Update()
    {
        base.Update();
        Gliding();
        attackCooltime += Time.deltaTime;
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

        if (transform.position.x == patrolPos2.x)
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(patrolPos1, patrolTime).SetEase(Ease.InOutCubic);
            tween.Play();
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.position.x == patrolPos1.x) 
        {
            Utility.KillTween(tween);
            tween = transform.DOMove(patrolPos2, patrolTime).SetEase(Ease.InOutCubic);
            tween.Play();
            transform.eulerAngles = Vector3.zero;
        }
    }

    protected override void Attack()
    {
        base.Attack();

        //rotate
        if (transform.position.x > GameManager.instance.playerController.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        //ready to attack
        if(trigger)
        {
            Utility.KillTween(tween);
            var targetDir = transform.position - GameManager.instance.playerController.transform.position;
            targetDir.Normalize();
            var targetPos = transform.position - new Vector3(targetDir.x, 0, 0) * 0.5f;
            tween = transform.DOMove(targetPos, 0.5f).SetEase(Ease.Unset);
            trigger = false;
        }

        //attack
        if (attackCooltime > attackDelay)
        {
            int attackType;
            attackType = Random.Range(0, 10);

            var normalShot = NormalShot();
            StartCoroutine(normalShot);

            //shootingCount++;

            if (attackType >= 0 && attackType < 7)
            {
                // Normal Attack
                //var normalShot = NormalShot();
                //StartCoroutine(normalShot);
            }
            else
            {
                // Triple Attack
                //var tripleShot = TripleShot();
                //StartCoroutine(tripleShot);
            }

            attackCooltime = 0f;
        }
    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        base.Death();
    }

    public IEnumerator NormalShot()
    {
        var shotDir = GameManager.instance.playerController.transform.position - transform.position;
        shotDir.Normalize();

        if (shootingCount > feather.childCount)
        {
            Debug.Log("normalShot");
            shootingCount = 0;
        }

        featherList[shootingCount].SetActive(true);

        Vector3 curPosition = transform.position;

        while ((curPosition - featherList[shootingCount].transform.position).sqrMagnitude < featherRange)
        {
            featherList[shootingCount].transform.Translate(shotDir * shotSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        featherList[shootingCount].SetActive(false);
        featherList[shootingCount].transform.localPosition = Vector3.zero;
        



        //curveBullets[shootingCount].gameObject.transform.position = gameObject.transform.position;
        //curveBullets[shootingCount].gameObject.SetActive(true);
        //curveBullets[shootingCount].GetComponent<CurveBullet>().target = currentPlayerPos.position;
        //StartCoroutine(curveBullets[shootingCount].GetComponent<CurveBullet>().ParabolaShoot());
        //yield return new WaitForSeconds(shootDelay);
        //curveBullets[shootingCount].gameObject.SetActive(false);
        //if (shootingCount == curveBullets.Count - 1)
        //    shootingCount = 0;
        //else
        //    shootingCount++;
        //isRunninCo = false;
    }

    //public IEnumerator TripleShot()
    //{
    //    //curveBullets[0].gameObject.SetActive(false);
    //    //curveBullets[1].gameObject.SetActive(false);
    //    //curveBullets[2].gameObject.SetActive(false);
    //    //isRunninCo = true;
    //    //curveBullets[0].gameObject.transform.position = gameObject.transform.position;
    //    //curveBullets[0].gameObject.SetActive(true);
    //    //curveBullets[0].GetComponent<CurveBullet>().target = currentPlayerPos.position;
    //    //curveBullets[1].gameObject.transform.position = gameObject.transform.position;
    //    //curveBullets[1].gameObject.SetActive(true);
    //    //curveBullets[1].GetComponent<CurveBullet>().target = currentPlayerPos.position + new Vector3(-1,0,0);
    //    //curveBullets[2].gameObject.transform.position = gameObject.transform.position;
    //    //curveBullets[2].gameObject.SetActive(true);
    //    //curveBullets[2].GetComponent<CurveBullet>().target = currentPlayerPos.position + new Vector3(1,0,0);
    //    //StartCoroutine(curveBullets[shootingCount].GetComponent<CurveBullet>().ParabolaShoot());
    //    //StartCoroutine(curveBullets[shootingCount+1].GetComponent<CurveBullet>().ParabolaShoot());
    //    //StartCoroutine(curveBullets[shootingCount+2].GetComponent<CurveBullet>().ParabolaShoot());
    //    //yield return new WaitForSeconds(shootDelay);
    //    //curveBullets[0].gameObject.SetActive(false);
    //    //curveBullets[1].gameObject.SetActive(false);
    //    //curveBullets[2].gameObject.SetActive(false);
    //    //shootingCount = 0;
    //    //isRunninCo = false;
    //}

}

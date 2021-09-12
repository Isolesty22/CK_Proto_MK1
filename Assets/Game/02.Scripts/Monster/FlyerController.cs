using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyerController : MonsterController
{
    #region
    public Transform feather;
    public List<GameObject> featherList = new List<GameObject>();

    public Vector3 patrolPos1;
    public Vector3 patrolPos2;
    public float patrolRange;
    public float patrolTime = 2f;

    public Tween  tween;
    bool trigger = true;
    bool moveTrigger = true;
    public bool isAttack;

    public float shotDelay;
    public float tripleShotDelay;
    private float attackCooltime;

    public float shotSpeed;
    public float featherRange;

    public float moveSpeed;
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
    }

    void Start()
    {

    }

    public override void Update()
    {
        base.Update();
        attackCooltime += Time.deltaTime;
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

        trigger = true;
        moveTrigger = true;

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

        if (isAttack)
            return;

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
        else
        {
            if(moveTrigger)
            {
                var pos1 = Mathf.Abs(transform.position.x - patrolPos1.x);
                var pos2 = Mathf.Abs(transform.position.x - patrolPos2.x);
                if (pos1 > pos2)
                {
                    Utility.KillTween(tween);
                    tween = transform.DOMove(patrolPos1, patrolTime).SetEase(Ease.InOutCubic);
                    tween.Play();
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (pos2 > pos1)
                {
                    Utility.KillTween(tween);
                    tween = transform.DOMove(patrolPos2, patrolTime).SetEase(Ease.InOutCubic);
                    tween.Play();
                    transform.eulerAngles = Vector3.zero;
                }

                moveTrigger = false;
            }
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
        if (trigger)
        {
            Utility.KillTween(tween);
            var targetDir = transform.position - GameManager.instance.playerController.transform.position;
            targetDir.Normalize();
            var targetPos = transform.position - new Vector3(targetDir.x, 0, 0) * 0.5f;
            tween = transform.DOMove(targetPos, 0.5f).SetEase(Ease.Unset);
            trigger = false;
        }

        //attack
        if (attackCooltime > shotDelay)
        {
            int attackType;
            attackType = Random.Range(0, 10);

            if (attackType >= 0 && attackType < 7)
            {
                // Normal Attack
                isAttack = true;
                var shotDir = GameManager.instance.playerController.transform.position - transform.position;
                shotDir.Normalize();
                var normalShot = NormalShot(shotDir);
                StartCoroutine(normalShot);
                isAttack = false;
            }
            else
            {
                // Triple Attack
                isAttack = true;
                var shotDir = GameManager.instance.playerController.transform.position - transform.position;
                shotDir.Normalize();
                var tripleShot = TripleShot(shotDir);
                StartCoroutine(tripleShot);
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

    public IEnumerator NormalShot(Vector3 shotDir)
    {
        yield return new WaitForSeconds(0.5f);

        var featherShot = SpawnFeather();

        while ((transform.position - featherShot.transform.position).sqrMagnitude < featherRange)
        {
            featherShot.transform.Translate(shotDir * shotSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        DespawnFeather(featherShot);
    }

    public GameObject SpawnFeather()
    {
        var instFeather = featherList[0];
        instFeather.SetActive(true);
        instFeather.transform.position = transform.position;

        featherList.Remove(featherList[0]);


        return instFeather;
    }


    public void DespawnFeather(GameObject feather)
    {
        feather.SetActive(false);
        feather.transform.localPosition = Vector3.zero;

        featherList.Add(feather);
    }

    public IEnumerator TripleShot(Vector3 shotDir)
    {
        var normalShot = NormalShot(shotDir);
        var normalShot2 = NormalShot(shotDir);
        var normalShot3 = NormalShot(shotDir);

        StartCoroutine(normalShot);
        yield return new WaitForSeconds(tripleShotDelay);
        StartCoroutine(normalShot2);
        yield return new WaitForSeconds(tripleShotDelay);
        StartCoroutine(normalShot3);

        isAttack = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pixy : MonoBehaviour
{
    public PlayerController pc;
    public Animator anim;
    public ParticleSystem charge;
    public ParticleSystem ultEnerge;

    public bool getPixy;

    public bool isAttack;
    public bool isUlt;

    public float smoothSpeed;

    public Transform firePos;
    public Transform ultPos;
    [HideInInspector] public Vector3 targetPos;

    public float pixyMoveTime = 0.2f;
    public float counterRange = 100f;
    public float counterSpeed = 10f;

    public bool isReady = false;

    public List<GameObject> enemyList;

    public float ultDelay = 0.2f;
    public float ultTime = 10f;


    private void Awake()
    {
        
    }

    private void Start()
    {
        pc = GameManager.instance.playerController;

        targetPos = pc.Com.pixyTargetPos.position;
        firePos = pc.Com.pixyFirePos;
        ultPos = pc.Com.pixyUltPos;

        if(getPixy)
        {
            transform.position = targetPos;
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    private void Update()
    {
        if(getPixy)
        {
            HandleAni();

        }

        if (enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].GetComponent<MonsterController>() != null)
                {
                    if (!enemyList[i].GetComponent<MonsterController>().Stat.isAlive)
                    {
                        enemyList.Remove(enemyList[i]);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(getPixy)
        {
            if (isUlt)
            {
                targetPos = pc.Com.pixyUltPos.position;
                Move();
                Rotate();
            }
            else
            {
                targetPos = pc.Com.pixyTargetPos.position;
                if (!isAttack)
                {
                    Rotate();
                    Move();
                }
            }
        }
    }

    public void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }

    public void Rotate()
    {
        if (!pc.State.isLeft)
        {
            //see right;
            transform.localScale = new Vector3(1, 1, 1);
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            //see left
            transform.localScale = new Vector3(-1, 1, 1);
            transform.eulerAngles = new Vector3(0, -90, 0);

        }

    }

    public void HandleAni()
    {
        anim.SetBool("IsMoving", pc.State.isMoving);
    }

    public void ReadyToCounter()
    {
        var ready = Ready();
        StartCoroutine(ready); 
    }

    public IEnumerator Ready()
    {
        isAttack = true;

        anim.SetTrigger("Attack");
        charge.Play();

        transform.DOMove(firePos.position, pixyMoveTime).SetEase(Ease.Unset);

        yield return new WaitForSeconds(pixyMoveTime);

        var counter = Counter();
        StartCoroutine(counter);

        isAttack = false;
    }

    public IEnumerator Counter()
    {
        var counter = CustomPoolManager.Instance.counterPool.SpawnThis(transform.position, transform.eulerAngles, null);
        counter.isActive = true;
        Vector3 curPosition = transform.position;
        AudioManager.Instance.Audios.audioSource_PAttack.PlayOneShot(AudioManager.Instance.powerAttack);

        while (counter.isActive)
        {
            if ((curPosition - counter.transform.position).magnitude < counterRange)
            {
                counter.transform.Translate(Vector3.forward * counterSpeed * Time.fixedDeltaTime);

                yield return null;
            }
            else
            {
                counter.isActive = false;
                CustomPoolManager.Instance.ReleaseThis(counter);
            }
        }
    }

    public void Ult()
    {
        var ult = UltReady();
        StartCoroutine(ult);
    }

    public IEnumerator UltReady()
    {
        isAttack = true;

        var ult = CheckUltTime();
        StartCoroutine(ult);

        ultEnerge.Play();

        float cooltime = 10f;
        while(isUlt)
        {
            cooltime += Time.deltaTime;

            if (enemyList.Count > 0)
            {
                if (cooltime > ultDelay)
                {
                    UltShot();
                    cooltime = 0;
                }

            }

            yield return null;
        }

        isAttack = false;
    }

    IEnumerator CheckUltTime()
    {
        isUlt = true;

        yield return new WaitForSeconds(ultTime);

        ultEnerge.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        isUlt = false;
    } 

    public void UltShot()
    {
        if (enemyList.Count < 1)
            return;

        var ult = CustomPoolManager.Instance.bezierPool.SpawnThis(transform.position, transform.eulerAngles, null);

        ult.t = 0f;

        ult.master = this.gameObject;

        ult.enemy = null;

        int n = Random.Range(0, AudioManager.Instance.specialAttackClips.Count);


        AudioManager.Instance.Audios.audioSource_PAttack.PlayOneShot(AudioManager.Instance.specialAttackClips[n]);

        float shortDist = 100f;
        for(int i =0;i<enemyList.Count;i++)
        {
            var dist = (transform.position - enemyList[i].transform.position).sqrMagnitude;
            if (dist < shortDist)
            {
                shortDist = dist;
                ult.enemy = enemyList[i];
            }
        }

        if(ult.enemy == null)
        {
            CustomPoolManager.Instance.ReleaseThis(ult);
        }
        else
        {
            ult.Initialize();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (other.gameObject == enemyList[i])
                {
                    return;
                }
                else if (other.GetComponent<MonsterController>())
                {
                    if (other.GetComponent<MonsterController>().Stat.noneHit)
                        return;
                }
            }
            enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            enemyList.Remove(other.gameObject);
        }
    }
}

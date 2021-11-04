using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pixy : MonoBehaviour
{
    public PlayerController pc;
    public Transform firePos;

    public Animator anim;

    public bool isAttack;
    public bool isUlt;

    public Vector3 pixyPos;
    public Vector3 courchPixyPos;
    public Vector3 ultPos;

    public float pixyMoveTime = 0.2f;
    public float counterRange = 100f;
    public float counterSpeed = 10f;

    public bool isReady = false;

    public List<GameObject> enemyList;

    public float ultDelay = 0.2f;
    public float ultTime = 10f;



    private void Awake()
    {
        //firePos = transform.localPosition;
        transform.localPosition = pixyPos;
        //pixyModel.localPosition = pixyPos;
    }

    private void Update()
    {
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

    public void ReadyToCounter()
    {
        var ready = Ready();
        StartCoroutine(ready); 
    }

    public IEnumerator Ready()
    {
        isAttack = true;

        transform.parent = null;

        anim.SetTrigger("Attack");

        transform.DOMove(firePos.position, pixyMoveTime).SetEase(Ease.Unset);

        yield return new WaitForSeconds(pixyMoveTime);

        var counter = Counter();
        StartCoroutine(counter);
        EndCounter();
    }

    public IEnumerator Counter()
    {
        var counter = CustomPoolManager.Instance.counterPool.SpawnThis(transform.position, transform.eulerAngles, null);
        counter.isActive = true;
        Vector3 curPosition = transform.position;

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

    public void EndCounter()
    {
        isAttack = false;   

        transform.parent = pc.transform;
        transform.DOLocalMove(pixyPos, pixyMoveTime).SetEase(Ease.Unset);
        transform.DOLocalRotate(Vector3.zero, pixyMoveTime).SetEase(Ease.Unset);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Ult()
    {
        var ult = UltReady();
        StartCoroutine(ult);
    }

    public IEnumerator UltReady()
    {
        isAttack = true;

        transform.DOLocalMove(ultPos, pixyMoveTime).SetEase(Ease.Unset);

        yield return new WaitForSeconds(pixyMoveTime);

        float cooltime = 10f;

        var ult = CheckUltTime();
        StartCoroutine(ult);

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

        EndCounter();
    }

    IEnumerator CheckUltTime()
    {
        isUlt = true;

        yield return new WaitForSeconds(ultTime);

        isUlt = false;
    }

    public void UltShot()
    {
        if (enemyList.Count < 1)
            return;

        Debug.Log("work");

        var ult = CustomPoolManager.Instance.bezierPool.SpawnThis(transform.position, transform.eulerAngles, null);

        ult.t = 0f;

        ult.master = this.gameObject;

        ult.enemy = null;


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

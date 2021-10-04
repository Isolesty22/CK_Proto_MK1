using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashProjectile : BearProjectile
{
    public Transform s;
    public Transform m;
    public Transform e;

    private Vector3 middlePos;
    public float despawnTime = 2f;
    private WaitForSeconds waitDespawnTime;


    private void Start()
    {
        Init(s.position, m.position, e.position);
        Move();
    }
    public void Init(Vector3 _start, Vector3 _middle, Vector3 _end)
    {
        startPos = _start;
        middlePos = _middle;
        endPos = _end;
        waitDespawnTime = new WaitForSeconds(despawnTime);
        //패링가능한지 안가능한지
        if (Random.Range(0, 2) == 1)
        {
            SetParryMode(false);
        }
        else
        {
            SetParryMode(true);
        }

        moveEnumerator = ProcessMove();
        playerController = GameManager.instance.playerController;
        parryEnumerator = playerController.Parrying();
    }

    public override void Move()
    {
        StartCoroutine(moveEnumerator);
    }

    protected override IEnumerator ProcessDespawn()
    {
        StopCoroutine(moveEnumerator);
        yield return null;
        CustomPoolManager.Instance.ReleaseThis(this);
    }

    protected override IEnumerator ProcessMove()
    {
        float progress = 0f;
        float timer = 0;

        Vector3 p1;
        Vector3 p2;

        while (progress < 1f)
        {
            timer += Time.deltaTime;

            progress = timer / moveTime;

            p1 = Vector3.Lerp(startPos, middlePos, progress);
            p2 = Vector3.Lerp(middlePos, endPos, progress);

            myTransform.position = Vector3.Lerp(p1, p2, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        myTransform.position = endPos;

        //사라짐 대기시간
        yield return waitDespawnTime;
        Despawn();
    }
}

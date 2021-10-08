using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashProjectile : BearProjectile
{
    private Vector3 middlePos;
    public float despawnTime = 2f;
    private WaitForSeconds waitDespawnTime;

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
            this.gameObject.transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            SetParryMode(true);
            this.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        gameObject.transform.rotation = Quaternion.Euler(GetRandomVector3());
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

        moveTime = Random.Range(moveTime - 0.8f, moveTime);
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

        OnTrigger = VoidFunc;
        gameObject.tag = "Untagged";
        //사라짐 대기시간
        yield return waitDespawnTime;
        Despawn();
    }
    private Vector3 GetRandomVector3()
    {
        return new Vector3(Random.Range(0f,180f), Random.Range(0f, 180f), Random.Range(0f, 180f));
    }
    private void VoidFunc() { }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger();
        }
    }
}

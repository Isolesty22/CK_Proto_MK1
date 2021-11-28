using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarProjectile : BearProjectile
{
    private float tempMoveTime = 0f;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private ParticleSystem particle;
    public void Init(Vector3 _start, Vector3 _end)
    {
        startPos = _start;
        endPos = _end;
        SetParryMode(canParry);
        moveEnumerator = ProcessMove();
        playerController = GameManager.instance.playerController;
        parryEnumerator = playerController.Parrying();
        tempMoveTime = Random.Range(moveTime, moveTime + 0.5f);
        col.enabled = true;
        model.SetActive(true);
    }

    public override void Move()
    {
        StartCoroutine(moveEnumerator);
    }
    private WaitForSeconds waitParticlePlay = new WaitForSeconds(1.1f);
    protected override IEnumerator ProcessDespawn()
    {
        StopCoroutine(moveEnumerator);
        moveEnumerator = ProcessMove();
        yield return null;
        col.enabled = false;
        particle.Play();
        model.SetActive(false);
        yield return waitParticlePlay;
        CustomPoolManager.Instance.ReleaseThis(this);
    }

    protected override IEnumerator ProcessMove()
    {
        float progress = 0f;
        float timer = 0;
        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;

            progress = timer / tempMoveTime;
            myTransform.position = Vector3.Lerp(startPos, endPos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        myTransform.position = endPos;
        Despawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            //OnTrigger();
            Despawn();//만 불러도 됨
        }
    }
}

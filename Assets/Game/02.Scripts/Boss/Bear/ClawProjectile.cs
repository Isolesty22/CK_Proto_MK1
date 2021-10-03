using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawProjectile : MonoBehaviour
{
    public Transform myTransform;

    public float moveTime;
    public float degree;

    private Vector3 startPos;
    private Vector3 endPos;

    private PlayerController playerController = null;
    private IEnumerator moveEnumerator = null;
    public void Init(Vector3 _start, Vector3 _end)
    {
        startPos = _start;
        endPos = _end;

        moveEnumerator = ProcessMove();
        playerController = GameManager.instance.playerController;
    }

    public void Move()
    {
        StartCoroutine(moveEnumerator);
    }

    private IEnumerator ProcessMove()
    {
        float progress = 0f;
        float timer = 0;
        while (progress < 1f)
        {
            timer += Time.deltaTime;

            progress = timer / moveTime;
            myTransform.position = Vector3.Lerp(startPos, endPos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        myTransform.position = endPos;
        Despawn();
    }

    private void Despawn()
    {
        StopCoroutine(moveEnumerator);
        CustomPoolManager.Instance.ReleaseThis(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //피격 가능한 상태일 때
            if (!playerController.IsInvincible())
            {
                //패링 가능한 상태라면
                if (playerController.CanParry())
                {
                    //패링
                    StartCoroutine(playerController.Parrying());
                }
                else // 패링 불가능한 상태라면
                {
                    //피격
                    playerController.Hit();
                }
                Despawn();
            }

        }
    }
}

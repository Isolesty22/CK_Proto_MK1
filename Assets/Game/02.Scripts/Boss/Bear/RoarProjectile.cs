using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarProjectile : MonoBehaviour
{
    public Transform myTransform;

    public float moveTime;

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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //if (!playerController.IsInvincible())
            //{
            //    if (playerController.CanParry())
            //    {
            //        StartCoroutine(playerController.Parrying());
            //        return;
            //    }
            //    else
            //    {
            //        playerController.Hit();
            //        Despawn();
            //        return;
            //    }
            //}


            if (!playerController.IsInvincible() && playerController.CanParry())
            {

                StartCoroutine(playerController.Parrying());
                return;
            }


            if (!playerController.IsInvincible())
            {
                playerController.Hit();
                Despawn();
                return;
            }
        }
    }
}

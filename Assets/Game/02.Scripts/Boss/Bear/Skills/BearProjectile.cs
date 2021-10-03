using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearProjectile : MonoBehaviour
{

    public float moveTime;
    public Transform myTransform;

    protected Vector3 startPos;
    protected Vector3 endPos;

    protected PlayerController playerController = null;
    protected IEnumerator moveEnumerator = null;
    protected IEnumerator parryEnumerator = null;


    protected virtual void Despawn()
    {
        StartCoroutine(ProcessDespawn());
    }

    protected virtual IEnumerator ProcessDespawn()
    {
        StopCoroutine(moveEnumerator);
        yield return null;
        CustomPoolManager.Instance.ReleaseThis(this);
    }

    protected void OnTrigger()
    {
        //피격 가능한 상태일 때
        if (!playerController.IsInvincible())
        {
            //패링 가능한 상태라면
            if (playerController.CanParry())
            {
                StopCoroutine(parryEnumerator);
                parryEnumerator = playerController.Parrying();
                StartCoroutine(parryEnumerator);
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

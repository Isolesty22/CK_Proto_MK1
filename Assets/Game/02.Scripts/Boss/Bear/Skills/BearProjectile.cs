using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class BearProjectile : MonoBehaviour
{

    public bool canParry;
    public float moveTime;
    public Transform myTransform;

    protected Vector3 startPos;
    protected Vector3 endPos;

    protected PlayerController playerController = null;
    protected IEnumerator moveEnumerator = null;
    protected IEnumerator parryEnumerator = null;

    public Action OnTrigger;
    public void SetParryMode(bool _canParry)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(TagName.Arrow), LayerMask.NameToLayer("SkillEffect"));

        //OnTrigger = Despawn;
        canParry = _canParry;
        if (canParry)
        {
            //OnTrigger = OnTrigger_CanParry;
            OnTrigger = OnTrigger_Despawn;
            gameObject.tag = TagName.ParryingObject;
        }
        else
        {
            OnTrigger = OnTrigger_OnlyHit;
        }
    }
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

    public virtual void Move()
    {
        StartCoroutine(moveEnumerator);
    }

    protected virtual IEnumerator ProcessMove()
    {
        yield break;
    }
    /// <summary>
    /// Monster 태그일 때 자동으로 Parry되므로 잘 사용되지 않을 가능성이 있음. 
    /// </summary>
    protected void OnTrigger_CanParry()
    {

        if (playerController.CanParry())
        {
            StopCoroutine(parryEnumerator);
            //parryEnumerator = playerController.Parrying();
            StartCoroutine(parryEnumerator);
            Despawn();
            return;
        }

        if (!playerController.IsInvincible()) // 패링 불가능한 상태라면
        {
            //피격
            playerController.Hit();
            Despawn();
        }
    }

    protected void OnTrigger_Despawn()
    {
        Despawn();
    }

    protected void OnTrigger_OnlyHit()
    {
        //피격 가능한 상태일 때
        if (!playerController.IsInvincible())
        {
            playerController.Hit();
            Despawn();
        }
    }
}

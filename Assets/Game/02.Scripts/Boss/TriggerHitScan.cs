using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerHitScan : MonoBehaviour
{

    public bool canParry;
    PlayerController playerController;

    public Action<Collider> hitAction;


    private void Awake()
    {
        if (canParry)
        {
            hitAction = OnTrigger_CanParry;
        }
        else
        {
            hitAction = OnTrigger_OnlyHit;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.instance.playerController;
    }

    private const string playerTag = "Player";
    private void OnTrigger_CanParry(Collider other)
    {
        if (other.CompareTag(playerTag))
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
            }

        }
    }

    private void OnTrigger_OnlyHit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (!playerController.IsInvincible())
            {
                Debug.Log("Hit");
                playerController.Hit();
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        hitAction(other);
    }
}

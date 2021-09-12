using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PlayerController playerController;
    public CapsuleCollider hitBox;
    public CapsuleCollider crouchHitBox;
    

    private void Start()
    {
        playerController = GameManager.instance.playerController;
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController.State.canParry && !playerController.State.isHit)
        {
            if(other.CompareTag("Monster"))
            {
                StartCoroutine(playerController.Parrying());
                return;
            }
        }

        //피격 가능
        if (!playerController.State.isInvincible )
        {
            if (other.CompareTag("Monster"))
            {
                playerController.Hit(other.transform);

                Debug.Log("hit");
            }
        }

    }
}

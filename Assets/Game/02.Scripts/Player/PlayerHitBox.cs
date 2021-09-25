using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public PlayerController playerController;
    public CapsuleCollider hitBox;
    public CapsuleCollider crouchHitBox;

    private void OnTriggerStay(Collider other)
    {
        if (playerController.State.canParry && !playerController.State.isInvincible)
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
                playerController.Hit();
            }
        }

    }
}

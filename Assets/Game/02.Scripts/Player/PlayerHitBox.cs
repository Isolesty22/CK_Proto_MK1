using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public PlayerController playerController;
    public CapsuleCollider hitBox;
    public CapsuleCollider crouchHitBox;

    public IEnumerator parry;

    private void Start()
    {
        parry = playerController.Parrying();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (!other.GetComponent<MonsterController>().Stat.isAlive)
                return;
        }


        if (playerController.State.canParry)
        {
            if(other.CompareTag("Monster"))
            {
                StopCoroutine(parry);

                parry = playerController.Parrying();
                StartCoroutine(parry);
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

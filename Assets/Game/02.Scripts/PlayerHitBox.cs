using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.instance.playerController;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!playerController.State.isInvincible)
        {
            if (other.CompareTag("Monster"))
            {
                playerController.Hit(other.transform);

                Debug.Log("hit");
            }
        }
    }
}

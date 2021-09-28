using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitScan : MonoBehaviour
{
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.instance.playerController;
    }

    private const string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        //if (playerController.State.canParry && !playerController.State.isInvincible)
        //{
        //    if (collision.CompareTag(playerTag))
        //    {
        //        StartCoroutine(playerController.Parrying());
        //        return;
        //    }
        //}

        //피격 가능
        if (!playerController.State.isInvincible)
        {
            if (other.CompareTag(playerTag))
            {
                Debug.Log("Hit");
                playerController.Hit();
            }
        }
    }
}

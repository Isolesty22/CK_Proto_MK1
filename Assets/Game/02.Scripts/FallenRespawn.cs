using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenRespawn : MonoBehaviour
{
    public Transform spawnPos;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameManager.instance.playerController.Hit();

            if (GameManager.instance.playerController.Stat.hp <= 0)
                return;
            else
            {
                GameManager.instance.playerController.transform.position = spawnPos.position + Vector3.up;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixyEnergyIncrease : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GameManager.instance.playerController;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Arrow))
        {
            if (!player.Com.pixy.isAttack)
            {
                player.Stat.pixyEnerge = Mathf.Clamp(player.Stat.pixyEnerge + 3f, 0, 30);
            }
        }
    }

}

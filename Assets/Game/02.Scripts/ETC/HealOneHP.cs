using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOneHP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            if (GameManager.instance.playerController.Stat.hp < 3)
            {
                GameManager.instance.playerController.Stat.hp += 1;
                gameObject.SetActive(false);
            }
        }
    }
    
}

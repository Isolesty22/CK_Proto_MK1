using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 집중 스킬의 작동을 위한 클래스
/// </summary>
public class BearConcentrateHelper : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }
    public void StartCheck()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //피격 가능한 상태일 때
            if (!playerController.IsInvincible())
            {
                playerController.Hit();
                Despawn();
            }
        }
    }
}

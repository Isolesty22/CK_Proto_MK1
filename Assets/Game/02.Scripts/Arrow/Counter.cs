using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int damage = 3;

    public bool isActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (!other.GetComponent<MonsterController>().Stat.isAlive)
                return;
        }

        if (other.CompareTag("Monster"))
        {
            AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(AudioManager.Instance.clipDict_ArrowHit["arrowHitPower"]);
            other.GetComponent<MonsterController>().Hit(damage);

            return;
        }

        //if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //{
        //    isActive = false;
        //    CustomPoolManager.Instance.ReleaseThis(this);
        //}
    }
}

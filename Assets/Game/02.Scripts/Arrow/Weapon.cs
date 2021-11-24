using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum State
    { 
        Basic    
    }

    public State weaponState;

    private float cooltime = 0;

    public float basicArrowSpeed =10f;
    public float basicArrowRange = 15f;
    public float basicArrowCooldown = .2f;

    private void Update()
    {
        cooltime += Time.deltaTime;
    }

    public void Fire()
    {
        if (cooltime > basicArrowCooldown)
        {
            if (weaponState == State.Basic)
            {
                int n;
                n = Random.Range(0, 3);
                if(n == 0)
                    AudioManager.Instance.Audios.audioSource_PAttack.PlayOneShot(AudioManager.Instance.clipDict_Player["IpeaAttack"]);
                else if(n==1)
                    AudioManager.Instance.Audios.audioSource_PAttack.PlayOneShot(AudioManager.Instance.clipDict_Player["IpeaAttack_low"]);
                else
                    AudioManager.Instance.Audios.audioSource_PAttack.PlayOneShot(AudioManager.Instance.clipDict_Player["IpeaAttack_high"]);

                var fire = BasicFire();
                StartCoroutine(fire);
            }
            cooltime = 0;
        }
    }

    public IEnumerator BasicFire()
    {
        var arrow = CustomPoolManager.Instance.basicArrowPool.SpawnThis(transform.position, transform.eulerAngles, null);
        arrow.VFX_trail.Clear();
        var fire = CustomPoolManager.Instance.firePool.SpawnThis(transform.position, Vector3.zero, null);
        fire.Play();

        arrow.isActive = true;


        while (arrow.isActive)
        {
            if ((transform.position - arrow.transform.position).magnitude < basicArrowRange)
            {
                var playerSpeed = Mathf.Abs(GameManager.instance.playerController.Val.moveVelocity.x);
                arrow.transform.Translate(Vector3.forward * (basicArrowSpeed + playerSpeed) * Time.deltaTime);

                yield return null;
            }
            else
            {
                arrow.isActive = false;
                CustomPoolManager.Instance.ReleaseThis(arrow);
            }
        }
    }

}

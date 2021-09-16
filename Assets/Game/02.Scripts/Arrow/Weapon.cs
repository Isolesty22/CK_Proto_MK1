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
                var fire = BasicFire();
                StartCoroutine(fire);
            }
            cooltime = 0;
        }
    }

    public IEnumerator BasicFire()
    {
        var arrow = CustomPoolManager.Instance.basicArrowPool.SpawnThis(transform.position, transform.eulerAngles, null);
        arrow.isActive = true;

        Vector3 curPosition = transform.position;

        while (arrow.isActive)
        {
            if ((curPosition - arrow.transform.position).sqrMagnitude < basicArrowRange)
            {
                arrow.transform.Translate(Vector3.forward * basicArrowSpeed * Time.fixedDeltaTime);

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

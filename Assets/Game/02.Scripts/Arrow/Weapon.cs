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
        GameObject arrow;
        arrow = ArrowPool.instance.SpawnArrow(transform.position, Quaternion.Euler(transform.eulerAngles));
        arrow.GetComponent<ArrowBase>().isAlive = true;

        Vector3 curPosition = transform.position;

        while (arrow.GetComponent<ArrowBase>().isAlive)
        {
            if ((curPosition - arrow.transform.position).sqrMagnitude > basicArrowRange)
            {
                arrow.GetComponent<ArrowBase>().isAlive = false;
                ArrowPool.instance.Despawn(arrow);
                break;
            }

            arrow.transform.Translate(Vector3.forward * basicArrowSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

    }

}

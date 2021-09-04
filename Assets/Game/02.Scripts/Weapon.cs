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

    public float basicArrowSpeed =10f;

    public float basicArrowRange = 15f;

    public IEnumerator Fire(Vector3 fireDir)
    {
        var arrow = ArrowPool.instance.SpawnArrow(transform.position, Quaternion.identity);
        Vector3 curPosition = transform.position;

        arrow.GetComponent<ArrowBase>().isAlive = true;

        if (weaponState == State.Basic)
        {
            while (arrow.GetComponent<ArrowBase>().isAlive) 
            {
                if ((curPosition - arrow.transform.position).sqrMagnitude > basicArrowRange)
                {
                    arrow.GetComponent<ArrowBase>().isAlive = false;
                    ArrowPool.instance.Despawn(arrow);
                    break;
                }

                arrow.transform.Translate(fireDir * basicArrowSpeed * Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }
        }
    }

}

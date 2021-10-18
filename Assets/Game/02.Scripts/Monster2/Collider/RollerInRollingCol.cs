using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerInRollingCol : MonoBehaviour
{
    public RollerController rollerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            if (other.GetComponent<RollerController>())
                return;

            if(other.transform.GetComponent<MonsterController>())
                other.transform.GetComponent<MonsterController>().Hit(rollerController.Stat2.rollingDamage);
        }

        else if (other.transform.name == "PlayerHitBox")
        {
            if (GameManager.instance.playerController.State.canParry == true)
            {
                if (rollerController.moveDir.x < 0)
                {
                    rollerController.moveDir = new Vector3(1, 0, 0);
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    rollerController.moveDir = new Vector3(-1, 0, 0);
                    transform.localEulerAngles = Vector3.zero;
                }
            }
        }
    }
}

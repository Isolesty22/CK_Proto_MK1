using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenRespawn : MonoBehaviour
{
    public Transform spawnPos;

    public ParticleSystem spawn;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameManager.instance.playerController.Hit();

            if (GameManager.instance.playerController.Stat.hp <= 0)
                return;
            else
            {
                spawn.gameObject.SetActive(true);
                spawn.transform.position = spawnPos.position+Vector3.up*0.05f;
                spawn.Play();

                //yield return new WaitForSeconds(playerController.Stat.spawnTime);

                GameManager.instance.playerController.transform.position = spawnPos.position + Vector3.up;
            }
        }
        else if (collision.transform.CompareTag("Monster"))
        {
            collision.transform.GetComponent<MonsterController>().ChangeState(MonsterController.MonsterState.DEATH);
        }
    }
}

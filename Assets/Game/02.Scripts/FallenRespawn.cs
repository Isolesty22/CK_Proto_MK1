using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenRespawn : MonoBehaviour
{
    public Transform spawnPos;

    public ParticleSystem spawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var fall = Fall();
            StartCoroutine(fall);
        }
        else if (other.CompareTag("Monster"))
        {
            other.transform.GetComponent<MonsterController>().ChangeState(MonsterController.MonsterState.DEATH);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Player"))
    //    {
    //        var fall = Fall();
    //        StartCoroutine(fall);
    //    }
    //    else if (collision.transform.CompareTag("Monster"))
    //    {
    //        collision.transform.GetComponent<MonsterController>().ChangeState(MonsterController.MonsterState.DEATH);
    //    }
    //}

    IEnumerator Fall()
    {
        GameManager.instance.playerController.Hit();

        Debug.Log("fall");

        if (GameManager.instance.playerController.Stat.hp <= 0)
        {

        }
        else
        {
            spawn.gameObject.SetActive(true);
            spawn.transform.position = spawnPos.position + Vector3.up * 0.05f;
            spawn.Play();

            yield return new WaitForSeconds(GameManager.instance.playerController.Stat.spawnTime);

            GameManager.instance.playerController.transform.position = spawnPos.position + Vector3.up;
        }
    }
}

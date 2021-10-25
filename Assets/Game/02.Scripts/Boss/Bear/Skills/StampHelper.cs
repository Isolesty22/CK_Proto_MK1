using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampHelper : MonoBehaviour
{
    MushRoomController monster;

    public BoxCollider wall;
    private void Awake()
    {
        monster = GetComponent<MushRoomController>();

    }
    private void Start()
    {
        StartCoroutine(ProcessDeath());
    }

    private IEnumerator ProcessDeath()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Death!!");
        monster.ChangeState(MonsterController.MonsterState.DEATH);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);


    }
}

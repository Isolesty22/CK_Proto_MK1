using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public int moveDir;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.gameObject.transform.position) > 15f)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move() // 1 : 辦難 2 : 謝難 3 : 辦鼻 4 : 謝鼻 5 : 鼻
    {
        if (moveDir == 0) return;
        else if (moveDir == 1) rigid.velocity = new Vector3(moveSpeed, 0, 0);
        else if (moveDir == 2) rigid.velocity = new Vector3(-moveSpeed, 0, 0);
        else if (moveDir == 3) rigid.velocity = new Vector3(moveSpeed, moveSpeed, 0);
        else if (moveDir == 4) rigid.velocity = new Vector3(-moveSpeed, moveSpeed, 0);
        else if (moveDir == 5) rigid.velocity = new Vector3(0, moveSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ground"))
            gameObject.SetActive(false);
    }
}

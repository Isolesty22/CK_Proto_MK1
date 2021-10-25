using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private GameObject player;


    void Awake()
    {
        player = GameManager.instance.playerController.gameObject;
    }
    void Update()
    {
        gameObject.transform.position = player.transform.position;
    }
}
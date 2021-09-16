//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MonsterHitBox : MonoBehaviour
//{
//    public GameObject originMonster;

//    private void Start()
//    {
//    }
//    private void Update()
//    {
//        gameObject.transform.position = originMonster.transform.position;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.transform.CompareTag("Bullet"))
//        {
//            other.gameObject.SetActive(false);
//            originMonster.transform.SendMessage("Hitted");
//        }

//        if (other.transform.GetComponent<SowBugController>() != null)
//        {
//            if (other.transform.CompareTag("Player") || other.transform.CompareTag("Wall"))
//            {
//                originMonster.transform.SendMessage("Dead");
//            }
//        }
//    }
//}

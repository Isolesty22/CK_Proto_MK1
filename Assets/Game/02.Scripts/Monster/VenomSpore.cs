//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class VenomSpore : MonoBehaviour
//{
//    public GameObject originMonster;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.transform.CompareTag("Player"))
//        {
//                //플레이어 중독상태
//                StartCoroutine(VenomMushroonPoison());
//        }

//    }
//    private IEnumerator VenomMushroonPoison()
//    {
//        if (GameManager.instance.playerController.State.isPoison == true)
//            yield return null;

//        GameManager.instance.playerController.State.isPoison = true;
//        yield return new WaitForSeconds(originMonster.GetComponent<VenomMushroomController>().poisonTime);
//        GameManager.instance.playerController.State.isPoison = false;
//    }

//}

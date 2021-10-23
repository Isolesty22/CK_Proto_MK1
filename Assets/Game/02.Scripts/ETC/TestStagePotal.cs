using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStagePotal : MonoBehaviour
{

    public void Active() => gameObject.SetActive(true);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GoNextStage();
        }
    }
}

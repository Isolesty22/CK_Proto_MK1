using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public FlyerController flyerController;

    private void Awake()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            flyerController.DespawnFeather(this.gameObject);
        }
    }
}

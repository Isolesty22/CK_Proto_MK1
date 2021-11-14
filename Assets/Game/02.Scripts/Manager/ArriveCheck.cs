using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArriveCheck : MonoBehaviour
{
    public virtual event Action arrive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrive?.Invoke();
        }
    }
}

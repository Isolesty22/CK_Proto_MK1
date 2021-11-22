using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialCollider : MonoBehaviour
{

    public string coName;
    //public UnityEvent onTriggerEnter;

    private void Start()
    {
        TempTutorialLoader.Instance.AddDict(coName, this);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            TempTutorialLoader.Instance.StartCoPrac(coName);
            //onTriggerEnter.Invoke();
        }
    }

}

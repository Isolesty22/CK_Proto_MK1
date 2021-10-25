using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStagePotal : MonoBehaviour
{

    public bool noSaveReturnFieldMap;
    public void Active() => gameObject.SetActive(true);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (noSaveReturnFieldMap)
            {
                GameManager.instance.ReturnFieldMap();
            }
            else
            {
                GameManager.instance.GoNextStage();
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxInvisible : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(ActiveFalse());
    }
    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


}

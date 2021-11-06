using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeHelper : MonoBehaviour
{

    public BoxCollider box;
    private void Awake()
    {

        box.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(CoProcessStrike());
    }

    private IEnumerator CoProcessStrike()
    {
        yield return new WaitForSeconds(2f);
        box.enabled = true;
        yield return new WaitForSeconds(0.3f);
        box.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTalker : MonoBehaviour
{
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagName.Player))
        {
            col.enabled = false;
            StartCoroutine(CoTalk());
        }

    }

    private WaitForSeconds waitSec = new WaitForSeconds(3f);
    private IEnumerator CoTalk()
    {
        UIManager.Instance.Talk(DataManager.Instance.stageCode, 2.5f);
        yield return waitSec;

        UIManager.Instance.Talk(DataManager.Instance.stageCode + 1, 2.5f);
        yield return waitSec;

    }
}

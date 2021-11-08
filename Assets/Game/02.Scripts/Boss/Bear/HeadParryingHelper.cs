using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 집중 스킬의 작동을 위한 클래스
/// </summary>
public class HeadParryingHelper : MonoBehaviour
{
    IEnumerator parryEnumerator;

    [HideInInspector]
    public bool isSucceedParry = false;
    public void Init()
    {
        isSucceedParry = false;
        gameObject.SetActive(false);
    }
    public void StartCheck()
    {
        isSucceedParry = false;
        gameObject.SetActive(true);
        parryEnumerator = GameManager.instance.playerController.Parrying();
    }
    public void EndCheck()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            if (isSucceedParry)
            {
                return;
            }

            if (GameManager.instance.playerController.CanParry())
            {
                Debug.Log("Head Parry Succeed!");
                StopCoroutine(parryEnumerator);
                StartCoroutine(parryEnumerator);
                isSucceedParry = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pixy : MonoBehaviour
{
    public Transform pixyModel;
    public Vector3 originalPos;
    public Vector3 crouchPos;
    public Vector3 originalModelPos;
    public float pixyMoveTime = 0.2f;
    public float counterRange = 100f;
    public float counterSpeed = 10f;

    private void Awake()
    {
        originalPos = transform.localPosition;
        pixyModel.localPosition = originalModelPos;
    }

    public void ReadyToCounter()
    {
        pixyModel.DOLocalMove(Vector3.zero, pixyMoveTime).SetEase(Ease.Unset);
    }

    public IEnumerator Counter()
    {
        GameObject counter;
        counter = ArrowPool.instance.SpawnCounter(transform.position, Quaternion.Euler(transform.eulerAngles));

        Vector3 curPosition = transform.position;

        counter.GetComponent<ArrowBase>().isAlive = true;

        while (counter.GetComponent<ArrowBase>().isAlive)
        {
            if ((curPosition - counter.transform.position).sqrMagnitude > counterRange)
            {
                counter.GetComponent<ArrowBase>().isAlive = false;
                ArrowPool.instance.DespawnCounter(counter);
                break;
            }

            counter.transform.Translate(Vector3.forward * counterSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

    }

    public void EndCounter()
    {
        pixyModel.DOLocalMove(originalModelPos, pixyMoveTime).SetEase(Ease.Unset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPEffector : MonoBehaviour
{

    public Rigidbody rb;
    public BoxCollider boxCollider;
    public Vector3 effectVector;

    private const float cameraShakeValue = 0.5f;


    private void Awake()
    {

    }
    private void Start()
    {
       StartCoroutine(CoGrow());

    }
    private IEnumerator CoGrow()
    {
        boxCollider.isTrigger = true;

        rb.MovePosition(rb.position - new Vector3(0, boxCollider.bounds.size.y / 2f, 0));
        yield return YieldInstructionCache.WaitForFixedUpdate;

        GameManager.instance.cameraManager.AddShakeValue(cameraShakeValue);
        float timer = 0f;
        float progress = 0f;
        Vector3 currentPos;
        Vector3 startPos = rb.position;
        Vector3 endPos = rb.position + new Vector3(0, boxCollider.bounds.size.y / 2f, 0);
        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / 1f;

            currentPos = Vector3.Lerp(startPos, endPos, progress);
            rb.MovePosition(currentPos);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        GameManager.instance.cameraManager.AddShakeValue(-cameraShakeValue);
        boxCollider.isTrigger = false;
        yield break;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            GameManager.instance.playerController.Val.knockBackVelocity += effectVector;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            GameManager.instance.playerController.Val.knockBackVelocity -= effectVector;
        }
    }
}

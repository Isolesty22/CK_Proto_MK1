using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomMapWall : MonoBehaviour
{

    private bool isUp;
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public Transform modelTransform;
    [Space(5)]
    public Vector3 effectVector;

    /// <summary>
    /// 땅 속에 있을 때의 포지션값입니다.
    /// </summary>
    public Vector3 invisiblePosition { get; private set; }

    /// <summary>
    /// 땅 밖에 있을 때의 포지션값입니다.
    /// </summary>
    public Vector3 visiblePosition { get; private set; }

    private const float cameraShakeValue = 0.5f;


    private void Awake()
    {
        //포지션 값 초기화
        invisiblePosition = rb.position - new Vector3(0, boxCollider.bounds.size.y, 0);
        visiblePosition = invisiblePosition + new Vector3(0, boxCollider.bounds.size.y, 0);

        rb.transform.position = invisiblePosition;
        isUp = false;
    }


    public void StartCoUp()
    {
        if (isUp)
        {
            return;
        }
        StartCoroutine(CoUp());
    }

    public void StartCoDown()
    {
        if (!isUp)
        {
            return;
        }
        StartCoroutine(CoDown());
    }
    /// <summary>
    /// 땅 밖으로 나옵니다.
    /// </summary>
    private IEnumerator CoUp()
    {
        isUp = true;
        boxCollider.isTrigger = true;

        rb.position = invisiblePosition;
        yield return YieldInstructionCache.WaitForFixedUpdate;

        GameManager.instance.cameraManager.AddShakeValue(cameraShakeValue);
        float timer = 0f;
        float progress = 0f;
        Vector3 currentPos;
        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / 1f;

            currentPos = Vector3.Lerp(invisiblePosition, visiblePosition, progress);
            rb.MovePosition(currentPos);
            yield return YieldInstructionCache.WaitForFixedUpdate;

        }

        GameManager.instance.cameraManager.AddShakeValue(-cameraShakeValue);
        boxCollider.isTrigger = false;
        yield break;
    }

    /// <summary>
    /// 땅 속으로 들어갑니다.
    /// </summary>
    private IEnumerator CoDown()
    {
        isUp = false;
        boxCollider.isTrigger = false;

        rb.position = visiblePosition;
        yield return YieldInstructionCache.WaitForFixedUpdate;

        GameManager.instance.cameraManager.AddShakeValue(cameraShakeValue);

        float timer = 0f;
        float progress = 0f;
        Vector3 currentPos;

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / 1f;

            currentPos = Vector3.Lerp(visiblePosition, invisiblePosition, progress);
            rb.MovePosition(currentPos);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        GameManager.instance.cameraManager.AddShakeValue(-cameraShakeValue);
        boxCollider.isTrigger = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushSpiderHelper : MonoBehaviour
{

    public SpiderController[] spiders;

    public Transform moveUpTransform;
    private Vector3 originPos;


    public Rigidbody rb;

    private Vector3 moveUpPosition;
    private void Awake()
    {
        originPos = rb.position;
        moveUpPosition = new Vector3(originPos.x, rb.position.y, originPos.z);
    }
    private void Start()
    {
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        //대략 러쉬가 끝날 때 까지 대기
        yield return new WaitForSeconds(3f);

        float timer = 0f;
        float moveTime = 2f;
        float progress = 0f;

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / moveTime;

            rb.position = Vector3.Lerp(originPos, moveUpPosition, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        for (int i = 0; i < spiders.Length; i++)
        {
            spiders[i].ChangeState(MonsterController.MonsterState.DEATH);
        }
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

}

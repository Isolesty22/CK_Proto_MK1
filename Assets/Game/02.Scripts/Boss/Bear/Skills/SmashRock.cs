using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 곰의 Smash 스킬에 사용되는 돌덩이 하나.
/// </summary>
public class SmashRock : MonoBehaviour
{
    [Tooltip("소멸 시 되돌아갈 부모 트랜스폼")]
    public Transform parentTransform;

    [Space(5)]

    public Transform tr;
    public Transform modelTr;
    public Rigidbody rb;

    public Renderer meshRenderer;

    private Vector3 startPos;
    private Vector3 midPos;
    private Vector3 endPos;

    private Vector3 originPos;
    private Vector3 rotateDir;

    private Quaternion originRot;

    private float rotateSpeed;
    private float moveTime;

    private WaitForSeconds waitDespawnTime;

    private IEnumerator move;

    private void Awake()
    {
        rotateSpeed = 1f;
        moveTime = 2.5f;


        //피봇을 가운데로 변경하기

        //Vector3 center = meshRenderer.bounds.center;
        //Debug.Log(center);
        //modelTr.position -= center;
        //tr.position += center;

        //originRot = Quaternion.Euler(Vector3.zero);
        //originPos = tr.localPosition;

        move = CoMove();
        waitDespawnTime = new WaitForSeconds(2f);

        SetActive(false);
    }

    public void UpdatePivot(Vector3 _value)
    {
        Vector3 center = meshRenderer.bounds.center;
        center -= _value;

        modelTr.position -= center;
        tr.position += center;

        originRot = Quaternion.Euler(Vector3.zero);
        originPos = tr.localPosition;
    }
    public void UpdateVectors(Vector3 _start, Vector3 _mid, Vector3 _end)
    {
        startPos = _start;
        midPos = _mid;
        endPos = _end;

        //회전값 랜덤으로 설정
        rotateDir = new Vector3((int)Random.Range(0, 2), (int)Random.Range(0, 2), (int)Random.Range(0, 2));

        //아예 안돌면 z값 돌리기
        if (rotateDir == Vector3.zero)
        {
            rotateDir = new Vector3(0, 0, 1);
        }
        rotateDir *= rotateSpeed;
    }

    public void StartMove()
    {
        if (move != null)
        {
            StopCoroutine(move);
        }
        SetActive(true);
        move = CoMove();
        StartCoroutine(move);
    }
    protected IEnumerator CoMove()
    {

        tr.parent = null;
        gameObject.tag = TagName.ParryingObject;

        float progress = 0f;
        float timer = 0;


        Vector3 p1;
        Vector3 p2;
        Vector3 currentPos;
        Vector3 currentRot = Vector3.zero;

        float randMoveTime = Random.Range(moveTime - 1f, moveTime);

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;

            progress = timer / randMoveTime;

            p1 = Vector3.Lerp(startPos, midPos, progress);
            p2 = Vector3.Lerp(midPos, endPos, progress);
            currentPos = Vector3.Lerp(p1, p2, progress);

            currentRot += rotateDir;

            rb.MovePosition(currentPos);
            rb.MoveRotation(Quaternion.Euler(currentRot));

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        gameObject.tag = TagName.Untagged;
        //사라짐 대기시간
        yield return waitDespawnTime;
        Despawn();

    }

    public void SetActive(bool _b) => gameObject.SetActive(_b);

    public void Despawn()
    {
        SetActive(false);

        //부모를 원래대로 되돌리기
        tr.parent = parentTransform;

        //위치, 회전값을 원래대로 되돌리기
        tr.localRotation = originRot;
        tr.localPosition = originPos;


        move = null;
    }
}

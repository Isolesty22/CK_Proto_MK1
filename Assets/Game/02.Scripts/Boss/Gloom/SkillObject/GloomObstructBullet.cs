﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomObstructBullet : MonoBehaviour
{

    [Tooltip("투사체가 생성된 후 waitTime만큼 대기 후 이동을 시작합니다.")]
    private float waitTime;
    [Tooltip("투사체가 맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
    private float moveTime;

    [HideInInspector]
    public GloomController gloom;
    private AnimationCurve curve;

    public Rigidbody rb;
    //public Transform myTransform;
    public Collider myCollider;

    private Vector3 startPos;
    private Vector3 endPos;

    private PlayerController player;
    private IEnumerator moveCoroutine = null;

    private WaitForSeconds waitSec = null;
    private Quaternion rightRot;
    private Quaternion leftRot;


    private void Start()
    {
        player = GameManager.instance.playerController;
    }


    public void Init(GloomController _gloom, Vector3 _startPos, Vector3 _endPos, float _moveTime)
    {
        moveCoroutine = ProcessMove();
        gloom = _gloom;
        curve = _gloom.SkillVal.obstruct.curve;
        startPos = _startPos;
        endPos = _endPos;
        moveTime = _moveTime;
        waitTime = gloom.SkillVal.obstruct.waitTime;
        myCollider.enabled = false;
        waitSec = new WaitForSeconds(waitTime);


    }

    public void Move()
    {
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator ProcessMove()
    {
        yield return waitSec;

        float timer = 0f;
        float progress = 0f;

        myCollider.enabled = true;

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime * curve.Evaluate(progress);
            progress = timer / moveTime;

            rb.MovePosition(Vector3.Lerp(startPos, endPos, progress));
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        rb.MovePosition(endPos);
        Despawn();
    }
    public void Despawn()
    {
        StopCoroutine(moveCoroutine);
        StartCoroutine(ProcessDespawn());
    }

    private IEnumerator ProcessDespawn()
    {

        yield return null;

        gloom.Pool.obstructBullet.ReleaseThis(this);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            Despawn();
        }
    }
}

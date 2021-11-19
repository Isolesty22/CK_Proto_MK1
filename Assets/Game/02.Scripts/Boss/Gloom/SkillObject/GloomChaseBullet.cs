using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Values = GloomController.SkillValues.ChasePattern;

public class GloomChaseBullet : MonoBehaviour
{

    public Rigidbody rigidBody;

    [ReadOnly]
    public Values Val;

    private Vector3 startPosition;
    private Vector3 midPosition;
    private Vector3 endPosition;

    private Vector3 currentPosition;

    private GloomController gloom;
    private IEnumerator moveCoroutine = null;

    public void Init(GloomController _gloom)
    {
        gloom = _gloom;
        moveCoroutine = ProcessMove();
        Val = gloom.SkillVal.chase;
        //gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    public void SetPosition(Vector3 _start, Vector3 _end)
    {
        startPosition = _start;
        endPosition = _end;
        midPosition = gloom.SkillVal.chase.curvedPosition + (_end * 0.3f);
    }
    public void Move()
    {
        StartCoroutine(moveCoroutine);
    }
    private IEnumerator ProcessMove()
    {
        float timer = 0f;
        float progress = 0f;
        //float posZ = rigidBody.position.z;
        //float posY = rigidBody.position.y;
        Vector3 p1, p2;
        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / Val.moveTime;

            p1 = Vector3.Lerp(startPosition, midPosition, progress);
            p2 = Vector3.Lerp(midPosition, endPosition, progress);

            currentPosition = Vector3.Lerp(p1, p2, progress);

            rigidBody.MoveRotation(Quaternion.LookRotation(currentPosition - rigidBody.position, Vector3.up));
            rigidBody.MovePosition(currentPosition);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Despawn();
    }

    public void Despawn()
    {
        StopCoroutine(moveCoroutine);
        StartCoroutine(ProcessDespawn());
    }

    private IEnumerator ProcessDespawn()
    {

        // yield return YieldInstructionCache.WaitForFixedUpdate;
        yield return null;
        gloom.Pool.chaseHit.SpawnThis(rigidBody.position);
        gloom.Pool.chaseBullet.ReleaseThis(this);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            Despawn();
        }
    }
}

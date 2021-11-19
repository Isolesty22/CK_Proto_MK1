using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Values = GloomController.SkillValues.WavePattern;

[RequireComponent(typeof(Rigidbody))]
public class GloomWaveBullet : MonoBehaviour
{

    public enum eMode
    {
        Up,
        Down
    }

    public Rigidbody rigidBody;

    [ReadOnly]
    public eMode mode;

    [ReadOnly]
    public Values Val;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private Vector3 currentPosition;

    private GloomController gloom;
    public void Init(GloomController _gloom, eMode _mode)
    {
        gloom = _gloom;
        Val = _gloom.SkillVal.wave;
        mode = _mode;

        Val.magnitude = Mathf.Abs(Val.magnitude);
        if (_mode == eMode.Down)
        {
            Val.magnitude *= -1f;
        }

    }

    public void SetPosition(Vector3 _start, Vector3 _end)
    {
        startPosition = _start;
        endPosition = _end;
    }
    public void Move()
    {
        StartCoroutine(ProcessMove());
    }

    private IEnumerator ProcessMove()
    {
        float timer = 0f;
        float progress = 0f;
        float posZ = rigidBody.position.z;
        float posY = rigidBody.position.y;

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / Val.moveTime;

            currentPosition = new Vector3(
                Mathf.Lerp(startPosition.x, endPosition.x, progress),
                posY + Mathf.Sin(timer * Val.frequency) * Val.magnitude,
                posZ);

            rigidBody.MoveRotation(Quaternion.LookRotation(currentPosition - rigidBody.position, Vector3.up));
            rigidBody.MovePosition(currentPosition);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        gloom.Pool.waveBullet.ReleaseThis(this);
    }

}

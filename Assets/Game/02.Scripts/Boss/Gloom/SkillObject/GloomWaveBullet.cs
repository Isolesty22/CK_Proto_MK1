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

    [HideInInspector]
    public Vector3 startPosition;

    [HideInInspector]
    public Vector3 endPosition;

    private Vector3 currentPosition;

    private GloomController gloom;
    public void Init(GloomController _gloom, eMode _mode)
    {
        gloom = _gloom;
        Val = _gloom.SkillVal.wave;
        mode = _mode;
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

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / Val.moveTime;

            currentPosition = new Vector3(
                Mathf.Lerp(startPosition.x, endPosition.x, progress),
                Mathf.Sin(timer * Val.frequency) * Val.magnitude,
                posZ
                );

            rigidBody.position = currentPosition;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
    }

}

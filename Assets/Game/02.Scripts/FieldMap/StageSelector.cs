using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent((typeof(Rigidbody)))]
/// <summary>
/// 스테이지를 선택하는, 이동할 수 있는 무언가
/// </summary>
public class StageSelector : MonoBehaviour
{


    public enum eState
    {
        Wait,
        Move
    }

    [System.Serializable]
    public class Components
    {
        public Rigidbody rigidBody;
        public Animator animator;
    }
    public Components Com => _components;

    [SerializeField]
    private Components _components;

    [HideInInspector]
    public float timer = 0f;
    [HideInInspector]
    public float progress = 0f;
    [HideInInspector]
    public float moveTime = 3f;

    public eState state;

    private Vector3 startPosition;
    private Vector3 destPosition;

    public void StartProcessMove()
    {
        StartCoroutine(ProcessMove());
    }
    private IEnumerator ProcessMove()
    {
        state = eState.Move;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / moveTime;

            SetPosition(Vector3.Lerp(startPosition, destPosition, progress));

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Debug.Log("Finish Move!");

        state = eState.Wait;
    }

    /// <summary>
    /// ProcessMove가 실행되는 도중에 목적지를 바꿉니다.
    /// </summary>
    public void ChangeDestinationPos(Vector3 position)
    {
        startPosition = Com.rigidBody.position;
        destPosition = position;
        timer = 0f;
    }

    public void SetStartPos(Vector3 position) => startPosition = position;
    public void SetDestinationPos(Vector3 position) => destPosition = position;
    public void SetPosition(Vector3 position) => Com.rigidBody.position = position;

}

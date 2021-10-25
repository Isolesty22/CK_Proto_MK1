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

    private Vector3 currentPosition;
    private Vector3 destPosition;

    private IEnumerator ProcessMove()
    {
        state = eState.Move;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / moveTime;

            currentPosition = Vector3.Lerp(Com.rigidBody.position, destPosition, progress);

            SetPosition(currentPosition);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Debug.Log("Finish Move!");

        state = eState.Wait;
    }

    public void SetDestination(Vector3 position) => destPosition = position;
    public void SetPosition(Vector3 position) => Com.rigidBody.position = position;

}

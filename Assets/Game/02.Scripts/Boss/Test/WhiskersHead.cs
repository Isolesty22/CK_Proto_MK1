using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gloom 수염의 시작부분입니다.
/// </summary>
public class WhiskersHead : MonoBehaviour
{
    public float rotationSpeed; //회전할 때의 스피드
    public float moveSpeed;
    public Transform target; //바라볼 타겟
    public Rigidbody rigidBody;

    private Vector2 dirtection;

    private Vector2 lastMousePosition;
    private Vector2 mousePosition;
    public float limitDistance = 2f;

    void Start()
    {
        //GoRotate();
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        GoRotate(); //헐 짱 편한데 델리게이트??? 뭐임이게?? 와...
        GoMove(target.position);
    }
    void GoMove(Vector3 target)
    {
        //rigidBody.MovePosition(target);
    }

    void GoRotate()
    {
        Vector3 targetPosition = target.position;
        dirtection = targetPosition - rigidBody.position;
        float angle = Mathf.Atan2(dirtection.y, dirtection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
        //rigidBody.transform.rotation = 
        //rigidBody.transform.SetPositionAndRotation(, Quaternion.Slerp(rigidBody.transform.rotation, rotation, rotationSpeed * Time.smoothDeltaTime));
        rigidBody.rotation = Quaternion.Slerp(rigidBody.transform.rotation, rotation, rotationSpeed * Time.smoothDeltaTime);
        rigidBody.position = target.position;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, limitDistance);
    }
}

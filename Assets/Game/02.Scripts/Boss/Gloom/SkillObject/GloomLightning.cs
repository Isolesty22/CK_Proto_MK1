using UnityEngine;
using System.Collections;

public class GloomLightning : MonoBehaviour
{

    [HideInInspector]
    public float moveTime = 10f;
    [Space(5)]
    public Rigidbody myRB;
    public Rigidbody startRB;
    public Rigidbody endRB;
    public Transform myTransform;
    public Transform lineParent;
    [Space(5)]

    [Tooltip("사용할 라인렌더러의 개수")]
    public int lineCount = 2;
    public float lineWidth = 3f;
    [Tooltip("흔들흔들 랜덤값의 범위입니다.")]
    public float widthRandRange = 1f;

    [Tooltip("라인 렌더러의 속 포인트 개수")]
    public int pointCount = 3;

    [Tooltip("흔들흔들 랜덤값의 범위입니다.")]
    public float jigleRandRange = 0.2f;
    [Range(0.01f, 0.1f)]
    public float updateDelay = 0.2f;

    [Tooltip("매 업데이트시, 해당 그라디언트들 중 랜덤으로 색이 출력됩니다.")]
    public Gradient[] lineGradients = new Gradient[3];
    // public Gradient lightGradient;
    [Header("프리팹")]
    public GameObject lineRendererPrefab;

    private LineRenderer[] lines;

    private float nextUpdateTime = 0f;
    private int hitMask;

    private RaycastHit hit;
    private Ray ray = new Ray();

    [Header("Raycase 관련")]
    public float hitDistance = 50f;
    [ReadOnly]
    public Vector3 hitPosition;

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Vector3 endPosition;

    //private void FixedUpdate()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > nextUpdateTime)
    //    {

    //        UpdateLightning();
    //        nextUpdateTime = timer + updateDelay;
    //    }
    //}
    public void Init()
    {
        nextUpdateTime = 0f;
    }
    public void SetXPosition(Vector3 _startPos, Vector3 _endPos)
    {
        startPosition = myTransform.position;
        endPosition = myTransform.position;

        startPosition = new Vector3(_startPos.x, startPosition.y, startPosition.z);
        endPosition = new Vector3(_endPos.x, startPosition.y, endPosition.z);

    }
    public void Init_Position()
    {
        myTransform.position = startPosition;
    }


    public IEnumerator CoMove()
    {
        float progress = 0f;
        float fixedTimer = 0f;
        Vector3 currentPos;


        while (progress < 1f)
        {
            fixedTimer += Time.deltaTime;
            progress = fixedTimer / moveTime;

            //번개 모양 업데이트
            if (fixedTimer > nextUpdateTime)
            {
                UpdateLightning();
                nextUpdateTime = fixedTimer + updateDelay;
            }

            //위치 이동
            currentPos = Vector3.Lerp(startPosition, endPosition, progress);
            myRB.MovePosition(currentPos);



            ray.direction = Vector3.down;
            ray.origin = startRB.position;
            if (Physics.Raycast(ray, out hit, hitDistance, hitMask))
            {
                hitPosition = hit.point;
                hitPosition = new Vector3(myRB.position.x, hitPosition.y, myRB.position.z);
            }
            else
            {
                hitPosition = Vector3.down * hitDistance;
                hitPosition = new Vector3(myRB.position.x, hitPosition.y, myRB.position.z);
            }

            endRB.MovePosition(hitPosition);

            Debug.DrawLine(startPosition, hitPosition, Color.green);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
    }



    public void Create()
    {

        lines = new LineRenderer[lineCount];
        hitMask = 1 << LayerMask.NameToLayer("Ground");
        hitPosition = Vector3.down * hitDistance;
        //lineCount만큼 복사
        for (int i = 0; i < lineCount; i++)
        {

            LineRenderer currentLine = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
            lines[i] = currentLine;

            currentLine.enabled = false;
            currentLine.colorGradient = GetRandomGradient();

            currentLine.startWidth = lineWidth;
            currentLine.endWidth = lineWidth;

            currentLine.transform.parent = lineParent;
        }

        nextUpdateTime = updateDelay;
    }

    public void SetEnabled(bool _b)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].enabled = _b;
        }
    }
    private Gradient GetRandomGradient() => lineGradients[Random.Range(0, lineGradients.Length)];

    private float GetRandomWidth() => lineWidth + Random.Range(-widthRandRange, widthRandRange);

    public void UpdateLightning()
    {
        float distance = Vector2.Distance(startRB.position, endRB.position);

        for (int i = 0; i < lines.Length; i++)
        {
            LineRenderer tempLine = lines[i];

            tempLine.positionCount = pointCount;

            tempLine.SetPosition(0, startRB.position);

            Vector2 endPos = startRB.position;

            for (int j = 1; j < pointCount - 1; j++)
            {
                Vector2 tempPos = Vector2.Lerp(startRB.position, endRB.position, (float)j / (float)pointCount);

                endPos = new Vector2(tempPos.x + Random.Range(-jigleRandRange, jigleRandRange), tempPos.y + Random.Range(-jigleRandRange, jigleRandRange));

                tempLine.SetPosition(j, endPos);
                tempLine.startWidth = GetRandomWidth();
                tempLine.endWidth = GetRandomWidth();
                tempLine.colorGradient = GetRandomGradient();
            }

            tempLine.SetPosition(pointCount - 1, endRB.position);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(startRB.position, 0.2f);
        Gizmos.DrawSphere(endRB.position, 0.2f);
        // Gizmos.DrawLine(startRB.position, endRB.position);
    }

}

using UnityEngine;
using System.Collections;

public class GloomLightning : MonoBehaviour
{

    [Tooltip("구슬의 y값을 정하기위한 트랜스폼.")]
    public Transform yTransform;
    private float yOriginPos;

    [HideInInspector]
    public float moveTime = 10f;

    [Header("Rigidbody")]
    public Rigidbody myRB;
    public Rigidbody startRB;
    public Rigidbody endRB;

    [Header("Transform")]
    public Transform myTransform;
    public Transform sphereTransform;
    public Transform sphereEffectTransform;
    public Transform lineEffectTransform;
    public Transform lineParent;

    [Header("Line Value")]
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

    /// <summary>
    /// 공격 중 구슬이 이동하는 시작 위치
    /// </summary>
    public Vector3 moveStartPosition { get; private set; }

    /// <summary>
    /// 공격 중 구슬이 이동하는 끝 위치
    /// </summary>
    public Vector3 moveEndPosition { get; private set; }

    /// <summary>
    /// 공격 전 구슬의 이동 연출에 사용되는 위치 중...시작 위치
    /// </summary>
    public Vector3 moveSphereStartPos { get; private set; }

    /// <summary>
    /// 공격 전 구슬의 이동 연출에 사용되는 위치 중...끝 위치
    /// </summary>
    public Vector3 moveSphereEndPos { get; private set; }

    /// <summary>
    /// 구슬이 제일 작을 때의 크기.
    /// </summary>
    private readonly Vector3 sphereSmallScale = new Vector3(0.1f, 0.1f, 0.1f);

    /// <summary>
    /// 구슬이 제일 클 때의 크기.
    /// </summary>
    private readonly Vector3 sphereBigScale = new Vector3(1.5f, 1.5f, 1.5f);

    private WaitForSeconds waitUpdateDelay = null;
    private float shakeAddValue;

    public AudioClip clip;
    private void Awake()
    {
        yOriginPos = yTransform.position.y;
        sphereEffectTransform.gameObject.SetActive(false);
        lineEffectTransform.gameObject.SetActive(false);
        shakeAddValue = 1f / lineCount;
        waitUpdateDelay = new WaitForSeconds(updateDelay);
       // Debug.LogError(shakeAddValue);
    }
    public void Init()
    {
        sphereEffectTransform.gameObject.SetActive(false);
        lineEffectTransform.gameObject.SetActive(false);
        nextUpdateTime = 0f;
    }

    public void Init_Position()
    {
        myTransform.position = moveSphereStartPos;
    }
    public void SetMovePosition(Vector3 _startPos, Vector3 _endPos)
    {
        moveStartPosition = myRB.position;
        moveEndPosition = myRB.position;

        moveStartPosition = new Vector3(_startPos.x, moveStartPosition.y, moveStartPosition.z);
        moveEndPosition = new Vector3(_endPos.x, moveStartPosition.y, moveEndPosition.z);
    }

    /// <summary>
    /// 번개를 쏘기 전 구슬이 움직일 위치를 정합니다. end의 y값은 yTransform의 값으로 들어갑니다.
    /// </summary>
    public void SetMoveSpherePosition(Vector3 _start, Vector3 _end)
    {
        myTransform.position = _start;

        moveSphereStartPos = _start;
        moveSphereEndPos = new Vector3(_end.x, yOriginPos, _end.z);

        sphereTransform.position = moveSphereStartPos;
        sphereTransform.localScale = sphereSmallScale;
    }
    private float progress = 0f;
    private float fixedTimer = 0f;
    private void ClearTimer()
    {
        progress = 0f;
        fixedTimer = 0f;
        nextUpdateTime = 0f;
    }

    /// <summary>
    /// 화면상에서 보이지 않는 높이의 Y값
    /// </summary>
    private float topPosY;


    private Vector3 startTopPos;

    /// <summary>
    /// 번개를 쏘지 않고 오직 구체만을 움직입니다. 크기도 커집니다(오브젝트 전체를 감싼 리지드바디를 움직이기 떄문에 주의가 필요합니다).
    /// </summary>
    public IEnumerator CoBeginMove()
    {


        ClearTimer();

        //끝부분 안보이게
        endRB.gameObject.SetActive(false);

        Vector3 currentPos = moveSphereStartPos;

        topPosY = moveSphereStartPos.y + 15f;

        startTopPos = new Vector3(moveSphereStartPos.x, topPosY, moveSphereStartPos.z);

        //=========================
        // 화면 밖 위 쪽으로 이동
        //=========================
        while (progress < 1f)
        {
            fixedTimer += Time.fixedDeltaTime;
            progress = fixedTimer / 2f;

            currentPos = Vector3.Lerp(moveSphereStartPos, startTopPos, progress);
            myRB.MovePosition(currentPos);

            sphereTransform.localScale = Vector3.Lerp(sphereSmallScale, sphereBigScale, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }


        //=========================
        // 아래로 이동해서 번개 이펙트 시작
        //=========================

        //시간 초기화

        ClearTimer();

        //사운드 시작
        AudioManager.Instance.Audios.audioSource_SFX.clip = clip;
        AudioManager.Instance.Audios.audioSource_SFX.Play();

        //위치 설정
        Vector3 endTopPos = new Vector3(moveSphereEndPos.x, startTopPos.y, moveSphereEndPos.z);
        myRB.position = endTopPos;

        yield return YieldInstructionCache.WaitForFixedUpdate;

        UpdateHitPosition();
        endRB.transform.position = hitPosition;

        //이펙트 등등 On
        lineParent.gameObject.SetActive(true);
        sphereEffectTransform.gameObject.SetActive(true);

        endRB.gameObject.SetActive(true);

        //카메라 흔들림값 초기화
        float currentShakeValue = 0f;
        //차례차례 번개 On
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].enabled = true;
            UpdateLightning(eUpdateLightningMode.Forced);

            yield return waitUpdateDelay;
            currentShakeValue += shakeAddValue;
            //GameManager.instance.cameraManager.SetShakeValue(currentShakeValue, currentShakeValue);
            GameManager.instance.cameraManager.AddShakeValue(shakeAddValue);
        }
        //마지막으로 큰 줄기 On
        lineEffectTransform.gameObject.SetActive(true);

        //-----아래로 슝
        while (progress < 1f)
        {
            fixedTimer += Time.fixedDeltaTime;
            progress = fixedTimer / 1f;

            UpdateLightning(eUpdateLightningMode.UpdateTime);

            currentPos = Vector3.Lerp(endTopPos, moveSphereEndPos, progress);
            myRB.MovePosition(currentPos);

            UpdateHitPosition();
            endRB.MovePosition(hitPosition);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

    }

    public IEnumerator CoMove()
    {
        ClearTimer();

        Vector3 currentPos;

        //정해진 방향으로 이동
        while (progress < 1f)
        {
            fixedTimer += Time.fixedDeltaTime;
            progress = fixedTimer / moveTime;

            //번개 모양 업데이트
            UpdateLightning(eUpdateLightningMode.UpdateTime);

            //위치 이동
            currentPos = Vector3.Lerp(moveStartPosition, moveEndPosition, progress);
            myRB.MovePosition(currentPos);

            //번개 길이 조절
            UpdateHitPosition();
            endRB.MovePosition(hitPosition);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //=========================
        // 이동을 끝내고 위 쪽으로 이동
        //=========================

        //시간 초기화 
        ClearTimer();
        Vector3 endTopPos = new Vector3(myRB.position.x, topPosY, myRB.position.z);



        while (progress < 1f)
        {
            fixedTimer += Time.fixedDeltaTime;
            progress = fixedTimer / 1.5f;

            UpdateLightning(eUpdateLightningMode.UpdateTime);

            currentPos = Vector3.Lerp(moveEndPosition, endTopPos, progress);
            myRB.MovePosition(currentPos);

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        yield return null;

        //=========================
        // 글룸 쪽으로 돌아가기
        //=========================

        //사운드 종료
        //AudioManager.Instance.Audios.audioSource_SFX.Stop();

        //이펙트 등등 Off
        lineEffectTransform.gameObject.SetActive(false);
        sphereEffectTransform.gameObject.SetActive(false);

        //흔들림값 초기화
        float currentShakeValue = 1f;

        //차례차례 번개 Off
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].enabled = false;
            UpdateLightning(eUpdateLightningMode.Forced);

            //서서히 덜 흔들리게
            //currentShakeValue -= shakeAddValue;
            //GameManager.instance.cameraManager.SetShakeValue(currentShakeValue, currentShakeValue);

            currentShakeValue -= shakeAddValue;
            //GameManager.instance.cameraManager.SetShakeValue(currentShakeValue, currentShakeValue);
            GameManager.instance.cameraManager.AddShakeValue(-shakeAddValue);
            yield return waitUpdateDelay;
        }

        //이펙트 끄기
        lineParent.gameObject.SetActive(false);
        endRB.gameObject.SetActive(false);


        //시간 초기화
        ClearTimer();

        //원래 위치의 위쪽으로 이동
        myRB.position = startTopPos;
        yield return YieldInstructionCache.WaitForFixedUpdate;

        //원래 위치로 이동과 동시에 스케일 줄이기
        while (progress < 1f)
        {
            fixedTimer += Time.fixedDeltaTime;
            progress = fixedTimer / 1f;

            currentPos = Vector3.Lerp(startTopPos, moveSphereStartPos, progress);
            myRB.MovePosition(currentPos);
            sphereTransform.localScale = Vector3.Lerp(sphereBigScale, sphereSmallScale, progress);

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

    public void SetLineEnabled(bool _b)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].enabled = _b;
        }
    }
    private Gradient GetRandomGradient() => lineGradients[Random.Range(0, lineGradients.Length)];

    private float GetRandomWidth() => lineWidth + Random.Range(-widthRandRange, widthRandRange);

    public enum eUpdateLightningMode
    {
        Forced,
        UpdateTime,
    }
    public void UpdateLightning(eUpdateLightningMode _mode)
    {

        if (_mode == eUpdateLightningMode.Forced)
        {
            UpdateLines();
        }
        else
        {
            if (fixedTimer > nextUpdateTime)
            {
                nextUpdateTime = fixedTimer + updateDelay;
                UpdateLines();
            }
        }
    }

    /// <summary>
    /// 번개의 라인렌더러를 업데이트합니다. 직접 호출하는건 좀 그럴수도...
    /// </summary>
    private void UpdateLines()
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

    /// <summary>
    /// Raycast를 실행하고, 결과에 따라서 hitPosition의 값을 변경합니다. 
    /// </summary>
    public void UpdateHitPosition()
    {
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //   Gizmos.DrawSphere(startRB.position, 0.2f);
        Gizmos.DrawSphere(endRB.position, 0.2f);
        // Gizmos.DrawLine(startRB.position, endRB.position);
    }

}

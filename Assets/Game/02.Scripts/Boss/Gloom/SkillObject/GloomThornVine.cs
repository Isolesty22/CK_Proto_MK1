using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomThornVine : MonoBehaviour, IDamageable
{

    #region Class

    [System.Serializable]
    public class Values
    {
        [HideInInspector]
        public WaitForSeconds waitTime = null;

        [Header("생기는 시간")]
        public float growTime;

        [Header("사라지는 시간")]
        public float fadeOutTime;

        public Color originalColor = Color.white;
        public Color hitColor = new Color(0.3f, 0.3f, 0.3f);

        [HideInInspector]
        public Vector3 startPosition;

        [HideInInspector]
        public Vector3 endPosition;

        [Space(10)]
        [Tooltip("글룸이 도약을 사용헀을 때, 덩쿨이 해당 위치에 존재할 경우 gloomAttackDelayTime 이후에 사망 처리 됩니다.")]
        public float gloomAttackDelayTime;

        [HideInInspector]
        public WaitForSeconds attackDelayTime = null;
    }

    [System.Serializable]
    public class Components
    {
        public Renderer renderer;

        [HideInInspector]
        public Material material = null;

        public Rigidbody rigidbody;
        public BoxCollider collider;
    }

    [System.Serializable]
    public class Effects
    {
        public GameObject thornSign;
    }

    public enum eState
    {
        Idle,
        Grow,
        Die
    }


    #endregion

    #region field
    [Header("현재 체력")]
    public int hp;

    [HideInInspector]
    public MapBlock currentBlock;

    [HideInInspector]
    public int currentIndex;

    [ReadOnly]
    [Header("현재 상태")]
    public eState currentState;

    [SerializeField]
    private Values _values = new Values();

    [SerializeField]
    private Components _components = new Components();

    [SerializeField]
    private Effects _effects = new Effects();

    public Values Val => _values;
    public Components Com => _components;
    public Effects Effect => _effects;

    [HideInInspector]
    public GloomController gloom;
    #endregion

    private IEnumerator hitCoroutine = null;
    private WaitForSeconds hitTime = new WaitForSeconds(0.1f);

    private string str_Amount = "_Amount";
    private string str_TexColor = "_TexColor";

    private int damage = 1;

    private Quaternion zeroRotation;

    private void Awake()
    {
        if (Com.material == null)
        {
            Com.material = Com.renderer.material;
        }
        Val.attackDelayTime = new WaitForSeconds(Val.gloomAttackDelayTime);

        //페이드아웃타임이 0보다 작으면 1로 설정
        if (Val.fadeOutTime <= 0f)
        {
            Val.fadeOutTime = 1f;
        }
        zeroRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Start()
    {
    }

    public void Init()
    {
        Com.material.SetFloat(str_Amount, 0f);
        Com.material.SetColor(str_TexColor, Val.originalColor);

        hitCoroutine = null;
        currentState = eState.Idle;
    }

    public void SetValues(MapBlock _block, int _index, int _hp, float _waitTime, Vector3 _startPos)
    {
        currentIndex = _index;
        currentBlock = _block;
        hp = _hp;
        Val.waitTime = new WaitForSeconds(_waitTime);

        Val.startPosition = _startPos;
        gameObject.transform.position = _startPos;

    }

    public void StartGrow()
    {
        currentBlock.SetCurrentType(MapBlock.eType.Used);
        StartCoroutine(ProcessGrow());
    }
    private IEnumerator ProcessGrow()
    {
        //딕셔너리에 들어가있지 않으면 추가
        if (!gloom.ContainsThornVineDict(currentIndex))
        {
            gloom.AddThornVineDict(currentIndex, this);
        }
        Com.collider.isTrigger = true;
        Com.collider.enabled = true;
        Quaternion testRotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f,280f), 0f));

        currentState = eState.Grow;

        float timer = 0f;
        float progress = 0f;

        Effect.thornSign.SetActive(true);

        //대기시간만큼 기다린 다음에 생성 시작
        yield return Val.waitTime;

        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / Val.growTime;

            Com.rigidbody.MovePosition(Vector3.Lerp(Val.startPosition, Val.endPosition, progress));
            Com.rigidbody.MoveRotation(Quaternion.Lerp(zeroRotation, testRotation, progress));

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Effect.thornSign.SetActive(false);
        Com.rigidbody.MovePosition(Val.endPosition);
        Com.collider.isTrigger = false;
        currentState = eState.Idle;
    }
    private IEnumerator ProcessHit()
    {
        Com.material.SetColor(str_TexColor, Val.hitColor);
        yield return hitTime;
        Com.material.SetColor(str_TexColor, Val.originalColor);

        hitCoroutine = null;
    }

    /// <summary>
    /// 일정시간 이후에 죽는 함수를 호출합니다.
    /// </summary>
    public void StartDelayDie()
    {
        StartCoroutine(DelayDie());
    }
    private IEnumerator DelayDie()
    {
        yield return Val.attackDelayTime;
        //대기 후 죽음
        StartCoroutine(ProcessDie());
    }
    private IEnumerator ProcessDie()
    {
        //죽은 상태로 변경
        currentState = eState.Die;

        //딕셔너리에 들어가있으면 삭제
        if (gloom.ContainsThornVineDict(currentIndex))
        {
            gloom.RemoveThornVineDict(currentIndex);
        }



        //충돌처리 끄기
        Com.collider.enabled = false;

        float amount = 0f;
        float timer = 0f;


        while (amount < 1f)
        {
            timer += Time.deltaTime / Val.fadeOutTime;
            amount = Mathf.Lerp(0f, 1f, timer);
            Com.material.SetFloat(str_Amount, amount);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        //원래 타입으로 돌리기
        currentBlock.SetCurrentTypeToOrigin();

        gloom.Pool.thornVine.ReleaseThis(this);

    }


    /// <summary>
    /// EndPosition을 새로 계산합니다.
    /// </summary>
    public void UpdateEndPosition()
    {
        Vector3 colSize = Com.collider.size;

        Val.endPosition = new Vector3(Val.startPosition.x,
            Val.startPosition.y + colSize.y,
            Val.startPosition.z);
    }
    public void ReceiveDamage()
    {
        hp -= damage;
    }

    public void OnHit()
    {
        if (currentState == eState.Idle && hp <= 1)
        {
            currentState = eState.Die;
            StartCoroutine(ProcessDie());
        }
        else
        {
            //코루틴이 실행 중이 아닐 때만 히트 코루틴 실행
            if (hitCoroutine == null)
            {
                hitCoroutine = ProcessHit();
                StartCoroutine(hitCoroutine);
            }

            //데미지 받는 처리
            ReceiveDamage();
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagName.Arrow))
        {
            // damage = other.GetComponent<ArrowBase>().damage;
            OnHit();
        }
    }
}

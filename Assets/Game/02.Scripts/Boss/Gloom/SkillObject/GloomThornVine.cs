using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(Rigidbody)))]
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
    #endregion

    private IEnumerator hitCoroutine = null;
    private WaitForSeconds hitTime = new WaitForSeconds(0.1f);

    private string str_Amount = "_Amount";
    private string str_TexColor = "_TexColor";
    private string str_Arrow = "Arrow";

    private int damage = 1;


    public void Init()
    {
        if (Com.material == null)
        {
            Com.material = Com.renderer.material;
        }
        Com.material.SetFloat(str_Amount, 0f);
        Com.material.SetColor(str_TexColor, Val.originalColor);

        Com.collider.enabled = true;
        hitCoroutine = null;

        currentState = eState.Idle;
    }
    public void SetValues(MapBlock _block, int _hp, float _waitTime, Vector3 _startPos)
    {
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

        Vector3 testRotation = new Vector3(0f, 300f, 0f);

        currentState = eState.Grow;

        float timer = 0f;
        float progress = 0f;

        //대기시간만큼 기다린 다음에 생성 시작
        yield return Val.waitTime;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / Val.growTime;
            Com.rigidbody.position = Vector3.Lerp(Val.startPosition, Val.endPosition, progress);
            Com.rigidbody.rotation = Quaternion.Euler((Vector3.Lerp(Vector3.zero, testRotation, progress)));

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        Com.rigidbody.position = Val.endPosition;
        currentState = eState.Idle;
    }
    private IEnumerator ProcessHit()
    {
        Com.material.SetColor(str_TexColor, Val.hitColor);
        yield return hitTime;
        Com.material.SetColor(str_TexColor, Val.originalColor);

        hitCoroutine = null;
    }
    private IEnumerator ProcessDie()
    {
        if (Val.fadeOutTime <= 0f)
        {
            Val.fadeOutTime = 1f;
        }

        //충돌처리 끄기
        Com.collider.enabled = false;

        currentState = eState.Die;
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

        CustomPoolManager.Instance.GetPool<GloomThornVine>().ReleaseThis(this);
    }


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
        if (currentState == eState.Idle && hp <= 0)
        {
            currentState = eState.Die;
            StartCoroutine(ProcessDie());
            return;
        }
        else
        {
            //코루틴이 실행 중이 아닐 때만 히트 코루틴 실행
            if (hitCoroutine == null)
            {
                hitCoroutine = ProcessHit();
                StartCoroutine(hitCoroutine);
            }
            ReceiveDamage();
        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(str_Arrow))
        {
            // damage = other.GetComponent<ArrowBase>().damage;
            OnHit();
        }
    }

}

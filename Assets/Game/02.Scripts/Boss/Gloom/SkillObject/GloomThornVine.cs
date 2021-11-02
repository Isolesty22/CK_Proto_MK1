using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomThornVine : MonoBehaviour, IDamageable
{

    #region Class

    [System.Serializable]
    public class Values
    {
        [Header("생기는 시간")]
        public float growTime;

        [Header("사라지는 시간")]
        public float fadeOutTime;

        public Color originalColor = Color.white;
        public Color hitColor = new Color(0.3f, 0.3f, 0.3f);
    }

    [System.Serializable]
    public class Components
    {
        public Renderer renderer;

        [HideInInspector]
        public Material material = null;
    }

    [System.Serializable]
    public class SkillObjects
    {
    }

    public enum eState
    {
        Idle,
        Grow,
        Die
    }


    #endregion
    [Header("현재 체력")]
    public int hp;

    [ReadOnly]
    [Header("현재 상태")]
    public eState currentState;

    [SerializeField]
    private Values _values = new Values();

    [SerializeField]
    private Components _components = new Components();

    [SerializeField]
    private SkillObjects _skillObjects = new SkillObjects();

    public Values Val => _values;
    public Components Com => _components;
    public SkillObjects Skills => _skillObjects;


    private IEnumerator hitCoroutine = null;
    private WaitForSeconds hitTime = new WaitForSeconds(0.1f);

    private string str_Amount = "_Amount";
    private string str_TexColor = "_TexColor";
    private string str_Arrow = "Arrow";

    private int damage = 1;
    

    private void Start()
    {
        Init();
        StartGrow();
    }
    public void Init()
    {
        if (Com.material == null)
        {
            Com.material = Com.renderer.material;
        }
        Com.material.SetFloat(str_Amount, 0f);
        Com.material.SetColor(str_TexColor, Val.originalColor);

        hitCoroutine = null;

        currentState = eState.Idle;
    }
    public void StartGrow()
    {
        StartCoroutine(ProcessGrow());
    }
    private IEnumerator ProcessGrow()
    {
        currentState = eState.Grow;
        yield return null;
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
        currentState = eState.Die;
        float amount = 0f;
        float timer = 0f;

        while (amount < 1f)
        {
            timer += Time.deltaTime / 1f;
            amount = Mathf.Lerp(0f, 1f, timer);
            Com.material.SetFloat(str_Amount, amount);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        CustomPoolManager.Instance.GetPool<GloomThornVine>().ReleaseThis(this);
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

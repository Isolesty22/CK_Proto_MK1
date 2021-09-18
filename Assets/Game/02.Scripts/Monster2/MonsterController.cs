using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    #region
    [Serializable]
    public enum MonsterState
    {
        WAIT,
        IDLE,
        MOVE,
        DETECT,
        ATTACK,
        DEATH
    }

    [Serializable]
    public class MonsterStatus
    {
        public string name;
        public int hp;
        public int maxHp;
        public float moveSpeed;


        [Header("Sub Status")]
        public bool isAlive;

        public float hitTime = 0.2f;
        public float fadeOutTime;
    }

    [Serializable]
    public class MonsterComponents
    {
        public Rigidbody rigidbody;
        public Collider collider;

        public GameObject monsterModel;
        public Renderer renderer;
        public Animator animator;

        public Color originalColor;
        public Color hitColor;
    }



    //field
    public MonsterState state;

    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();
    [SerializeField] private MonsterComponents components = new MonsterComponents();

    public MonsterStatus Stat => monsterStatus;
    public MonsterComponents Com => components;

    public IEnumerator hitColor;
    #endregion

    public virtual void Initialize()
    {
        Stat.hp = Stat.maxHp;
        Com.monsterModel.SetActive(false);
        state = MonsterState.WAIT;
        Com.originalColor = Com.renderer.material.color;
        Stat.isAlive = true;
    }

    public virtual void Awake()
    {
        Initialize();
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        State(state);
        HandleAnimation();
    }

    public virtual void State(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.WAIT:
                Search();
                break;

            case MonsterState.IDLE:
                Idle();
                break;

            case MonsterState.DETECT:
                Detect();
                break;

            case MonsterState.ATTACK:
                Attack();
                break;

            case MonsterState.MOVE:
                Move();
                break;

            case MonsterState.DEATH:
                Death();
                break;

            default:
                break;
        }
    }

    public virtual void ChangeState(MonsterState stateName)
    {
        switch (stateName)
        {
            case MonsterState.WAIT:
                state = MonsterState.WAIT;
                break;

            case MonsterState.IDLE:
                state = MonsterState.IDLE;
                break;

            case MonsterState.DETECT:
                state = MonsterState.DETECT;
                break;

            case MonsterState.ATTACK:
                state = MonsterState.ATTACK;
                break;

            case MonsterState.MOVE:
                state = MonsterState.MOVE;
                break;

            case MonsterState.DEATH:
                state = MonsterState.DEATH;
                break;

            default:
                break;
        }
    }

    protected virtual void Search()
    {

    }

    protected virtual void Idle()
    {
        
    }

    protected virtual void Detect()
    {
        
    }

    protected virtual void Move()
    {

    }

    protected virtual void Attack()
    {

    }

    public virtual void Hit(int damage)
    {
        if(hitColor != null)
            StopCoroutine(hitColor);

        hitColor = HitColor();
        StartCoroutine(hitColor);

        Stat.hp -= damage;

        if(Stat.hp <= 0)
        {
            ChangeState(MonsterState.DEATH);
        }
    }

    protected virtual void Death()
    {
        if(Stat.isAlive)
        {
            Stat.isAlive = false;
            Com.collider.enabled = false;
            Com.rigidbody.velocity = Vector3.zero;
            Com.rigidbody.useGravity = false;

            var dead = Dead();
            StartCoroutine(dead);
        }
    }

    IEnumerator HitColor()
    {
        Com.renderer.material.color = Com.hitColor;
        yield return new WaitForSeconds(Stat.hitTime);
        Com.renderer.material.color = Com.originalColor;
    }

    IEnumerator Dead()
    {
        var fadecolor = Com.renderer.material.color;

        float time = 0f;

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / Stat.fadeOutTime;
            fadecolor.a = Mathf.Lerp(255/255f, 0, time);
            Com.renderer.material.color = fadecolor;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    protected virtual void HandleAnimation()
    {
    }

}

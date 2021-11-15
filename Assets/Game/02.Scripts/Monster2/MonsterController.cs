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
        public bool isCanSpawnMon;

        public float hitTime = 0.2f;
        public float fadeOutTime;

        public float initDistance = 10f;
        public float respawnTime = 10f;
        public float audioVolume = 1;
        public float deathVolume = 0.5f;
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

        public Vector3 spawnPos;
        public AudioSource audio;
    }

    //field
    public MonsterState state;

    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();
    [SerializeField] private MonsterComponents components = new MonsterComponents();

    public MonsterStatus Stat => monsterStatus;
    public MonsterComponents Com => components;

    public IEnumerator hitColor;
    public bool playerOutOfRange;
    [HideInInspector] public bool inAttackCol;

    private float time;
    #endregion

    public virtual void Initialize()
    {
        Com.renderer.material.color = Color.white;
        Stat.hp = Stat.maxHp;
        Com.monsterModel.SetActive(false);
        state = MonsterState.WAIT;
        Com.originalColor = Com.renderer.material.GetColor("_TexColor");
        Com.renderer.material.SetColor("_TexColor", Com.originalColor);
        Stat.isAlive = true;
        transform.position = Com.spawnPos;
        Com.renderer.material.SetFloat("_Amount", 0);
        Com.audio.volume = Stat.audioVolume * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
    }

    public virtual void Awake()
    {
        Com.spawnPos = transform.position;
        Initialize();
    }
    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        State(state);
        HandleAnimation();

        if(playerOutOfRange)
        {
            CheckInit();
        }

        if (!Stat.isAlive && Stat.isCanSpawnMon)
        {
            time += Time.deltaTime;
            if(time >= Stat.respawnTime) // Respawn
            {
                Initialize();
                time = 0;
                if(playerOutOfRange == false)
                {
                    if (inAttackCol)
                    {
                        Com.monsterModel.SetActive(true);
                        ChangeState(MonsterController.MonsterState.DETECT);
                    }
                    else
                    {
                        Com.monsterModel.SetActive(true);
                        ChangeState(MonsterController.MonsterState.IDLE);
                    }
                }
            }
        }
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
        if (Stat.isAlive)
        {
            Com.audio.Stop();
            Stat.isAlive = false;
            Com.rigidbody.velocity = Vector3.zero;
            Com.rigidbody.useGravity = true;

            var dead = Dead();
            StartCoroutine(dead);
        }
    }

    IEnumerator HitColor()
    {
        Com.renderer.material.SetColor("_TexColor", Com.hitColor);
        yield return new WaitForSeconds(Stat.hitTime);
        Com.renderer.material.SetColor("_TexColor", Com.originalColor);
    }

    IEnumerator Dead()
    {
        float temp = Com.audio.volume;
        Com.audio.volume = Stat.deathVolume * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
        Com.audio.PlayOneShot(AudioManager.Instance.monsterDeath);
        var amount = 0f;

        float time = 0f;

        Com.renderer.material.SetColor("_TexColor", Com.originalColor);

        while (amount < 1f)
        {
            time += Time.deltaTime / Stat.fadeOutTime;
            amount = Mathf.Lerp(0, 1, time);
            Com.renderer.material.SetFloat("_Amount", amount);
            yield return null;
        }

        this.Com.monsterModel.SetActive(false);
    }

    protected virtual void HandleAnimation()
    {
    }


    public virtual void CheckInit()
    {
        var distance = GameManager.instance.playerController.transform.position.x - Com.spawnPos.x;
        if(Math.Abs(distance) > Stat.initDistance)
        {
            Initialize();
            playerOutOfRange = false;
        }
    }

    private float GetFloat(string _input)
    {
        return (float)System.Convert.ToDouble(_input);
    }
}

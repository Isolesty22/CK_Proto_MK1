using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        SEARCH,
        IDLE,
        MOVE,
        DETECT,
        ATTACK,
        DEATH
    }

    #region
    [Serializable]
    public class MonsterStatus : Status
    {
        public int mon_No;
        public string name_Kr;
        public int type_Move;
        public int type_Gimic;
        public float move_Speed;
        public int atk_Type;
        public float atk_Speed;
        public int atk_Range;
        public float respawn;
        public float agro_End_Time;
        public float fadeOutTime;
        public float destroyDistance;
    }

    [Serializable]
    public class Components
    {
        public Rigidbody rigidbody;
        public Collider collider;
        public Collider searchCol;
        public GameObject monsterModel;
    }

    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;

    public MonsterState state;

    public bool isAlive;
    public bool isRunninCo;
    public bool active;
    public IEnumerator hitColor;
    public Color originalColor;
    public float hitTime =0.2f;

    #endregion

    public virtual void Awake()
    {
        monsterStatus.Initialize();
        Com.monsterModel.SetActive(false);
        state = MonsterState.SEARCH;
        active = false;
        isAlive = true;
        Color originalColor = Com.monsterModel.GetComponent<Renderer>().material.color;
    }

    public virtual void Update()
    {
        if (active && isAlive)
            State(state);
    }

    public virtual void State(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.SEARCH:
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

    public virtual void ChangeState(string functionName)
    {
        if (functionName == "SEARCH")
        {
            state = MonsterState.SEARCH;
        }
        else if (functionName == "IDLE")
        {
            state = MonsterState.IDLE;
        }
        else if (functionName == "DETECT")
        {
            state = MonsterState.DETECT;
        }
        else if (functionName == "ATTACK")
        {
            state = MonsterState.ATTACK;
        }
        else if (functionName == "MOVE")
        {
            state = MonsterState.MOVE;
        }
        else if (functionName == "DEATH")
        {
            state = MonsterState.DEATH;
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
            ChangeState("DEATH");
        }
    }

    protected virtual void Death()
    {
        if(isAlive)
        {
            var dead = Dead();
            StartCoroutine(dead);
            isAlive = false;
        }
    }

    IEnumerator HitColor()
    {

        Com.monsterModel.GetComponent<Renderer>().material.color = new Color(150 / 255f, 150 / 255f, 150 / 255f, 255 / 255f);
        yield return new WaitForSeconds(hitTime);
        Com.monsterModel.GetComponent<Renderer>().material.color = originalColor;

    }

    IEnumerator Dead()
    {
        Com.collider.enabled = false;
        Com.rigidbody.velocity = Vector3.zero;
        Com.rigidbody.useGravity = false;
        Color fadecolor = Com.monsterModel.GetComponent<Renderer>().material.color;
        float time = 0f;

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / Stat.fadeOutTime;
            fadecolor.a = Mathf.Lerp(255/255f, 0, time);
            Com.monsterModel.GetComponent<Renderer>().material.color = fadecolor;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}

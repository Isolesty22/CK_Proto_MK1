using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        IDLE,
        MOVE,
        DETECT,
        ATTACK,
        HIT,
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

    public MonsterState state = MonsterState.IDLE;
    public MonsterState prevState = MonsterState.IDLE;

    public int damage;
    public bool isRunninCo;
    public bool active;
    #endregion

    public virtual void State(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.IDLE:
                Idle();
                break;

            case MonsterState.DETECT:
                Detect();
                break;

            case MonsterState.ATTACK:
                Attack();
                break;

            case MonsterState.HIT:
                Hit();
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
        if (functionName == "IDLE")
        {
            prevState = state;
            state = MonsterState.IDLE;
        }
        else if (functionName == "DETECT")
        {
            prevState = state;
            state = MonsterState.DETECT;
        }
        else if (functionName == "ATTACK")
        {
            prevState = state;
            state = MonsterState.ATTACK;
        }
        else if (functionName == "MOVE")
        {
            prevState = state;
            state = MonsterState.MOVE;
        }
        else if (functionName == "HIT")
        {
            prevState = state;
            state = MonsterState.HIT;
        }
        else if (functionName == "DEATH")
        {
            prevState = state;
            state = MonsterState.DEATH;
        }
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

    public virtual void Hit()
    {
        StartCoroutine(HitColor());
        if (Stat.hp <= damage)
            ChangeState("DEATH");
        else
        {
            Stat.hp -= damage;
            if (prevState == MonsterState.IDLE)
                ChangeState("IDLE");
            else if (prevState == MonsterState.DETECT)
                ChangeState("ATTACK");
            else if (prevState == MonsterState.ATTACK)
                ChangeState("ATTACK");
            else if (prevState == MonsterState.MOVE)
                ChangeState("IDLE");
        }
    }

    protected virtual void Death()
    {
        if(isRunninCo == false)
            StartCoroutine(Dead());
    }

    IEnumerator HitColor()
    {
        Color prevColor = Com.monsterModel.GetComponent<Renderer>().material.color;
        Com.monsterModel.GetComponent<Renderer>().material.color = new Color(150 / 255f, 150 / 255f, 150 / 255f, 255 / 255f);
        yield return new WaitForSeconds(0.2f);
        Com.monsterModel.GetComponent<Renderer>().material.color = prevColor;
    }

    IEnumerator Dead()
    {
        isRunninCo = true;
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
        isRunninCo = false;
        this.gameObject.SetActive(false);
    }
}

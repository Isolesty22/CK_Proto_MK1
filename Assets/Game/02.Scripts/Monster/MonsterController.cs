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
    }

    [Serializable]
    public class Components
    {
        public Rigidbody rigidbody;
        public Collider collider;
        public Collider searchCol;
    }

    [SerializeField] private Components components = new Components();
    [SerializeField] private MonsterStatus monsterStatus = new MonsterStatus();

    public Components Com => components;
    public MonsterStatus Stat => monsterStatus;

    public MonsterState state = MonsterState.IDLE;
    public MonsterState prevState = MonsterState.IDLE;

    public int damage;
    public bool isRunninCo;
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
        if (Stat.hp <= 1)
            ChangeState("DEATH");

        Stat.hp -= damage;
        ChangeState("IDLE");
    }

    protected virtual void Death()
    {
        this.gameObject.SetActive(false);
    }
}

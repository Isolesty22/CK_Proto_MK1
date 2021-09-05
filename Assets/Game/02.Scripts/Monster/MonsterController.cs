using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState
    {
        Search,
        Chase,
        Attack,
        Dead
    }

    #region
    [Serializable]
    public class MonsterStatus : Status
    {
        
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
    public MonsterState state = MonsterState.Search;

    public bool isRunninCo;
    #endregion

    protected virtual void Search()
    {
        
    }

    protected virtual void Chase()
    {
        
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Dead()
    {
        transform.parent.gameObject.SetActive(false);
    }
}

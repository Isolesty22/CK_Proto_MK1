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
        public CapsuleCollider collider;
        public CapsuleCollider searchCol;
    }

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
        gameObject.SetActive(false);
    }
}

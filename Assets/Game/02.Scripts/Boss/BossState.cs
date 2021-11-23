using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    /// <summary>
    /// exit해도 괜찮은 상태인가?
    /// </summary>
    public bool canExit = true;

    public IEnumerator currentCoroutine { get; protected set; }

    /// <summary>
    /// 상태가 시작할때 호출됨
    /// </summary>
    public virtual void OnEnter()
    {
        canExit = true;

    }

    /// <summary>
    /// (사용되지 않음)
    /// Monobehaviour Update에서 지속적으로 호출됨(사용되지 않음)
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// (사용되지 않음)
    /// Monobehaviour FixedUpdate에서 지속적으로 호출됨
    /// </summary>
    public virtual void OnFixedUpdate()
    {

    }
    /// <summary>
    /// 상태가 끝나고 다른 상태로 진입할때 호출됨
    /// </summary>
    public virtual void OnExit()
    {

    }
}

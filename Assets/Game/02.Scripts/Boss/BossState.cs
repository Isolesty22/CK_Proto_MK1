using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    /// <summary>
    /// 상태가 시작할때 호출됨
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// Monobehaviour Update에서 지속적으로 호출됨
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
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



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



#region BearState
public class BearState : BossState
{
    protected BearController bearController;
}

public class BearState_Idle : BearState
{
    public BearState_Idle(BearController _bearController)
    {
        bearController = _bearController;
    }
}

public class BearState_Doljin : BearState
{
    public BearState_Doljin(BearController _bearController)
    {
        bearController = _bearController;
    }
}
public class BearState_Halquigi_A : BearState
{
    public BearState_Halquigi_A(BearController _bearController)
    {
        bearController = _bearController;
    }
}
public class BearState_Halquigi_B : BearState
{
    public BearState_Halquigi_B(BearController _bearController)
    {
        bearController = _bearController;
    }
}
public class BearState_Phohyo : BearState
{
    public BearState_Phohyo(BearController _bearController)
    {
        bearController = _bearController;
    }
}
public class BearState_Naeryeochigi : BearState
{
    public BearState_Naeryeochigi(BearController _bearController)
    {
        bearController = _bearController;
    }
}


#endregion



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomState : BossState
{
    protected GloomController gloomController;
}

public class GloomState_Idle : GloomState
{
    public GloomState_Idle(GloomController _gloomController)
    {
        gloomController = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = true;
    }
}

public class GloomState_Obstruct : GloomState
{
    public GloomState_Obstruct(GloomController _gloomController)
    {
        gloomController = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}

public class GloomState_Chase : GloomState
{
    public GloomState_Chase(GloomController _gloomController)
    {
        gloomController = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}

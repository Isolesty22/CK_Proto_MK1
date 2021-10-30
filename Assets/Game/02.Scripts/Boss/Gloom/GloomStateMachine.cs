using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomStateMachine : BossStateMachine
{
    private GloomController gloomController;
    public GloomStateMachine(GloomController _gloomController)
    {
        gloomController = _gloomController;
    }

    public override void StartState(int _state)
    {
        base.StartState(_state);
    }
    public override void ChangeState(int _state)
    {
        base.ChangeState(_state);
    }

    public override BossState GetState(int _state)
    {
        eGloomState tempState = (eGloomState)_state;


        BossState bossState = tempState switch
        {
            eGloomState.None => new GloomState_Idle(gloomController),
            eGloomState.Idle  => new GloomState_Idle(gloomController),
            eGloomState.Obstruct => new GloomState_Obstruct(gloomController),
            eGloomState.Chase => new GloomState_Chase(gloomController),
            _ => new GloomState_Idle(gloomController)
        };

        return bossState;

    }
}

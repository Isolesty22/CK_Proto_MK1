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

            eGloomState.Idle => new GloomState_Idle(gloomController),

            eGloomState.Chase => new GloomState_Chase(gloomController),

            eGloomState.Leap => new GloomState_Leap(gloomController),

            eGloomState.Resonance => new GloomState_Resonance(gloomController),

            eGloomState.Threat => new GloomState_Threat(gloomController),

            eGloomState.ThornForest => new GloomState_ThornForest(gloomController),

            eGloomState.Obstruct => new GloomState_Obstruct(gloomController),

            eGloomState.ThornPath => new GloomState_ThornPath(gloomController),

            eGloomState.Wave => new GloomState_Wave(gloomController),

            eGloomState.Advance => new GloomState_Advance(gloomController),

            eGloomState.Berserk => new GloomState_Berserk(gloomController),

            eGloomState.Powerless => new GloomState_Powerless(gloomController),

            eGloomState.Die => new GloomState_Die(gloomController),

            _ => new GloomState_Idle(gloomController)
        };

        return bossState;

    }
}

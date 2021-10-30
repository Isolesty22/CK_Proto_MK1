using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStateMachine : BossStateMachine
{
    private BearController bearController;
    public BearStateMachine(BearController _bearController)
    {
        bearController = _bearController;
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
        eBearState tempState = (eBearState)_state;

        switch (tempState)
        {
            case eBearState.Idle:
                return new BearState_Idle(bearController);

            case eBearState.Stamp:
                return new BearState_Stamp(bearController);



            case eBearState.Roar_A:
            case eBearState.Roar_B:
                return new BearState_Roar(bearController);

            case eBearState.Strike_A:
            case eBearState.Strike_B:
                return new BearState_Strike(bearController);

            case eBearState.Claw_A:
            case eBearState.Claw_B:
                return new BearState_Claw(bearController);

            case eBearState.Smash:
                return new BearState_Smash(bearController);

            case eBearState.Concentrate:
                return new BearState_Concentrate(bearController);

            case eBearState.Powerless:
                return new BearState_Powerless(bearController);

            case eBearState.Rush:
                return new BearState_Rush(bearController);
           
            case eBearState.Die:
                return new BearState_Die(bearController);

            default:
                return new BearState_Idle(bearController);
        }
    }
}

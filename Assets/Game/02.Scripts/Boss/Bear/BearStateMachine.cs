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

    public override void StartState(eBossState _state)
    {
        base.StartState(_state);
    }
    public override void ChangeState(eBossState _state)
    {
        base.ChangeState(_state);
    }
    public override BossState GetState(eBossState _state)
    {

        switch (_state)
        {
            case eBossState.BearState_Idle:
                return new BearState_Idle(bearController);

            case eBossState.BearState_Stamp:
                return new BearState_Stamp(bearController);

            case eBossState.BearState_Rush:
                return new BearState_Rush(bearController);

            case eBossState.BearState_Roar_A:
            case eBossState.BearState_Roar_B:
                return new BearState_Roar(bearController);

            case eBossState.BearState_Strike_A:
            case eBossState.BearState_Strike_B:
            case eBossState.BearState_Strike_C:
                return new BearState_Strike(bearController);

            case eBossState.BearState_Claw_A:
            case eBossState.BearState_Claw_B:
            case eBossState.BearState_Claw_C:
                return new BearState_Claw(bearController);




            default:
                return new BearState_Idle(bearController);
        }
    }

}

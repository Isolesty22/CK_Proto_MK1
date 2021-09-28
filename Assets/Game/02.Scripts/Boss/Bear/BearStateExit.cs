using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStateExit : BearStateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bearController.OnAnimStateExit();
    }
}

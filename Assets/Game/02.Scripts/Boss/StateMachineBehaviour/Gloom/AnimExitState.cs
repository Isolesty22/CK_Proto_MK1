using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimExitState : GloomStateMachineBehaviour
{
    //애니메이션이 끝나서 다른 State로의 전환이 이루어질때 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gloomController.OnAnimStateExit();
    }
}

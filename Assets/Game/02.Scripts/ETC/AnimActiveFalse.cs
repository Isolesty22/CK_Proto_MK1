using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimActiveFalse : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}

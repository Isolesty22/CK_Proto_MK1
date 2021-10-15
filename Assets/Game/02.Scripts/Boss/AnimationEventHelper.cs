using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션 
/// </summary>
public class AnimationEventHelper : MonoBehaviour
{
    public BossController controller;

    public void SkillAction()
    {
        controller.SkillAction();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ִϸ��̼� 
/// </summary>
public class AnimationEventHelper : MonoBehaviour
{
    public BossController controller;

    public void SkillAction()
    {
        controller.SkillAction();
    }
}

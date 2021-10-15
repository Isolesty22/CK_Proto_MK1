using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerHelper : MonoBehaviour
{
    public BossController controller;

    public void SkillAction()
    {
        controller.SkillAction();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStrikeBehaviour : BearStateMachineBehaviour
{
    [Header("테스트용 스트라이크 큐브")]
    public GameObject strikeCube;
    public void Skill_Strike()
    {
        GameObject.Instantiate(strikeCube, bearController.bearMapInfo.bearBlocks[0].position.groundCenter, Quaternion.identity);
        GameObject.Instantiate(strikeCube, bearController.bearMapInfo.bearBlocks[2].position.groundCenter, Quaternion.identity);
        GameObject.Instantiate(strikeCube, bearController.bearMapInfo.bearBlocks[3].position.groundCenter, Quaternion.identity);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Skill_Strike();
    }
}

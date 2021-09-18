using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    public Animator animator;

    private BearStateMachine bearStateMachine;


    private void Awake()
    {
        Init();
        Debug.Log("Init 완료");
    }
    private void Start()
    {
        StartCoroutine(ProcessChangeStateTest());
    }
    private void Init()
    {
        bearStateMachine = new BearStateMachine(this);
        bearStateMachine.StartState(eBossState.BearState_Doljin);
    }

    IEnumerator ProcessChangeStateTest()
    {
        while (!bearStateMachine.CanExit())
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        bearStateMachine.ChangeState(eBossState.BearState_Idle);

    }
}

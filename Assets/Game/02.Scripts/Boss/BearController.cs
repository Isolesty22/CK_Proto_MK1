using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class BearController : MonoBehaviour
{
    public Animator animator;

    private BearStateMachine bearStateMachine;

    public List<eBossState> phaseList_01 = new List<eBossState>();
    private Queue<eBossState> phaseQueue_01 = new Queue<eBossState>();

    private void Awake()
    {
        Init();
        Debug.Log("Init 완료");
    }
    private void Init()
    {
        bearStateMachine = new BearStateMachine(this);
        bearStateMachine.StartState(eBossState.BearState_Idle);

        int length = phaseList_01.Count;
        for (int i = 0; i < length; i++)
        {
            phaseQueue_01.Enqueue(phaseList_01[i]);
        }
    }
    private void Start()
    {
        StartCoroutine(ProcessChangeStateTest());
    }


    private void Update()
    {
    }
    private bool ChangeState(eBossState _state)
    {
        if (!bearStateMachine.CanExit())
        {
            return false;
        }
        else
        {
            bearStateMachine.ChangeState(_state);
            return true;
        }

    }

    WaitForSecondsRealtime waitOneSec = new WaitForSecondsRealtime(1f);
    private IEnumerator ProcessChangeStateTest()
    {
        while (phaseQueue_01.Count > 0)
        {
            if (ChangeState(phaseQueue_01.Peek()))
            //성공적으로 변경되었으면
            {
                phaseQueue_01.Dequeue();
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Debug.Log("End Pattern!");
    }
}


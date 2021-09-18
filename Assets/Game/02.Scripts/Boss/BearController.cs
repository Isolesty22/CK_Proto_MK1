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
        bearStateMachine.StartState(eBossState.BearState_Idle);
    }

    IEnumerator ProcessChangeStateTest()
    {
        bearStateMachine.ChangeState(eBossState.BearState_Doljin);
        yield return new WaitForSecondsRealtime(3f);
        bearStateMachine.ChangeState(eBossState.BearState_Halquigi_A);
        yield return new WaitForSecondsRealtime(3f);
        bearStateMachine.ChangeState(eBossState.BearState_Halquigi_B);

    }
}

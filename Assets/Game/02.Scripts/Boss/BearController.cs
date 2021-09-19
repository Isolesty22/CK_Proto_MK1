using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class StateInfo
{
    [Tooltip("현재 페이즈")]
    public string phase = "";
    [Tooltip("현재 상태")]
    public string state = "";
}
public class BearController : MonoBehaviour
{
    public Animator animator;

    private BearStateMachine bearStateMachine;

    [Header("현재 상태")]
    public StateInfo stateInfo = new StateInfo();
    public List<eBossState> phaseList_01 = new List<eBossState>();
    private Queue<eBossState> phaseQueue_01 = new Queue<eBossState>();

    [Tooltip("애니메이터 파라미터")]
    public Dictionary<string, int> aniHash = new Dictionary<string, int>();

    private void Awake()
    {
        Init();
        Init_Animator();
        Debug.Log("Init 완료");
    }
    private void Init()
    {
        int length = phaseList_01.Count;
        for (int i = 0; i < length; i++)
        {
            phaseQueue_01.Enqueue(phaseList_01[i]);
        }
    }
    private void Init_Animator()
    {
        BearStateMachineBehaviour[] behaviours = animator.GetBehaviours<BearStateMachineBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].bearController = this;
        }

        AddAnimatorHash("Start_Idle");
        AddAnimatorHash("Start_Doljin");
        AddAnimatorHash("Start_Phohyo");
        AddAnimatorHash("Start_Halquigi_A");
    }

    private void Start()
    {
        bearStateMachine = new BearStateMachine(this);
        bearStateMachine.StartState(eBossState.BearState_Idle);
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
        stateInfo.phase = "Phase 01";

        while (phaseQueue_01.Count > 0)
        {
            if (ChangeState(phaseQueue_01.Peek()))
            //성공적으로 변경되었으면
            {
                stateInfo.state = phaseQueue_01.Peek().ToString();
                phaseQueue_01.Dequeue();
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Debug.Log("End Pattern!");
    }

    public void SetTrigger(string _paramName)
    {
        animator.SetTrigger(aniHash[_paramName]);
    }
    private void AddAnimatorHash(string _paramName)
    {
        aniHash.Add(_paramName, Animator.StringToHash(_paramName));
    }
    /// <summary>
    /// 현재 상태의 canExit를 설정합니다.
    /// </summary>
    public void SetCanExit(bool _canExit)
    {
        bearStateMachine.currentState.canExit = _canExit;
    }
}


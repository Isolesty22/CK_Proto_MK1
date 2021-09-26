using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class BearController : MonoBehaviour
{
    public Animator animator;


    private BearStateMachine bearStateMachine;

    #region Test용
    [Tooltip("현재 상태")]
    public StateInfo stateInfo = new StateInfo();

    [Serializable]
    public class TestTextMesh
    {
        public TextMesh stateText;
        public TextMesh phaseText;
        public TextMesh hpText;
    }
    public TestTextMesh testTextMesh;
    #endregion


    [Range(0, 100)]
    public float hp = 100f;
    public BossPhaseValue bossPhaseValue;

    [Header("패턴 목록")]
    public List<eBossState> phase_01_List = new List<eBossState>();
    private Queue<eBossState> phase_01_Queue = new Queue<eBossState>();

    public List<eBossState> phase_02_List = new List<eBossState>();
    private Queue<eBossState> phase_02_Queue = new Queue<eBossState>();

    public List<eBossState> phase_03_List = new List<eBossState>();
    private Queue<eBossState> phase_03_Queue = new Queue<eBossState>();

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
        int length = phase_01_List.Count;
        for (int i = 0; i < length; i++)
        {
            phase_01_Queue.Enqueue(phase_01_List[i]);
        }

        length = phase_02_List.Count;
        for (int i = 0; i < length; i++)
        {
            phase_02_Queue.Enqueue(phase_02_List[i]);
        }

        length = phase_03_List.Count;
        for (int i = 0; i < length; i++)
        {
            phase_03_Queue.Enqueue(phase_03_List[i]);
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
        bearStateMachine.isDebugMode = true;
        bearStateMachine.StartState(eBossState.BearState_Idle);
        StartCoroutine(ProcessChangeStateTest());
    }


    private void Update()
    {
        testTextMesh.stateText.text = stateInfo.state;
        testTextMesh.hpText.text = hp.ToString();
        testTextMesh.phaseText.text = stateInfo.phase;
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
        //해야함 : 반복되는 부분 정리하고, List 3개를 Queue로 만들어서 페이즈가 지날 때마다 디큐 시켜서 자동화하기
        bool thisPhase = true;
        int i = 0;
        int length = phase_01_List.Count;
        stateInfo.phase = "Phase 01";
        while (thisPhase)
        {
            i = i % length;
            if (ChangeState(phase_01_List[i]))
            {
                stateInfo.state = phase_01_List[i].ToString();
                Debug.Log("현재 인덱스 " + (i));
                i += 1;
                if (hp <= bossPhaseValue.phase2)
                {
                    thisPhase = false;
                }
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        thisPhase = true;
        i = 0;
        length = phase_02_List.Count;
        stateInfo.phase = "Phase 02";
        while (thisPhase)
        {
            i = i % length;
            if (ChangeState(phase_02_List[i]))
            {
                stateInfo.state = phase_02_List[i].ToString();
                Debug.Log("현재 인덱스 " + (i));
                i += 1;
                if (hp <= bossPhaseValue.phase3)
                {
                    thisPhase = false;
                }
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        thisPhase = true;
        i = 0;
        length = phase_03_List.Count;
        stateInfo.phase = "Phase 03";
        while (thisPhase)
        {
            i = i % length;
            if (ChangeState(phase_03_List[i]))
            {
                stateInfo.state = phase_03_List[i].ToString();
                Debug.Log("현재 인덱스 " + (i));
                i += 1;
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
    }
    private IEnumerator ProcessChangeStateTest_UseQueue()
    {

        stateInfo.phase = "Phase 01";
        stateInfo.state = eBossState.BearState_Idle.ToString();
        yield return waitOneSec;

        while (phase_01_Queue.Count > 0)
        {
            if (ChangeState(phase_01_Queue.Peek()))
            //성공적으로 변경되었으면
            {
                stateInfo.state = phase_01_Queue.Peek().ToString();
                phase_01_Queue.Dequeue();
                CheckChangePhase(2);
                yield return waitOneSec;
            }


            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        stateInfo.phase = "Phase 02";
        Debug.LogWarning("페이즈 2로 전환되었다!");
        while (phase_02_Queue.Count > 0)
        {
            if (ChangeState(phase_02_Queue.Peek()))
            //성공적으로 변경되었으면
            {
                stateInfo.state = phase_02_Queue.Peek().ToString();
                phase_02_Queue.Dequeue();
                CheckChangePhase(3);
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        stateInfo.phase = "Phase 03";
        Debug.LogWarning("페이즈 3으로 전환되었다!");
        while (phase_03_Queue.Count > 0)
        {
            if (ChangeState(phase_03_Queue.Peek()))
            //성공적으로 변경되었으면
            {
                stateInfo.state = phase_03_Queue.Peek().ToString();
                phase_03_Queue.Dequeue();
                yield return waitOneSec;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }


        //animator.Play("Halquigi_A.Start_Halguigi_A", 0, 0f);

        //while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Halquigi_A.Start_Halguigi_A"))
        //{
        //    yield return null;
        //}
        //Debug.Log("애니메이션 진입");

        //while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        //{
        //    yield return null;
        //}
        //Debug.Log("애니메이션 끝");

        //animator.CrossFade("Idle", 3f, 0);
        Debug.Log("End Pattern!");
        yield break;
    }

    private void CheckChangePhase(int _changePhase)
    {
        switch (_changePhase)
        {
            case 2:
                if (hp <= bossPhaseValue.phase2)
                {
                    phase_01_Queue.Clear();
                }
                break;

            case 3:
                if (hp <= bossPhaseValue.phase3)
                {
                    phase_02_Queue.Clear();
                }
                break;

            default:
                break;
        }
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

    public void AnimatorPlay(string _pathAndName)
    {
        animator.Play(_pathAndName, 0, 0f);
    }
}


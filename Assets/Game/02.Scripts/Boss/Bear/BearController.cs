using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class BearController : BossController
{
    public Animator animator;
    private BearStateMachine bearStateMachine;
    public BearMapInfo bearMapInfo;

    #region definitions
    [Serializable]
    public class Patterns
    {

        public List<BearPattern> phase_01_List = new List<BearPattern>();
        public List<BearPattern> phase_02_List = new List<BearPattern>();
        public List<BearPattern> phase_03_List = new List<BearPattern>();

        //public Queue<eBossState> phase_01_Queue = new Queue<eBossState>();
        //public Queue<eBossState> phase_02_Queue = new Queue<eBossState>();
        //public Queue<eBossState> phase_03_Queue = new Queue<eBossState>();
    }

    [Serializable]
    public class SkillObjects
    {
        public GameObject strikeCube;
        public GameObject roarCube;
    }

    #endregion

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

    public SkillObjects skillObjects;
    [Header("현재 체력")]
    [Range(0, 100)]
    public float hp = 100f;
    [Header("페이즈 전환 체력")]
    public BossPhaseValue bossPhaseValue;

    [Header("패턴 목록")]
    public Patterns patterns;

    [Tooltip("애니메이터 파라미터")]
    public Dictionary<string, int> aniHash = new Dictionary<string, int>();


    [HideInInspector]
    private List<List<BearPattern>> phaseList = new List<List<BearPattern>>();

    /// <summary>
    /// 스킬 액션
    /// </summary>
    private Action skillAction;

    private void Awake()
    {
        Init();
        Init_Animator();
        bearMapInfo.Init();
    }
    private void Init()
    {
        phaseList.Add(patterns.phase_01_List);
        phaseList.Add(patterns.phase_02_List);
        phaseList.Add(patterns.phase_03_List);
    }
    private void Init_Animator()
    {
        BearStateMachineBehaviour[] behaviours = animator.GetBehaviours<BearStateMachineBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].bearController = this;
        }

        AddAnimatorHash("Start_Idle");
        AddAnimatorHash("Start_Rush");
        AddAnimatorHash("Start_Roar");
        AddAnimatorHash("Start_Claw");
        AddAnimatorHash("Start_Strike");
        AddAnimatorHash("Phase");
        AddAnimatorHash("Start_Stamp");
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
        testTextMesh.phaseText.text = stateInfo.phase.ToString();
    }
    private bool ChangeState(eBossState _state)
    {
        if (_state == eBossState.BearState_Random)
        {
            bearStateMachine.ChangeState((eBossState)UnityEngine.Random.Range(0, (int)eBossState.BearState_Random));
        }
        else
        {
            bearStateMachine.ChangeState(_state);

        }
        return false;
    }

    private bool CanChangeState()
    {
        return bearStateMachine.CanExit();
    }

    /// <summary>
    /// 다음 페이즈로 가기 위한 체력을 반환해줍니다.
    /// </summary>
    private float GetNextPhaseHP(ePhase _currentPhase)
    {
        switch (_currentPhase)
        {
            case ePhase.Phase_1:
                return bossPhaseValue.phase2;

            case ePhase.Phase_2:
                return bossPhaseValue.phase3;

            case ePhase.Phase_3:
                return 0;

            default:
                return 0;
        }
    }
    private void GoNextPhase()
    {
        //현재 페이즈에 1 추가

    }
    WaitForSecondsRealtime waitOneSec = new WaitForSecondsRealtime(1f);
    BearPattern currentPattern;
    private IEnumerator ProcessChangeStateTest()
    {
        //해야함 : 반복되는 부분 정리하고, List 3개를 Queue로 만들어서 페이즈가 지날 때마다 디큐 시켜서 자동화하기
        stateInfo.phase = ePhase.Phase_1;
        int i = 0;
        int length = phaseList[stateInfo].Count;

        while (true)
        {
            if (CanChangeState()) //패턴을 바꿀 수 있는 상태라면
            {
                //페이즈를 전환 체크
                if (hp <= GetNextPhaseHP(stateInfo.phase))
                {
                    //페이즈 전환
                    //GoNextPhase(); //빈 함수임

                    if (stateInfo.phase == ePhase.Phase_3)
                    {
                        break;
                    }
                    stateInfo.phase = stateInfo.phase + 1;
                    i = 0;
                    length = phaseList[stateInfo].Count;
                }

                i = i % length;

                //다음 패턴 가져오기
                currentPattern = phaseList[stateInfo][i];

                //대기 시간동안 기다림
                yield return new WaitForSeconds(currentPattern.waitTime);

                //스테이트 변경
                ChangeState(currentPattern.state);
                stateInfo.state = bearStateMachine.GetCurrentStateName();

                i += 1;

                yield return null;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        this.gameObject.SetActive(false);

    }
    public void SetTrigger(string _paramName)
    {
        animator.SetTrigger(aniHash[_paramName]);
    }
    /// <summary>
    /// 현재 상태의 canExit를 설정합니다.
    /// </summary>
    public void SetCanExit(bool _canExit)
    {
        bearStateMachine.currentState.canExit = _canExit;
    }
    public void SetSkillAction(Action _action)
    {
        skillAction = null;
        skillAction += () => Debug.Log("SkillAction!");
        skillAction += _action;
    }
    public void SkillAction()
    {
        skillAction();
    }

    #region Animation 관련
    private void AddAnimatorHash(string _paramName)
    {
        aniHash.Add(_paramName, Animator.StringToHash(_paramName));
    }
    public void AnimatorPlay(string _pathAndName)
    {
        animator.Play(_pathAndName, 0, 0f);
    }
    public void OnAnimStateExit()
    {
        bearStateMachine.currentState.canExit = true;
    }
    #endregion
}
[Serializable]
public struct BearPattern
{
    [Tooltip("대기 시간")]
    public float waitTime;

    [Tooltip("실행할 패턴")]
    public eBossState state;
}
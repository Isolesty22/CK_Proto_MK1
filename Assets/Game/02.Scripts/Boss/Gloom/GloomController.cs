using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomController : BossController
{
    [Header("이동 시 사용하는 강체")]
    [Tooltip("글룸은 이동할 때 트랜스폼을 사용하지 않고, \n리지드바디를 사용합니다.")]
    public Rigidbody myRigidbody;

    [Header("페이즈가 전환되는 HP")]
    public BossPhaseValue bossPhaseValue;





    private List<List<GloomPattern>> phaseList = new List<List<GloomPattern>>();
    [HideInInspector]
    public GloomPattern currentPattern;
    private void Awake()
    {
        //데미지 받았을때 할 행동 
        OnHitHandler = () => { };
    }
    private void Start()
    {
        OnTimelineEnded();
        //GameManager.instance.timelineManager.OnTimelineEnded += OnTimelineEnded;
    }

    private void OnTimelineEnded()
    {
        animator.runtimeAnimatorController = runtimeAnimator;

        Init();
        StartCoroutine(ExecutePatternCoroutine);
    }

    protected override void Init()
    {
        base.Init();
        bossPhaseValue.Init(hp);
        stateMachine = new GloomStateMachine(this);
        stateMachine.StartState((int)eGloomState.Idle);

        ExecutePatternCoroutine = ExecutePattern();
        Init_Animator();
    }
    private void Init_Animator()
    {
        GloomStateMachineBehaviour[] behaviours = animator.GetBehaviours<GloomStateMachineBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].gloomController = this;
        }

        int paramCount = animator.parameterCount;
        AnimatorControllerParameter[] aniParam = animator.parameters;

        for (int i = 0; i < paramCount; i++)
        {
            AddAnimatorHash(aniParam[i].name);
        }

        skillVarietyBlend = aniHash[str_SkillVarietyBlend];

    }



    private int currentIndex = 0;
    private IEnumerator ExecutePattern()
    {
        stateInfo.phase = ePhase.Phase_1;
        currentIndex = 0;
        int length = phaseList[stateInfo].Count;
        currentPattern = new GloomPattern();

        while (true)
        {
            if (stateMachine.CanExit()) //패턴을 바꿀 수 있는 상태라면
            {
                //페이즈 전환 체크
                if (hp <= GetNextPhaseHP(stateInfo.phase))
                {
                    //체력이 0이하면 break;
                    if (hp <= 0)
                    {
                        break;
                    }

                    ProcessChangePhase(stateInfo.phase);
                    length = phaseList[stateInfo].Count;
                    //continue;
                }
                else
                {
                    //대기 시간동안 기다림
                    yield return new WaitForSeconds(currentPattern.waitTime);

                    //다음 패턴 가져오기
                    SetCurrentPattern(phaseList[stateInfo][currentIndex]);

                    //스테이트 변경
                    ChangeState((int)currentPattern.state);

                    currentIndex += 1;
                    currentIndex = currentIndex % length;
                }
                yield return null;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        SetStateInfo((int)eGloomState.Die);
        ChangeState((int)eGloomState.Die);
        //testPotal.Active();
    }


    /// <summary>
    /// 페이즈가 전환될 때 호출됩니다.
    /// </summary>
    private void ProcessChangePhase(ePhase _phase)
    {
        switch (_phase)
        {
            case ePhase.Phase_1:
                //myTransform.SetPositionAndRotation(bearMapInfo.phase2Position, Quaternion.Euler(Vector3.zero));
                //myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                //투사체 위치 다시 계산
                //bearMapInfo.leftPadding = 0;
                //bearMapInfo.rightPadding = bearMapInfo.paddingSize;
                //bearMapInfo.Init_Projectiles();
                ChangeState((int)eGloomState.Berserk);
                break;

            default:
                break;
        }
        stateInfo.phase += 1;
        currentIndex = 0;

    }


    private float GetNextPhaseHP(ePhase _currentPhase)
    {
        switch (_currentPhase)
        {
            case ePhase.Phase_1:
                return bossPhaseValue.phase2Hp;

            default:
                return 0;
        }
    }
    private void SetCurrentPattern(GloomPattern _pattern)
    {
        currentPattern = _pattern;

        //if (currentPattern.state == eBossState.BearState)
        //{
        //    currentPattern.state = GetRandomState(stateInfo.phase);

        //}
    }


    public override void ChangeState(int _state)
    {
        base.ChangeState(_state);
    }
    public override string GetStateToString(int _state)
    {
        return base.GetStateToString(_state);
    }

}


[Serializable]
public struct GloomPattern
{
    [Tooltip("실행할 패턴")]
    public eGloomState state;

    [Tooltip("실행 후 대기 시간")]
    public float waitTime;
}

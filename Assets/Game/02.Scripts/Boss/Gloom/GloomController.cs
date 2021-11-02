using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomController : BossController
{

    #region definitions---
    [Serializable]
    public class Components
    {
        [Header("이동 시 사용하는 강체")]
        [Tooltip("글룸은 이동할 때 트랜스폼을 사용하지 않고, \n리지드바디를 사용합니다.")]
        public Rigidbody rigidbody;
        public GloomMap gloomMap;
    }

    [Serializable]
    public class Patterns
    {
        public List<GloomPattern> phase_01_List = new List<GloomPattern>();
        public List<GloomPattern> phase_02_List = new List<GloomPattern>();
    }
    [Serializable]
    public class Pools
    {
        public CustomPool<GloomThornVine> thornVine = new CustomPool<GloomThornVine>();
    }
    [Serializable]
    public class SkillObjects
    {
        [Header("방해 발사 위치")]
        public Transform[] obstructTransforms;

        [HideInInspector]
        public Vector3[] obstructPositions;
    }
    [Serializable]
    public class SkillValues
    {
        #region Struct

        [Serializable]
        public struct ThornPattern
        {
            [Tooltip("가시덩쿨은 hp 만큼의 체력을 갖습니다.")]
            public int hp;
            [Tooltip("이펙트 발생 이후 waitTime만큼 대기하고 덩쿨을 생성합니다.")]
            public float waitTime;
        }
        #endregion

        public ThornPattern thornPattern;
    }

    #endregion


    [Header("페이즈가 전환되는 HP")]
    public BossPhaseValue bossPhaseValue;

    [Header("스킬 세부 값")]
    [SerializeField]
    private SkillValues _skillValues;

    [Header("패턴")]
    [SerializeField]
    private Patterns _patterns;


    [Header("ETC")]

    [Tooltip("현재 보스의 위치")]
    [ReadOnly]
    public eDiretion diretion;

    [SerializeField]
    private Components _components;

    [SerializeField]
    private SkillObjects _skillObjects;


    [HideInInspector]
    public GloomPattern currentPattern;
    public Patterns patterns => _patterns;
    public SkillObjects SkillObj => _skillObjects;
    public Components Com => _components;

    public SkillValues SkillVal => _skillValues;

    [HideInInspector]
    public Pools Pool;

    private List<List<GloomPattern>> phaseList = new List<List<GloomPattern>>();

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

        //패턴 관련 초기화
        phaseList.Add(patterns.phase_01_List);
        phaseList.Add(patterns.phase_02_List);

        bossPhaseValue.Init(hp);

        ExecutePatternCoroutine = ExecutePattern();

        stateMachine = new GloomStateMachine(this);
        stateMachine.isDebugMode = true;
        stateMachine.StartState((int)eGloomState.Idle);

        ChangeDirection(eDiretion.Right);

        Init_Animator();
        Init_Pools();
        Init_Skills();
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

    private void Init_Pools()
    {
        Pool.thornVine = CustomPoolManager.Instance.CreateCustomPool<GloomThornVine>();
    }

    private void Init_Skills()
    {
        UpdateObstructPositions();
    }

    /// <summary>
    /// 방해 스킬의 위치를 재설정합니다.
    /// </summary>
    public void UpdateObstructPositions()
    {
        int length = SkillObj.obstructTransforms.Length;
        SkillObj.obstructPositions = new Vector3[length];
        for (int i = 0; i < length; i++)
        {
            SkillObj.obstructPositions[i] = SkillObj.obstructTransforms[i].localPosition;
        }
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

    }


    /// <summary>
    /// 페이즈가 전환될 때 호출됩니다.
    /// </summary>
    private void ProcessChangePhase(ePhase _phase)
    {
        switch (_phase)
        {
            case ePhase.Phase_1:

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
    }


    public override void ChangeState(int _state)
    {
        base.ChangeState(_state);
    }

    /// <summary>
    /// 보스의 방향을 바꿉니다. (왼쪽/오른쪽)
    /// </summary>
    public void ChangeDirection(eDiretion _direction)
    {
        diretion = _direction;
        Com.gloomMap.ChangeDirection(_direction);
    }

    public enum eUsableBlockMode
    {
        /// <summary>
        /// length만 고려된 상태입니다. 
        /// </summary>
        Default,

        /// <summary>
        /// Vine이 자라는 위치를 제외합니다.
        /// </summary>
        ExcludeVine
    }

    /// <summary>
    /// 사용할 수 있는 mapBlock의 인덱스를 리턴합니다.
    /// </summary>
    public List<int> GetUsableBlockList(eUsableBlockMode _mode)
    {
        List<int> tempList = new List<int>();

        switch (_mode)
        {
            case eUsableBlockMode.Default:

                for (int i = Com.gloomMap.mapLength.min; i < Com.gloomMap.mapLength.max; i++)
                {
                    tempList.Add(i);
                }

                break;

            case eUsableBlockMode.ExcludeVine:
                for (int i = Com.gloomMap.mapLength.min; i < Com.gloomMap.mapLength.max; i++)
                {
                    //사용 중인 블록이라면
                    if (Com.gloomMap.mapBlocks[i].currentType == MapBlock.eType.Used)
                    {
                        continue;
                    }

                    tempList.Add(i);
                }
                break;

            default:
                break;
        }

        return tempList;
    }

    /// <summary>
    /// 해당 블록의 타입을 지정합니다.
    /// </summary>
    public void SetBlockTypeToOrigin(int _index)
    {
        Com.gloomMap.mapBlocks[_index].SetCurrentTypeToOrigin();

    }

    /// <summary>
    /// 해당 블록의 타입을 지정합니다.
    /// </summary>
    public void SetBlockType(int _index, MapBlock.eType _type)
    {
        Com.gloomMap.mapBlocks[_index].SetCurrentType(_type);

    }

    /// <summary>
    /// 해당 블록의 타입을 반환합니다.
    /// </summary>
    public MapBlock.eType GetBlockType(int _index, MapBlock.eType _type)
    {
        return Com.gloomMap.mapBlocks[_index].currentType;
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

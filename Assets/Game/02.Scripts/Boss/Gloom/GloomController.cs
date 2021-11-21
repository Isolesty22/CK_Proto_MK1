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
        public Rigidbody rigidbody;
        public GloomMap gloomMap;
        public EmissionHelper emissionHelper;

        [Header("[Colliders]")]

        [Tooltip("보스에 붙어있는 콜라이더")]
        public BoxCollider bodyCollider;

        [Tooltip("플레이어 못 넘어가게하려는 콜라이더")]
        public BoxCollider wallCollider;
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
        public CustomPool<GloomObstructBullet> obstructBullet = new CustomPool<GloomObstructBullet>();
        public CustomPool<GloomObstructSign> obstructSign = new CustomPool<GloomObstructSign>();
        public CustomPool<GloomWaveBullet> waveBullet = new CustomPool<GloomWaveBullet>();
        public CustomPool<GloomChaseBullet> chaseBullet = new CustomPool<GloomChaseBullet>();
        public CustomPool<GloomChaseHit> chaseHit = new CustomPool<GloomChaseHit>();

    }
    [Serializable]
    public class SkillObjects
    {
        public GameObject whiskers;

        [Tooltip("불꽃 추아악")]
        public GloomLeapImpact leapImpact;

        [Tooltip("번개 좌자작")]
        public GloomLightning gloomLightning;

        public GameObject threat;

        [Header("방해 발사 위치")]
        public Transform[] obstructTransforms;

        [Header("파동 발사 위치")]
        public Transform waveTransform;

        [Header("패링 헬퍼")]
        [Tooltip("공명 스킬의 패링 성공여부를 확인하기 위해 사용합니다.")]
        public HeadParryingHelper resonanceHelper;

        [Header("구슬 트랜스폼")]
        public Transform sphereTransform;

        [Header("공명 구슬 이펙트")]
        public GameObject resonanceSphere;

        [Header("광폭화 이펙트")]
        public GloomBerserk berserk;

    }
    [Serializable]
    public class SkillValues
    {
        #region Struct

        [Serializable]
        public struct ChasePattern
        {
            [Tooltip("투사체를 count개 만큼 발사합니다.")]
            public int count;

            [Tooltip("투사체가 생성되는 간격입니다.")]
            public float intevar;

            [Tooltip("투사체가 목표물에게 이동할 때까지 걸리는 시간입니다.")]
            public float moveTime;

            [Space(5)]

            [Tooltip("투사체 이동 시, curvedValue만큼 더 굽은 선을 그리게 됩니다. 0에 가까울수록 직선이 됩니다.")]
            public float curvedValue;

            /// <summary>
            /// 투사체의 midPosition은 이 값을 사용하여 계산됩니다.
            /// </summary>
            [HideInInspector]
            public Vector3 curvedPosition;
        }

        [Serializable]
        public struct ThornPattern
        {
            [Tooltip("가시덩쿨은 hp 만큼의 체력을 갖습니다.")]
            public int hp;
            [Tooltip("이펙트 발생 이후 waitTime만큼 대기하고 덩쿨을 생성합니다.")]
            public float waitTime;
        }
        [Serializable]
        public struct LeapPattern
        {

            [Header("[Leap]")]

            [Tooltip("점프 시 위로 상승하는 시간입니다. 값이 적을수록 더 빠르게 상승합니다.")]
            public float upTime;
            [Tooltip("착지 시 아래로 하강하는 시간입니다. 값이 적을수록 더 빠르게 하강합니다.")]
            public float downTime;
            [Tooltip("착지 애니메이션의 실행 시간입니다. 착지가 끝나기 downAnimTime초 전에 애니메이션이 실행됩니다.")]
            public float downAnimTime;
            [Tooltip("화면 밖으로 상승하기 위해 더하는 값입니다.")]
            public float upPosValue;

            [Header("[LeapImpact]")]

            [Tooltip("리프임팩트의 지속 시간입니다.")]
            public float leapImpactDuration;
        }

        [Serializable]
        public struct ObstructPattern
        {
            [Tooltip("투사체의 이동에 참조하는 커브입니다.")]
            public AnimationCurve curve;

            [Tooltip("투사체의 생성 간격입니다.")]
            public float createInterval;
            [Tooltip("투사체가 생성된 후 waitTime만큼 대기 후 이동을 시작합니다.")]
            public float waitTime;
            [Tooltip("투사체가 맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
            public float moveTime;

            [HideInInspector]
            public Vector3[] positions;
        }
        [Serializable]
        public struct WavePattern
        {
            [Tooltip("투사체가 맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
            public float moveTime;
            [Tooltip("오락가락~파동의 속도입니다. 값이 높을수록 오락가락의 속도가 빨라집니다.")]
            public float frequency;
            [Tooltip("파동의 크기입니다. 값이 높을수록 오락가락의 정도가 심해집니다.")]
            public float magnitude;

            [HideInInspector]
            public Vector3 startPosition;
        }

        [Serializable]
        public struct ResonancePattern
        {
            [Header("[Resonance]")]
            [Tooltip("공명의 지속시간입니다.")]
            public float resonanceTime;


            [Tooltip("투사체의 생성 간격입니다.")]
            public float createInterval;

            [Tooltip("투사체가 맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
            public float moveTime;

            [Tooltip("뿔 사이의 구슬이 커지는 정도입니다.")]
            public float sphereScale;


            [Header("[Powerless]")]
            [Tooltip("무력화의 지속시간입니다.")]
            public float powerlessTime;
        }
        [Serializable]
        public struct AdvancePattern
        {
            [Tooltip("맵 끝으로 이동할 때까지 걸리는 시간입니다.")]
            public float moveTime;

        }

        #endregion

        public ChasePattern chase;
        public LeapPattern leap;
        public ThornPattern thorn;
        public ObstructPattern obstruct;
        public WavePattern wave;
        public ResonancePattern resonance;
        public AdvancePattern advance;

        [Space(5)]
        [Tooltip("맵 끝에서 사라져야하는 투사체들은, 실제 맵 사이즈에서 extendMapSize만큼 추가된 위치에서 사라지게 됩니다.")]
        public float extendMapSize;

        [HideInInspector]
        [Tooltip("맵 끝에서 사라져야하는 투사체들은 endPos에 해당 값을 더해야합니다.")]
        public Vector3 extendEndPos;
    }

    [Serializable]
    public class AudioClips
    {
        public AudioClip death;
        public AudioClip fire;
        public AudioClip fire2;
        public AudioClip jump;
        public AudioClip land;
        public AudioClip lightning;
        public AudioClip longHowling;
        public AudioClip shorHowling;
        public AudioClip magic;
        public AudioClip magicArrow;
        public AudioClip resonanace;
        public AudioClip scratchGround;
        public AudioClip thorn;
        public AudioClip walk;
        public AudioClip weild;
        public AudioClip chase;
        public AudioClip resonanceArrow;
        public AudioClip parrying;
        public AudioClip wave;
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


    [Header("----------ETC")]

    [Tooltip("현재 보스의 위치")]
    [ReadOnly]
    public eDirection diretion;

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

    public AudioClips audioClips;

    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public Pools Pool;

    /// <summary>
    /// int = 소환될 당시의 인덱스 값
    /// </summary>
    public Dictionary<int, GloomThornVine> aliveThornVineDict = new Dictionary<int, GloomThornVine>();

    private List<List<GloomPattern>> phaseList = new List<List<GloomPattern>>();


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 1 * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
        Init_Talk();
        OnTimelineEnded();
        //GameManager.instance.timelineManager.OnTimelineEnded += OnTimelineEnded;
    }

    private void OnTimelineEnded()
    {
        GameManager.instance.timelineManager.onTimelineEnded -= OnTimelineEnded;
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
        maxHp = hp;

        ExecutePatternCoroutine = ExecutePattern();

        stateMachine = new GloomStateMachine(this);
        //stateMachine.isDebugMode = true;
        stateMachine.StartState((int)eGloomState.Idle);

        ChangeDirection(eDirection.Right);

        onHitAction = OnHit;

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
        Pool.obstructBullet = CustomPoolManager.Instance.CreateCustomPool<GloomObstructBullet>();
        Pool.obstructSign = CustomPoolManager.Instance.CreateCustomPool<GloomObstructSign>();
        Pool.waveBullet = CustomPoolManager.Instance.CreateCustomPool<GloomWaveBullet>();
        Pool.chaseBullet = CustomPoolManager.Instance.CreateCustomPool<GloomChaseBullet>();
        Pool.chaseHit = CustomPoolManager.Instance.CreateCustomPool<GloomChaseHit>();
    }

    private void Init_Skills()
    {

        SkillVal.extendEndPos = new Vector3(SkillVal.extendMapSize, 0f, 0f);
        SkillVal.chase.curvedPosition = new Vector3(0f, SkillVal.chase.curvedValue, 0f);

        SkillObj.resonanceHelper.Init();

        SkillObj.gloomLightning.gameObject.SetActive(true);
        SkillObj.gloomLightning.Init();
        SkillObj.gloomLightning.Create();
        SkillObj.gloomLightning.gameObject.SetActive(false);
        UpdateObstructPositions();
        UpdateWavePosition();
        SkillObj.threat.SetActive(false);

    }

    private void Init_Talk()
    {
        for (int i = 400; i <= 411; i++)
        {
            int CODE = i;
            talkDict.Add(CODE, () => UIManager.Instance.Talk(CODE, 2f));
        }
    }

    /// <summary>
    /// 방해 스킬의 위치를 재설정합니다.
    /// </summary>
    public void UpdateObstructPositions()
    {
        int length = SkillObj.obstructTransforms.Length;
        SkillVal.obstruct.positions = new Vector3[length];
        for (int i = 0; i < length; i++)
        {
            SkillVal.obstruct.positions[i] = SkillObj.obstructTransforms[i].position;
        }
    }

    /// <summary>
    /// 파동 스킬의 위치를 재설정합니다.
    /// </summary>
    public void UpdateWavePosition()
    {
        SkillVal.wave.startPosition = SkillObj.waveTransform.position;
    }


    private int currentIndex = 0;
    private IEnumerator ExecutePattern()
    {
        if (diretion == eDirection.Right)
        {
            myTransform.position = Com.gloomMap.gloomPos_Right.position;
        }
        else
        {
            myTransform.position = Com.gloomMap.gloomPos_Left.position;
        }
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
    public void ChangeDirection(eDirection _direction)
    {
        diretion = _direction;
        Com.gloomMap.ChangeDirection(_direction);
        UpdateObstructPositions();
        UpdateWavePosition();
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
                for (int i = Com.gloomMap.index.min; i < Com.gloomMap.index.max; i++)
                {
                    tempList.Add(i);
                }
                break;

            case eUsableBlockMode.ExcludeVine:
                for (int i = Com.gloomMap.index.min; i < Com.gloomMap.index.max; i++)
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
    /// 체력이 20퍼가 될 때까지 기다리다가 대사를 출력합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitHpPer20()
    {
        while (true)
        {
            if (hp / maxHp <= 0.2f)
            {
                TalkOnce(410);

                yield break;
            }
            yield return null;
        }
    }


    /// <summary>
    /// 해당 블록의 타입을 지정합니다.
    /// </summary>
    public void SetBlockTypeToOrigin(int _index) => Com.gloomMap.mapBlocks[_index].SetCurrentTypeToOrigin();

    /// <summary>
    /// 해당 블록의 타입을 지정합니다.
    /// </summary>
    public void SetBlockType(int _index, MapBlock.eType _type) => Com.gloomMap.mapBlocks[_index].SetCurrentType(_type);

    /// <summary>
    /// 해당 블록의 타입을 반환합니다.
    /// </summary>
    public MapBlock.eType GetBlockType(int _index, MapBlock.eType _type) => Com.gloomMap.mapBlocks[_index].currentType;

    public override string GetStateToString(int _state) => ((eGloomState)_state).ToString();

    public void AddThornVineDict(int _index, GloomThornVine _thornVine) => aliveThornVineDict.Add(_index, _thornVine);

    public void RemoveThornVineDict(int _index) => aliveThornVineDict.Remove(_index);

    public bool ContainsThornVineDict(int _index) => aliveThornVineDict.ContainsKey(_index);


    private Action onHitAction = null;
    public override void OnHit()
    {
        ReceiveDamage();
        Com.emissionHelper.OnHit();
    }

    /// <summary>
    /// 무적 상태가 됩니다.
    /// </summary>
    public void StartInvincible()
    {
        onHitAction = VoidFunc;
    }

    /// <summary>
    /// 무적 상태를 종료합니다.
    /// </summary>
    public void EndInvincible()
    {
        onHitAction = OnHit;
    }

    /// <summary>
    /// 그냥 빈 함수입니다.
    /// </summary>
    private void VoidFunc() { }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Arrow))
        {
            onHitAction();
        }
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

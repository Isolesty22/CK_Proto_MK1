using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

[SelectionBase]
public class BearController : BossController
{

    [Header("임시 포탈")]
    public TestStagePotal testPotal;
    [Space(20)]
    public BearMapInfo bearMapInfo;

    #region definitions

    [Serializable]
    public class Patterns
    {
        public List<BearPattern> phase_01_List = new List<BearPattern>();
        public List<BearPattern> phase_02_List = new List<BearPattern>();
    }

    [Serializable]
    public class Colliders
    {
        public BoxCollider headCollider;
        public BoxCollider bodyCollider;

        [Tooltip("보스가 죽은 뒤에 활성화될 콜라이더")]
        public BoxCollider groundCollider;
        public Vector3 headColliderSize;
        public Vector3 bodyColliderSize;
    }

    [Serializable]
    public class SkillObjects
    {
        public GameObject strikeCube;
        public GameObject roarEffect;
        public GameObject roarGroundEffect;

        [Space(5)]
        public GameObject claw_A_Effect;
        public GameObject claw_B_Effect;
        public Transform clawUnderPosition;

        [Space(5)]
        public Transform headParringPosition;
        public HeadParryingHelper concentrateHelper;
        public GameObject concentrateSphere;
        public ParticleSystem stunEffect;

        [Space(5)]
        public SmashHelper smashHelper;
        public Transform handTransform;

        [Space(5)]
        public GameObject rushEffect;
        public Texture hurtTex;
        public RushSpiderHelper spiderHelper;

        [Space(5)]
        public GameObject stampShockEffect;
        public GameObject mushrooms;

        [Space(5)]
        public Transform mushroomPoint_Left;
        public Transform mushroomPoint_Right;
    }

    [Serializable]
    public class SkillValue
    {
        [Tooltip("할퀴기 추가공격 개수")]
        public int clawCount = 3;

        [Tooltip("할퀴기 추가공격 간격")]
        public float clawDelay = 0.5f;

        [Tooltip("포효 투사체 개수")]
        public int roarRandCount = 7;

        [Tooltip("스매쉬 투사체 개수")]
        public int smashRandCount = 4;

        [Tooltip("집중 시간")]
        public float concentrateTime = 3;

        [Tooltip("무력화 시간")]
        public float powerlessTime = 3;

        [Tooltip("러쉬 때 거미를 소환하는가?")]
        public bool summonRushSpider = false;
    }
    public class Pools
    {
        public CustomPool<RoarProjectile> roarProjectile = new CustomPool<RoarProjectile>();
        public CustomPool<ClawProjectile> clawProjectile = new CustomPool<ClawProjectile>();
    }

    [Serializable]
    public class AudioClips
    {
        public AudioClip death;
        public AudioClip forwardRoar;
        public AudioClip scratch;
        public AudioClip jumpAttack;
        public AudioClip rockBreak;
        public AudioClip upRoar;
        public AudioClip wakeUp;
        public AudioClip phase2Walk;
        public AudioClip energyGather;
        public AudioClip energyExplosion;
        public AudioClip dash;
        public AudioClip groundStrike;
        public AudioClip strikeAttack;
        public AudioClip parrying;
        public AudioClip stun;
        public AudioClip down;
        public AudioClip wind;
    }
    #endregion


    public Colliders colliders;
    public SkillObjects skillObjects;

    [Header("페이즈 전환 체력")]
    public BossPhaseValue bossPhaseValue;


    [Header("스킬 세부 값")]
    public SkillValue skillValue;


    [Header("패턴 관련")]
    public Patterns patterns;

    [Header("오디오 클립")]
    public AudioClips audioClips;
    private List<List<BearPattern>> phaseList = new List<List<BearPattern>>();

    [HideInInspector]
    public BearPattern currentPattern;
    [HideInInspector]
    public AudioSource audioSource;

    public Pools pools = new Pools();

    private EmissionHelper emissionHelper;
    #region Init 관련
    protected override void Init()
    {
        //패턴 관련 초기화
        phaseList.Add(patterns.phase_01_List);
        phaseList.Add(patterns.phase_02_List);
        ExecutePatternCoroutine = ExecutePattern();

        //맵 관련 초기화
        bearMapInfo.paddingSize = 4;
        bearMapInfo.leftPadding = bearMapInfo.paddingSize;
        bearMapInfo.Init();

        //int layerMask = 1 << LayerMask.NameToLayer(str_Arrow);
        emissionHelper = GetComponent<EmissionHelper>();


        //스테이트 머신 관련 초기화
        stateMachine = new BearStateMachine(this);
        //stateMachine.isDebugMode = true;
        stateMachine.StartState((int)eBearState.Idle);

        skillObjects.concentrateHelper.Init();
        bossPhaseValue.Init(hp);
        maxHp = hp;

        Init_Animator();
        Init_Pool();
        Init_Collider();

        onHitAction = OnHit;

        myTransform.position = new Vector3(bearMapInfo.mapBlocks[4].positions.groundCenter.x, 0.38f, myTransform.position.z);
    }
    private void Init_Animator()
    {
        BearStateMachineBehaviour[] behaviours = animator.GetBehaviours<BearStateMachineBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].bearController = this;
        }

        int paramCount = animator.parameterCount;
        AnimatorControllerParameter[] aniParam = animator.parameters;

        for (int i = 0; i < paramCount; i++)
        {
            AddAnimatorHash(aniParam[i].name);
        }

        skillVarietyBlend = aniHash[str_SkillVarietyBlend];

    }
    private void Init_Collider()
    {
        colliders.bodyCollider.enabled = true;
        //충돌하지 않게 
        // Physics.IgnoreCollision(colliders.headCollider, GameManager.instance.playerController.Com.collider, true);
        // Physics.IgnoreCollision(colliders.bodyCollider, GameManager.instance.playerController.Com.collider, true);

        colliders.headColliderSize = colliders.headCollider.size;
        colliders.bodyColliderSize = colliders.bodyCollider.size;
    }
    private void Init_Pool()
    {
        pools.roarProjectile = CustomPoolManager.Instance.CreateCustomPool<RoarProjectile>();
        pools.clawProjectile = CustomPoolManager.Instance.CreateCustomPool<ClawProjectile>();
    }


    private void Init_Talk()
    {

        //원래라면 이런 느낌으로 헀었어야하지만...
        //    for (int i = 200; i < 200+ DataManager.Instance.currentData_talk.talk_stage_02.Count; i++)
        //    {

        //    }

        talkDict = new Dictionary<int, Action>();

        for (int i = 200; i <= 210; i++)
        {
            int code = i;
            talkDict.Add(code, () => UIManager.Instance.Talk(code, 2f));
        }

        //talkDict.Add("StageClear", () => UIManager.Instance.Talk(206, 2f));
    }
    #endregion
    private void Awake()
    {
        colliders.bodyCollider.enabled = false;
    }
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 1 * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
        GameManager.instance.timelineManager.onTimelineEnded += OnTimelineEnded;
        Init_Talk();
    }
    private void OnTimelineEnded()
    {
        GameManager.instance.timelineManager.onTimelineEnded -= OnTimelineEnded;
        animator.runtimeAnimatorController = runtimeAnimator;


        Init();
        StartCoroutine(ExecutePatternCoroutine);
        StartCoroutine(WaitHpPer20());
        TalkOnce(200);
    }

    private void ProcessChangePhase(ePhase _phase)
    {
        switch (_phase)
        {
            case ePhase.Phase_1:
                //myTransform.SetPositionAndRotation(bearMapInfo.phase2Position, Quaternion.Euler(Vector3.zero));
                //myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                //해야함 : 빌드 시에 널체크 뺴기
                //2페이즈 대사 추가
                if (DataManager.Instance != null)
                {
                    if (talkDict.ContainsKey(203))
                    {
                        talkDict.Remove(203);
                    }
                    talkDict.Add(203, () => UIManager.Instance.Talk(206, 2f));

                }

                //투사체 위치 다시 계산
                bearMapInfo.leftPadding = 0;
                bearMapInfo.rightPadding = bearMapInfo.paddingSize;
                bearMapInfo.Init_Projectiles();
                ChangeState((int)eBearState.Rush);

                break;

            //case ePhase.Phase_2:
            //    myTransform.SetPositionAndRotation(bearMapInfo.phase3Position, Quaternion.Euler(new Vector3(0, 90, 0)));

            //    //투사체 위치 다시 계산
            //    bearMapInfo.Init_Projectiles();
            //    ChangeState((int)eBearState.FinalWalk);
            //    break;

            //case ePhase.Phase_3:
            //    break;

            default:
                break;
        }
        stateInfo.phase += 1;
        currentIndex = 0;

    }

    /// <summary>
    /// 다음 페이즈로 가기 위한 체력을 반환해줍니다.
    /// </summary>
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

    public override void ChangeState(int _state)
    {
        eBearState tempState = GetRandomState((eBearState)_state);
        SetStateInfo((int)tempState);
        stateMachine.ChangeState((int)tempState);
    }

    private int currentIndex = 0;

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
                TalkOnce(207);

                yield break;
            }
            yield return null;
        }
    }
    private IEnumerator ExecutePattern()
    {
        stateInfo.phase = ePhase.Phase_1;
        currentIndex = 0;
        int length = phaseList[stateInfo].Count;
        currentPattern = new BearPattern();

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

        SetStateInfo((int)eBearState.Die);
        ChangeState((int)eBearState.Die);
        testPotal.Active();
    }
    private void SetCurrentPattern(BearPattern _pattern)
    {
        currentPattern = _pattern;

        //if (currentPattern.state == eBossState.BearState)
        //{
        //    currentPattern.state = GetRandomState(stateInfo.phase);

        //}
    }

    public override string GetStateToString(int _state)
    {
        return ((eBearState)_state).ToString();
    }

    #region Random State 관련
    private readonly eBearState[] random_1 = new eBearState[] { eBearState.Stamp, eBearState.Strike_A };
    private readonly eBearState[] random_2 = new eBearState[] { eBearState.Roar_A, eBearState.Strike_A };
    private readonly eBearState[] random_3 = new eBearState[] { eBearState.Strike_A, eBearState.Strike_B };
    private readonly eBearState[] random_4 = new eBearState[] { eBearState.Smash, eBearState.Roar_A };
    private eBearState GetRandomState(eBearState _state)
    {
        int rand = UnityEngine.Random.Range(0, 2);

        switch (_state)
        {
            case eBearState.Random_1:
                return random_1[rand];

            case eBearState.Random_2:
                return random_2[rand];

            case eBearState.Random_3:
                return random_3[rand];

            case eBearState.Random_4:
                return random_4[rand];

            default:
                //그냥 _state 반환
                return _state;
        }
    }

    #endregion

    public void EmissionOn(float _value)
    {
        emissionHelper.EmissionOn(_value);
    }
    public void EmissionOff()
    {
        emissionHelper.EmissionOff();

    }


    private Action onHitAction = null;


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

    public void VoidFunc() { }
    public override void OnHit()
    {
        ReceiveDamage();
        emissionHelper.OnHit();
    }

    private void PlayerHit()
    {
        if (!GameManager.instance.playerController.IsInvincible())
        {
            GameManager.instance.playerController.Hit();
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.collider.CompareTag(TagName.Player))
    //    {
    //        PlayerHit();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Arrow))
        {
            onHitAction?.Invoke();
        }
    }
    public void SetHurtTexture()
    {
        emissionHelper.SetTexture(skillObjects.hurtTex);
    }
}

[Serializable]
public struct BearPattern
{
    [Tooltip("실행할 패턴")]
    public eBearState state;

    [Tooltip("실행 후 대기 시간")]
    public float waitTime;
}
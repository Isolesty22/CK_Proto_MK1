using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

[SelectionBase]
public class BearController : BossController
{

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
    public class Colliders
    {
        public BoxCollider headCollider;
        public BoxCollider bodyCollider;

        public Vector3 headColliderSize;
        public Vector3 bodyColliderSize;
    }

    [Serializable]
    public class SkillObjects
    {
        public GameObject strikeCube;
        public GameObject roarEffect;

        [Space(10)]
        public GameObject claw_A_Effect;
        public GameObject claw_B_Effect;
        public Transform clawUnderPosition;

        [Space(10)]
        public Transform headParringPosition;
        public BearConcentrateHelper concentrateHelper;
        public GameObject concentrateSphere;

        [Space(10)]
        public GameObject smashRock;
        public Transform handTransform;

        [Space(10)]
        public GameObject rushEffect;
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
    }

    #endregion

    #region Test용


    [Serializable]
    public class TestTextMesh
    {
        public TextMesh stateText;
        public TextMesh phaseText;
        public TextMesh hpText;
    }
    public TestTextMesh testTextMesh;
    #endregion


    public Colliders colliders;
    public SkillObjects skillObjects;

    [Header("현재 체력")]
    [Range(0, 100)]
    public float hp = 100f;
    [Header("페이즈 전환 체력")]
    public BossPhaseValue bossPhaseValue;


    [Header("스킬 세부 값")]
    public SkillValue skillValue;


    [Header("패턴 관련")]
    public Patterns patterns;

    private List<List<BearPattern>> phaseList = new List<List<BearPattern>>();
    [HideInInspector]
    public BearPattern currentPattern;

    public CustomPool<RoarProjectile> roarProjectilePool = new CustomPool<RoarProjectile>();
    public CustomPool<ClawProjectile> clawProjectilePool = new CustomPool<ClawProjectile>();
    public CustomPool<SmashProjectile> smashProjectilePool = new CustomPool<SmashProjectile>();

    #region Init 관련
    protected override void Init()
    {
        phaseList.Add(patterns.phase_01_List);
        phaseList.Add(patterns.phase_02_List);
        phaseList.Add(patterns.phase_03_List);
        ExecutePatternCoroutine = ExecutePattern();

        bearMapInfo.exclusionRange = 3;
        bearMapInfo.Init();

        //int layerMask = 1 << LayerMask.NameToLayer(str_Arrow);

        stateMachine = new BearStateMachine(this);
        stateMachine.isDebugMode = true;
        stateMachine.StartState((int)eBearState.Idle);

        skillObjects.concentrateHelper.Init();
        Init_Animator();
        Init_Pool();
        Init_Collider();
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

        //AddAnimatorHash("Start_Idle");
        //AddAnimatorHash("Start_Rush");
        //AddAnimatorHash("Start_Roar");
        //AddAnimatorHash("Start_Claw");
        //AddAnimatorHash("Start_Strike");
        //AddAnimatorHash("Start_Stamp");
        //AddAnimatorHash("Start_Smash");
        //AddAnimatorHash("Start_Powerless");
        //AddAnimatorHash("Start_Concentrate");
        //AddAnimatorHash("Start_Die");
        //AddAnimatorHash("End_Concentrate");
        //AddAnimatorHash("End_Powerless");
    }
    private void Init_Collider()
    {
        //충돌하지 않게 
        // Physics.IgnoreCollision(colliders.headCollider, GameManager.instance.playerController.Com.collider, true);
        Physics.IgnoreCollision(colliders.bodyCollider, GameManager.instance.playerController.Com.collider, true);

        colliders.headColliderSize = colliders.headCollider.size;
        colliders.bodyColliderSize = colliders.bodyCollider.size;
    }
    private void Init_Pool()
    {
        roarProjectilePool = CustomPoolManager.Instance.CreateCustomPool<RoarProjectile>();
        clawProjectilePool = CustomPoolManager.Instance.CreateCustomPool<ClawProjectile>();
        smashProjectilePool = CustomPoolManager.Instance.CreateCustomPool<SmashProjectile>();
    }

    #endregion
    private void Start()
    {
        Init();
        StartCoroutine(ExecutePatternCoroutine);
    }
    private void Update()
    {
        testTextMesh.stateText.text = stateInfo.state;
        testTextMesh.hpText.text = hp.ToString();
        testTextMesh.phaseText.text = stateInfo.phase.ToString();
    }

    private void ProcessChangePhase(ePhase _phase)
    {
        switch (_phase)
        {
            case ePhase.Phase_1:
                //myTransform.SetPositionAndRotation(bearMapInfo.phase2Position, Quaternion.Euler(Vector3.zero));
                //myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SetStretchColliderSize();

                //투사체 위치 다시 계산
                bearMapInfo.exclusionRange = 0;
                bearMapInfo.Init_Projectiles();
                ChangeState(eBearState.Rush);
                break;

            case ePhase.Phase_2:
                //myTransform.SetPositionAndRotation(bearMapInfo.phase3Position, Quaternion.Euler(new Vector3(0, 90, 0)));

                SetOriginalColliderSize();
                //투사체 위치 다시 계산
                bearMapInfo.exclusionRange = 3;
                bearMapInfo.Init_Projectiles();
                ChangeState(eBearState.FinalWalk);
                break;

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
                return bossPhaseValue.phase2;

            case ePhase.Phase_2:
                return bossPhaseValue.phase3;

            case ePhase.Phase_3:
                return 0;

            default:
                return 0;
        }
    }

    WaitForSecondsRealtime waitOneSec = new WaitForSecondsRealtime(1f);
    private int currentIndex = 0;
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
                    ChangeState(currentPattern.state);

                    currentIndex += 1;
                    currentIndex = currentIndex % length;
                }
                    yield return null;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        SetStateInfo((int)eBearState.Die);
        ChangeState(eBearState.Die);

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

    //랜덤 범위------------
    private readonly eBearState[] patterns_phase_1
        = { eBearState.Stamp, eBearState.Strike_A, eBearState.Claw_A };
    private readonly eBearState[] patterns_phase_2
        = { eBearState.Concentrate, eBearState.Claw_B, eBearState.Strike_B };//,eBossState.BearState_Roar_B};
    private readonly eBearState[] patterns_phase_3
        = { eBearState.Strike_A, eBearState.Strike_C };
    private eBearState GetRandomState(ePhase _phase)
    {
        switch (_phase)
        {
            case ePhase.Phase_1:
                return patterns_phase_1[UnityEngine.Random.Range(0, patterns_phase_1.Length)];

            case ePhase.Phase_2:
                return patterns_phase_2[UnityEngine.Random.Range(0, patterns_phase_2.Length)];

            case ePhase.Phase_3:
                return patterns_phase_3[UnityEngine.Random.Range(0, patterns_phase_3.Length)];

            default:
                Debug.LogError("GetRandomState Error");
                return eBearState.None;
        }
    }


    #region Collider 관련
    private void SetStretchColliderSize()
    {
        colliders.headCollider.size = new Vector3(colliders.headColliderSize.x, colliders.headColliderSize.y, 10f);
        colliders.bodyCollider.size = new Vector3(colliders.bodyColliderSize.x, colliders.bodyColliderSize.y, 10f);
    }
    private void SetOriginalColliderSize()
    {
        colliders.headCollider.size = new Vector3(colliders.headColliderSize.x, colliders.headColliderSize.y, colliders.headColliderSize.z);
        colliders.bodyCollider.size = new Vector3(colliders.bodyColliderSize.x, colliders.bodyColliderSize.y, colliders.bodyColliderSize.z);
    }
    #endregion


    private readonly string str_Arrow = "Arrow";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(str_Arrow))
        {
            hp -= 1f;
        }
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
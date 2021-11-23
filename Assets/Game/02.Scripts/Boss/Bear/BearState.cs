using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearState : BossState
{
    protected BearController bearController;
}

public class BearState_Idle : BearState
{
    public BearState_Idle(BearController _bearController)
    {
        bearController = _bearController;
    }

    public override void OnEnter()
    {
        canExit = true;
    }

}
public class BearState_Stamp : BearState
{
    public BearState_Stamp(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Stamp_Start");
        UIManager.Instance.Talk("뛰어! 이피아!");
    }

    public override void OnExit()
    {
        bearController.skillObjects.stampShockEffect.SetActive(false);
    }

    public void AnimEvent()
    {
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.jumpAttack);

        bearController.skillObjects.stampShockEffect.SetActive(true);
        //Camera Shake
        GameManager.instance.cameraManager.ShakeCamera();
        //땅에 있을 경우
        if (GameManager.instance.playerController.State.isGrounded == true)
        {
            //피격 가능 상태일 경우
            if (!GameManager.instance.playerController.IsInvincible())
            {
                GameManager.instance.playerController.Hit();
            }
        }

        if (bearController.stateInfo.phase == ePhase.Phase_1)
        {
            GameObject.Instantiate(
                bearController.skillObjects.mushrooms,
                bearController.skillObjects.mushroomPoint_Left.position, Quaternion.identity);

        }
        else
        {
            GameObject.Instantiate(
                bearController.skillObjects.mushrooms,
                bearController.skillObjects.mushroomPoint_Right.position, Quaternion.identity);

        }
    }
}
public class BearState_Rush : BearState
{
    Vector3 startPos;
    Vector3 leftRushPos;
    Vector3 phase2Pos;
    Quaternion phase2Rotation;

    float timer;
    float progress;
    float rushTime = 1.5f;
    float walkTime = 3f;

    bool canGo;
    public BearState_Rush(BearController _bearController)
    {
        bearController = _bearController;

        startPos = bearController.myTransform.position;
        //왼쪽 끝까지 돌진하는 위치 설정
        leftRushPos = new Vector3(bearController.bearMapInfo.mapData.minPosition.x - 3f,
            bearController.myTransform.position.y,
            bearController.myTransform.position.z);

        //마지막 위치
        phase2Rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
        phase2Pos = bearController.bearMapInfo.phase2Position;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.StartInvincible();

        canGo = true;
        bearController.colliders.bodyCollider.enabled = false;

        bearController.TalkOnce(204);

        //맵의 왼쪽으로 빠르게 이동
        bearController.SetAnimEvent(AnimEvent_LeftRush);

        //애니메이션 스타트
        bearController.SetTrigger("Rush_Start");
    }
    public override void OnExit()
    {
        //사운드 종료
        bearController.audioSource.loop = false;
        bearController.audioSource.Stop();


        //다시 공격을 받아야하니까 원래대로 변경
        bearController.gameObject.tag = TagName.Boss;
        bearController.EndInvincible();
    }

    public void AnimEvent_LeftRush()
    {
        currentCoroutine = CoLeftRush();
        bearController.StartCoroutine(currentCoroutine);
    }

    public void AnimEvent_GoMove()
    {
        canGo = true;
        //bearController.skillObjects.rushEffect.SetActive(true);
        bearController.SetAnimEvent(AnimEvent_StopMove);
    }
    public void AnimEvent_StopMove()
    {
        canGo = false;
        //bearController.skillObjects.rushEffect.SetActive(false);
        bearController.SetAnimEvent(AnimEvent_GoMove);
    }
    private IEnumerator CoLeftRush()
    {
        timer = 0f;
        progress = 0f;

        bearController.SetAnimEvent(AnimEvent_StopMove);

        //정령 게이지 안채우게 하려고 태그를 변경
        bearController.gameObject.tag = TagName.Untagged;

        //이펙트 켜기
        bearController.skillObjects.rushEffect.SetActive(true);

        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.dash);

        while (progress < 1f)
        {
            if (!canGo)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
                continue;
            }
            timer += Time.fixedDeltaTime;
            progress = timer / rushTime;

            bearController.myTransform.position = Vector3.Lerp(startPos, leftRushPos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //돌진 종료
        bearController.SetAnimEvent(AnimEvent_StopMove);
        bearController.SetTrigger("Rush_End");
        //이후 자동으로 걷기 애니메이션 출력됨
        bearController.skillObjects.rushEffect.SetActive(false);

        yield return YieldInstructionCache.WaitForEndOfFrame;

        //회전
        bearController.myTransform.rotation = phase2Rotation;

        //텍스처 변경
        bearController.SetHurtTexture();

        //콜라이더 활성화
        bearController.colliders.bodyCollider.enabled = true;

        timer = 0f;
        progress = 0f;

        UIManager.Instance.Talk("다시 이쪽으로 오고있어….");


        //페이즈 2 포지션까지 걷기
        while (progress < 1f)
        {
            if (!canGo)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
                continue;
            }
            timer += Time.fixedDeltaTime;
            progress = timer / walkTime;

            //사운드 출력
            if (!bearController.audioSource.isPlaying)
            {
                bearController.audioSource.clip = bearController.audioClips.phase2Walk;
                bearController.audioSource.loop = true;
                bearController.audioSource.Play();
            }
            bearController.myTransform.position = Vector3.Lerp(leftRushPos, phase2Pos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //페이즈 2 포지션까지 도착 완료

        //사운드 종료
        bearController.audioSource.loop = false;
        bearController.audioSource.Stop();

        bearController.SetTrigger("Rush_Walk_End");
        canExit = true;
    }

}

public class BearState_Roar : BearState
{

    private WaitForSeconds waitSec = new WaitForSeconds(1f);
    public BearState_Roar(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;

        switch ((eBearState)bearController.stateInfo.stateInt)
        {
            // 투사체
            case eBearState.Roar_A:
                bearController.bearMapInfo.UpdateProjectileRandArray();

                //사운드 출력
                bearController.audioSource.PlayOneShot(bearController.audioClips.upRoar);

                bearController.SetAnimEvent(AnimEvent_A);
                bearController.SetSkillVariety(0);

                bearController.skillObjects.roarGroundEffect.SetActive(true);
                break;

            // 중앙 공격
            case eBearState.Roar_B:
                //사운드 출력
                bearController.audioSource.PlayOneShot(bearController.audioClips.forwardRoar);

                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);

                UIManager.Instance.Talk("이피아! 몸을 낮춰야해!");
                break;

            default:
                break;
        }
        bearController.SetTrigger("Roar_Start");
        //bearController.StartCoroutine(ProcessUpdate());
    }
    public override void OnExit()
    {
        //혹시 모르니까 이펙트 끄기
        bearController.skillObjects.roarGroundEffect.SetActive(false);
        bearController.skillObjects.roarEffect.SetActive(false);
    }
    public void AnimEvent_A()
    {
        currentCoroutine = CoSkill_A();
        bearController.StartCoroutine(CoSkill_A());
    }
    public void AnimEvent_B()
    {
        currentCoroutine = CoSkill_B();
        bearController.StartCoroutine(CoSkill_B());
    }

    //위에서 뭔가 떨어짐
    private IEnumerator CoSkill_A()
    {
        //Camera Shake
        GameManager.instance.cameraManager.ShakeCamera();

        //Spawn Roar projectile
        int length = bearController.skillValue.roarRandCount;
        for (int i = 0; i < length; i++)
        {
            Vector3 startPos = bearController.bearMapInfo.projectilePositions[bearController.bearMapInfo.projectileRandArray[i]];
            Vector3 endPos = new Vector3(startPos.x, bearController.bearMapInfo.mapData.minPosition.y, startPos.z);


            RoarProjectile roarProjectile = bearController.pools.roarProjectile.SpawnThis();
            roarProjectile.Init(startPos, endPos);
            roarProjectile.Move();

            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
        }

        bearController.skillObjects.roarGroundEffect.SetActive(false);
        yield break;
    }

    //숙여서 공격
    private IEnumerator CoSkill_B()
    {
        bearController.skillObjects.roarEffect.SetActive(true);
        yield return waitSec;
        bearController.skillObjects.roarEffect.SetActive(false);
    }
}
public class BearState_Strike : BearState
{
    WaitForSeconds waitSec_B = new WaitForSeconds(0.9f);
    public BearState_Strike(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;

        ShuffleArray();

        switch ((eBearState)bearController.stateInfo.stateInt)
        {
            case eBearState.Strike_A:
                bearController.SetAnimEvent(AnimEvent_A);
                bearController.SetSkillVariety(0);

                bearController.TalkOnce(201);
                break;

            case eBearState.Strike_B:
                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);

                bearController.TalkOnce(205);
                break;

            default:
                break;
        }
        bearController.SetTrigger("Strike_Start");
    }

    private int[] strikePos;

    #region ShuffleArray
    private void ShuffleArray()
    {
        if (bearController.stateInfo.phase == ePhase.Phase_1)
        {
            ShuffleArray_Phase1();
        }
        else
        {
            ShuffleArray_Phase2();
        }
    }
    /// <summary>
    /// 페이즈 1 전용 배열 섞기
    /// </summary>
    private void ShuffleArray_Phase1()
    {
        strikePos = new int[4] { 0, 1, 2, 3 };

        int length = strikePos.Length;

        for (int i = 0; i < length; i++)
        {
            int randIndex = Random.Range(i, length);

            int tempPos = strikePos[i];
            strikePos[i] = strikePos[randIndex];
            strikePos[randIndex] = tempPos;
        }
    }

    /// <summary>
    /// 페이즈 2 전용 배열 섞기
    /// </summary>
    private void ShuffleArray_Phase2()
    {
        strikePos = new int[4] { 1, 2, 3, 4 };

        int length = strikePos.Length;

        for (int i = 0; i < length; i++)
        {
            int randIndex = Random.Range(i, length);

            int tempPos = strikePos[i];
            strikePos[i] = strikePos[randIndex];
            strikePos[randIndex] = tempPos;
        }
    }
    #endregion


    //해야함 : Clone 말고 풀링

    private void CloneStrikeCube(int _bearBlockIndex)
    {
        GameObject.Instantiate(bearController.skillObjects.strikeCube, bearController.bearMapInfo.mapBlocks[_bearBlockIndex].positions.groundCenter, Quaternion.identity);
    }
    //랜덤
    private void AnimEvent_A()
    {
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.strikeAttack);

        GameManager.instance.cameraManager.ShakeCamera();
        CloneStrikeCube(strikePos[0]);
        CloneStrikeCube(strikePos[1]);
        CloneStrikeCube(strikePos[2]);
    }

    //왼쪽부터 오른쪽까지
    private void AnimEvent_B()
    {
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.strikeAttack);

        //Camera Shake
        GameManager.instance.cameraManager.ShakeCamera();

        currentCoroutine = CoSkill_B();
        bearController.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoSkill_B()
    {
        for (int i = 1; i < 5; i++)
        {
            CloneStrikeCube(i);
            yield return waitSec_B;

        }
    }

}
public class BearState_Claw : BearState
{
    private int random = 0;
    private float projectileEndPosX;

    private float rotVal = 70f;

    private WaitForSeconds waitHalfSec = new WaitForSeconds(0.5f);

    private WaitForSeconds waitDelay_B = null;
    public BearState_Claw(BearController _bearController)
    {
        bearController = _bearController;
        waitDelay_B = new WaitForSeconds(bearController.skillValue.clawDelay);
    }
    public override void OnEnter()
    {
        canExit = false;

        switch ((eBearState)bearController.stateInfo.stateInt)
        {
            case eBearState.Claw_A:
                bearController.SetSkillVariety(0);
                bearController.SetAnimEvent(AnimEvent_A);
                break;

            case eBearState.Claw_B:
                bearController.SetSkillVariety(1);
                bearController.SetAnimEvent(AnimEvent_B);
                break;

            default:
                break;
        }

        switch (bearController.stateInfo.phase)
        {
            case ePhase.Phase_1:
                projectileEndPosX = bearController.bearMapInfo.mapData.minPosition.x;
                break;
            case ePhase.Phase_2:
                projectileEndPosX = bearController.bearMapInfo.mapData.maxPosition.x;
                break;
            default:
                break;
        }
        bearController.SetTrigger("Claw_Start");
    }


    public override void OnExit()
    {
        bearController.skillObjects.claw_B_Effect.SetActive(false);
        bearController.skillObjects.claw_A_Effect.SetActive(false);
    }
    public void AnimEvent_A()
    {
        currentCoroutine = CoSkill_A();
        bearController.StartCoroutine(currentCoroutine);
    }
    public void AnimEvent_B()
    {
        currentCoroutine = CoSkill_B();
        bearController.StartCoroutine(currentCoroutine);
    }



    private IEnumerator CoSkill_A()
    {
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.scratch);

        bearController.skillObjects.claw_A_Effect.SetActive(true);
        yield return waitHalfSec;
        bearController.skillObjects.claw_A_Effect.SetActive(false);
    }

    private IEnumerator CoSkill_B()
    {
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.scratch);

        bearController.skillObjects.claw_B_Effect.SetActive(true);

        //Spawn Claw projectile
        int length = bearController.skillValue.clawCount;

        for (int i = 0; i < length; i++)
        {
            ClawProjectile clawProjectile = bearController.pools.clawProjectile.SpawnThis();

            Vector3 startPos = Quaternion.Euler(0, 0, clawProjectile.degree) * bearController.skillObjects.clawUnderPosition.position;
            Vector3 endPos = new Vector3(projectileEndPosX, startPos.y, startPos.z);

            //사운드 출력
            bearController.audioSource.PlayOneShot(bearController.audioClips.wind);

            clawProjectile.Init(startPos, endPos);
            clawProjectile.Move();

            yield return waitDelay_B;
        }

        bearController.skillObjects.claw_B_Effect.SetActive(false);
        yield break;
    }
}
public class BearState_Smash : BearState
{
    WaitForSeconds waitSec = new WaitForSeconds(1f);
    SmashHelper smashHelper;
    public BearState_Smash(BearController _bearController)
    {
        bearController = _bearController;
        smashHelper = bearController.skillObjects.smashHelper;
    }
    public override void OnEnter()
    {
        canExit = false;
        //손 따라가게
        currentCoroutine = CoFollowHand();

        bearController.bearMapInfo.UpdateProjectileRandArray();

        bearController.SetAnimEvent(ActiveHandRock);
        bearController.SetTrigger("Smash_Start");
    }


    public override void OnExit()
    {
        smashHelper.SetActive(false);
    }
    /// <summary>
    /// 손에 있는 돌덩이를 활성화합니다.
    /// </summary>
    public void ActiveHandRock()
    {

        //바위 활성화
        smashHelper.SetActive(true);
        smashHelper.SetActiveRocks(true);


        bearController.StartCoroutine(currentCoroutine);

        bearController.SetAnimEvent(AnimEvent);
    }
    private IEnumerator CoFollowHand()
    {
        while (true)
        {
            smashHelper.myTransform.SetPositionAndRotation(
                smashHelper.bearHandTransform.position,
                smashHelper.bearHandTransform.rotation);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }


    public void AnimEvent()
    {
        currentCoroutine = CoSkill();
        bearController.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoSkill()
    {

        //바위가 곰의 손을 더이상 따라가지 않음
        bearController.StopCoroutine(currentCoroutine);
        smashHelper.SetParentRocks(null);

        //yield return new WaitForSeconds(0.2f);

        int length = smashHelper.rockCount;

        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.rockBreak);

        //바위 쿠과광
        for (int i = 0; i < length; i++)
        {
            Vector3 startPos = smashHelper.GetPosition(i);

            Vector3 tempPos = bearController.bearMapInfo.projectilePositions[bearController.bearMapInfo.projectileRandArray[i]];
            Vector3 midPos = new Vector3(tempPos.x, tempPos.y - 5f, tempPos.z);
            Vector3 endPos = new Vector3(midPos.x, bearController.bearMapInfo.mapData.minPosition.y, midPos.z);

            smashHelper.UpdateVectors(i, startPos, midPos, endPos);
            smashHelper.StartMove(i);
        }
        smashHelper.SetActive(false);
        yield break;
    }

}
public class BearState_Concentrate : BearState
{

    private Transform sphereTransform;
    private HeadParryingHelper helper;
    private VfxActiveHelper flashEffect;
    private UIFlashScreen flashScreen;

    public BearState_Concentrate(BearController _bearController)
    {
        bearController = _bearController;

        sphereTransform = bearController.skillObjects.concentrateSphere.transform;
        flashEffect = bearController.skillObjects.flashEffect;
        flashEffect.SetActiveTime(1f);

        helper = bearController.skillObjects.concentrateHelper;


        flashScreen = UIManager.Instance.GetUI<UIFlashScreen>() as UIFlashScreen;

    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.StartInvincible();
        currentCoroutine = CoSkill();

        bearController.TalkOnce(202);
        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Concentrate_Start");

    }
    public override void OnExit()
    {
        bearController.EndInvincible();
        bearController.EmissionOn(10f);
    }
    public void AnimEvent()
    {
        //사운드 출력
        bearController.audioSource.clip = bearController.audioClips.energyGather;
        bearController.audioSource.Play();

        helper.StartCheck();
        bearController.skillObjects.concentrateSphere.SetActive(true);
        bearController.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoSkill()
    {
        float timer = 0f;
        float maxTime = bearController.skillValue.concentrateTime + 1f;
        float progress = 0f;

        bearController.EmissionOn(60f);
        while (progress < 1f)
        {
            timer += Time.fixedDeltaTime;
            progress = timer / maxTime;

            bearController.EmissionOn(50f + (Mathf.Sin(timer * 10f)) * 30f);

            if (helper.isSucceedParry)
            {
                //사운드 출력
                bearController.audioSource.Stop();
                bearController.audioSource.PlayOneShot(bearController.audioClips.parrying);
                ChangeStatePowerless();
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //사운드 출력
        bearController.audioSource.Stop();
        bearController.audioSource.PlayOneShot(bearController.audioClips.energyExplosion);

        bearController.SetTrigger("Concentrate_End");
        helper.EndCheck();

        yield return new WaitForSeconds(0.1f);

        //구체 없애기
        sphereTransform.gameObject.SetActive(false);
        bearController.EmissionOn(10f);

        //이펙트 퍼펑~
        flashEffect.Active();
        flashScreen.StartFlashScreen();

        //플레이어에게 데미지를 입힘
        if (!GameManager.instance.playerController.IsInvincible())
        {
            GameManager.instance.playerController.Hit();
        }
    }
    private void ChangeStatePowerless()
    {
        bearController.StopCoroutine(currentCoroutine);

        sphereTransform.gameObject.SetActive(false);
        helper.EndCheck();

        bearController.ChangeState((int)eBearState.Powerless);
    }
}
public class BearState_Powerless : BearState
{
    private WaitForSeconds waitSecBegin;

    /// <summary>
    /// 무력화 이후의 waitTime을 적용하기 위해서...
    /// </summary>
    private WaitForSeconds waitSecEnd;

    private ParticleSystem stunEffect;
    public BearState_Powerless(BearController _bearController)
    {
        bearController = _bearController;
        waitSecBegin = new WaitForSeconds(bearController.skillValue.powerlessTime);
        waitSecEnd = new WaitForSeconds(bearController.currentPattern.waitTime);
        stunEffect = bearController.skillObjects.stunEffect;
    }
    public override void OnEnter()
    {
        canExit = false;

        bearController.EmissionOff();

        currentCoroutine = ProcessAnimEvent_Begin();
        bearController.StartCoroutine(currentCoroutine);


        bearController.TalkOnce(203);
        bearController.SetAnimEvent(AnimEvent_StunEffectOn);
        bearController.SetTrigger("Powerless_Start");
    }

    public override void OnExit()
    {
        //사운드 스탑
        bearController.audioSource.Stop();

        //이미션 원래대로...
        bearController.EmissionOn(10f);

        //이펙트 켜기
        stunEffect.Stop();
        stunEffect.gameObject.SetActive(false);


    }
    public void AnimEvent_StunEffectOn()
    {
        stunEffect.gameObject.SetActive(true);
        stunEffect.Play();

        //사운드 출력
        bearController.audioSource.loop = true;
        bearController.audioSource.clip = bearController.audioClips.stun;
        bearController.audioSource.Play();

        bearController.SetAnimEvent(AnimEvent_WaitEnd);
    }

    public void AnimEvent_WaitEnd()
    {
        currentCoroutine = ProcessWaitTime();
        bearController.StartCoroutine(currentCoroutine);
    }

    private IEnumerator ProcessAnimEvent_Begin()
    {
        //대기
        yield return waitSecBegin;
        bearController.EmissionOn(10f);
        bearController.SetTrigger("Powerless_End");

        stunEffect.Stop();
        stunEffect.gameObject.SetActive(false);

        //사운드 출력
        bearController.audioSource.Stop();
        bearController.audioSource.PlayOneShot(bearController.audioClips.wakeUp);

    }

    private IEnumerator ProcessWaitTime()
    {
        //대기
        yield return waitSecEnd;
        canExit = true;
    }
}

public class BearState_Die : BearState
{
    public BearState_Die(BearController _bearController)
    {
        bearController = _bearController;

    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.colliders.bodyCollider.enabled = false;
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.death);

        bearController.StartCoroutine(CoTalkDie());
        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Die_Start");
    }

    public void AnimEvent()
    {
        bearController.animator.enabled = false;
        bearController.colliders.groundCollider.enabled = true;
        bearController.testPotal.Active();

    }


    private IEnumerator CoTalkDie()
    {
        bearController.TalkOnce(208);
        yield return new WaitForSeconds(2.5f);
        bearController.TalkOnce(209);
        yield return new WaitForSeconds(1.5f);
        //사운드 출력
        bearController.audioSource.PlayOneShot(bearController.audioClips.down);
        yield return new WaitForSeconds(1f);
        bearController.TalkOnce(210);
    }
}




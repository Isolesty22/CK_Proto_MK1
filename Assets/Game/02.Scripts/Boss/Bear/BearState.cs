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
    }

    public override void OnExit()
    {
        bearController.skillObjects.stampShockEffect.SetActive(false);
    }

    public void AnimEvent()
    {
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

        //맵의 왼쪽으로 빠르게 이동
        bearController.SetAnimEvent(LeftRush);

        bearController.colliders.bodyCollider.enabled = false;
        //애니메이션 스타트
        bearController.SetTrigger("Rush_Start");
    }
    public override void OnExit()
    {
        bearController.colliders.bodyCollider.enabled = true;
        bearController.EndInvincible();
    }

    public void LeftRush()
    {
        bearController.StartCoroutine(ProcessLeftRush());
    }

    public void GoMove()
    {
        canGo = true;
        //bearController.skillObjects.rushEffect.SetActive(true);
        bearController.SetAnimEvent(StopMove);
    }
    public void StopMove()
    {
        canGo = false;
        //bearController.skillObjects.rushEffect.SetActive(false);
        bearController.SetAnimEvent(GoMove);
    }
    private IEnumerator ProcessLeftRush()
    {
        timer = 0f;
        progress = 0f;

        bearController.SetAnimEvent(StopMove);

        bearController.skillObjects.rushEffect.SetActive(true);
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

        bearController.SetAnimEvent(StopMove);
        bearController.SetTrigger("Rush_End");
        //이후 자동으로 걷기 애니메이션 출력됨

        bearController.skillObjects.rushEffect.SetActive(false);

        yield return YieldInstructionCache.WaitForEndOfFrame;

        //회전
        bearController.myTransform.rotation = phase2Rotation;

        //텍스처 변경

        bearController.SetHurtTexture();

        timer = 0f;
        progress = 0f;

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

            bearController.myTransform.position = Vector3.Lerp(leftRushPos, phase2Pos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //페이즈 2 포지션까지 도착 완료

        bearController.SetTrigger("Rush_Walk_End");
        canExit = true;
    }

    private IEnumerator ProcessChangePhase2()
    {
        yield break;
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

                bearController.SetAnimEvent(AnimEvent_A);
                bearController.SetSkillVariety(0);

                bearController.skillObjects.roarGroundEffect.SetActive(true);
                break;

            // 중앙 공격
            case eBearState.Roar_B:
                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);
                UIManager.Instance.Talk("이피아! 몸을 낮춰!");
                break;

            default:
                break;
        }
        bearController.SetTrigger("Roar_Start");
        //bearController.StartCoroutine(ProcessUpdate());
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public void AnimEvent_A()
    {
        bearController.StartCoroutine(ProcessAnimEvent_A());
    }
    public void AnimEvent_B()
    {
        bearController.StartCoroutine(ProcessAnimEvent_B());
    }

    //위에서 뭔가 떨어짐
    private IEnumerator ProcessAnimEvent_A()
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
    private IEnumerator ProcessAnimEvent_B()
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
                break;

            case eBearState.Strike_B:
                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);
                break;

            default:
                break;
        }
        UIManager.Instance.Talk("바닥을 조심해, 이피아!");
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
        GameManager.instance.cameraManager.ShakeCamera();
        CloneStrikeCube(strikePos[0]);
        CloneStrikeCube(strikePos[1]);
        CloneStrikeCube(strikePos[2]);
    }

    //왼쪽부터 오른쪽까지
    private void AnimEvent_B()
    {        
        //Camera Shake
        GameManager.instance.cameraManager.ShakeCamera();

        bearController.StartCoroutine(ProcessAnimEvent_B());
    }


    private IEnumerator ProcessAnimEvent_B()
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

    public void AnimEvent_A()
    {
        bearController.StartCoroutine(ProcessAnimEvent_A());
    }
    public void AnimEvent_B()
    {
        bearController.StartCoroutine(ProcessAnimEvent_B());
    }

    private IEnumerator ProcessAnimEvent_A()
    {
        bearController.skillObjects.claw_A_Effect.SetActive(true);
        yield return waitHalfSec;
        bearController.skillObjects.claw_A_Effect.SetActive(false);
    }

    private IEnumerator ProcessAnimEvent_B()
    {
        bearController.skillObjects.claw_B_Effect.SetActive(true);

        //Spawn Claw projectile
        int length = bearController.skillValue.clawCount;

        for (int i = 0; i < length; i++)
        {
            ClawProjectile clawProjectile = bearController.pools.clawProjectile.SpawnThis();

            Vector3 startPos = Quaternion.Euler(0, 0, clawProjectile.degree) * bearController.skillObjects.clawUnderPosition.position;
            Vector3 endPos = new Vector3(projectileEndPosX, startPos.y, startPos.z);

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

        followHand = CoFollowHand();

        bearController.bearMapInfo.UpdateProjectileRandArray();

        bearController.SetAnimEvent(ActiveHandRock);
        bearController.SetTrigger("Smash_Start");
    }

    private IEnumerator followHand = null;
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

    /// <summary>
    /// 손에 있는 돌덩이를 활성화합니다.
    /// </summary>
    public void ActiveHandRock()
    {

        //바위 활성화
        smashHelper.SetActive(true);
        smashHelper.SetActiveRocks(true);

        //손 따라가게
        bearController.StartCoroutine(followHand);

        bearController.SetAnimEvent(AnimEvent);
    }
    public void AnimEvent()
    {
        bearController.StartCoroutine(ProcessAnimEvent());
    }

    private IEnumerator ProcessAnimEvent()
    {

        //바위가 곰의 손을 더이상 따라가지 않음
        bearController.StopCoroutine(followHand);
        smashHelper.SetParentRocks(null);

        //yield return new WaitForSeconds(0.2f);

        int length = smashHelper.rockCount;
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
    private IEnumerator concentrate;

    private Transform sphereTransform;
    private HeadParryingHelper helper;


    public BearState_Concentrate(BearController _bearController)
    {
        bearController = _bearController;

        sphereTransform = bearController.skillObjects.concentrateSphere.transform;
        helper = bearController.skillObjects.concentrateHelper;

    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.StartInvincible();
        UIManager.Instance.Talk("앗, 안돼! 어서 머리를 밟아버려!");
        concentrate = ProcessConcentrate();

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
        helper.StartCheck();
        bearController.skillObjects.concentrateSphere.SetActive(true);
        bearController.StartCoroutine(concentrate);
    }

    private IEnumerator ProcessConcentrate()
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
                ChangeStatePowerless();
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        bearController.SetTrigger("Concentrate_End");
        bearController.EmissionOn(10f);
        sphereTransform.gameObject.SetActive(false);
        helper.EndCheck();
    }

    private void ChangeStatePowerless()
    {
        bearController.StopCoroutine(concentrate);

        sphereTransform.gameObject.SetActive(false);
        helper.EndCheck();

        bearController.ChangeState((int)eBearState.Powerless);
    }
}
public class BearState_Powerless : BearState
{
    private WaitForSeconds waitSecBegin;
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
        bearController.StartCoroutine(ProcessAnimEvent_Begin());

        UIManager.Instance.Talk("이때다! 이피아, 공격해!");

        bearController.SetAnimEvent(AnimEvent_StunEffectOn);
        bearController.SetTrigger("Powerless_Start");

    }

    public override void OnExit()
    {
        bearController.EmissionOn(10f);

    }
    public void AnimEvent_StunEffectOn()
    {
        stunEffect.gameObject.SetActive(true);
        stunEffect.Play();

        bearController.SetAnimEvent(AnimEvent_WaitEnd);
    }

    public void AnimEvent_WaitEnd()
    {
        bearController.StartCoroutine(ProcessWaitTime());
    }

    private IEnumerator ProcessAnimEvent_Begin()
    {
        //대기
        yield return waitSecBegin;
        bearController.EmissionOn(10f);
        bearController.SetTrigger("Powerless_End");

        stunEffect.Stop();
        stunEffect.gameObject.SetActive(false);

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

        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Die_Start");
        UIManager.Instance.Talk("휴...더 이상 우리를 공격하진 못할 것 같아.");
    }

    public void AnimEvent()
    {
        bearController.animator.enabled = false;
        bearController.colliders.groundCollider.enabled = true;
    }
}




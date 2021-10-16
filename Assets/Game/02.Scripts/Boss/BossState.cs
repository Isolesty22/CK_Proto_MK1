using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    /// <summary>
    /// exit해도 괜찮은 상태인가?
    /// </summary>
    public bool canExit = true;

    /// <summary>
    /// 상태가 시작할때 호출됨
    /// </summary>
    public virtual void OnEnter()
    {
        canExit = true;

    }

    /// <summary>
    /// Monobehaviour Update에서 지속적으로 호출됨
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// Monobehaviour FixedUpdate에서 지속적으로 호출됨
    /// </summary>
    public virtual void OnFixedUpdate()
    {

    }
    /// <summary>
    /// 상태가 끝나고 다른 상태로 진입할때 호출됨
    /// </summary>
    public virtual void OnExit()
    {

    }
}

#region BearState
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
        //자동으로 Idle이 되니까 한번 SetTrigger를 안해본다
        //bearController.SetTrigger("Idle_Start");
        //bearController.StartCoroutine(ProcessUpdate());
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    IEnumerator ProcessUpdate()
    {
        while (!bearController.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            yield return null;
        }
        Debug.Log("Idle 애니메이션 진입");

        while (bearController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        Debug.Log("Idle 애니메이션 끝");
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

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void AnimEvent()
    {
        //땅에 있을 경우
        if (GameManager.instance.playerController.State.isGrounded == true)
        {
            //피격 가능 상태일 경우
            if (!GameManager.instance.playerController.IsInvincible())
            {
                GameManager.instance.playerController.Hit();
            }
        }
    }
}
public class BearState_Rush : BearState
{
    Vector3 startPos;
    Vector3 leftRushPos;
    Vector3 zTelePos;
    Vector3 phase2Pos;
    Quaternion zWalkRotation;
    Quaternion phase2Rotation;

    float timer;
    float progress;
    float rushTime = 2f;
    float walkTime = 8f;
    float rotateTime = 1f;

    bool canGo;
    public BearState_Rush(BearController _bearController)
    {
        bearController = _bearController;

        startPos = bearController.myTransform.position;
        //왼쪽 끝까지 돌진하는 위치 설정
        leftRushPos = new Vector3(bearController.bearMapInfo.mapData.minPosition.x - 3f, bearController.myTransform.position.y, bearController.myTransform.position.z);

        //돌진 후 phase2Pos로 걸어가기위해 순간이동하는 위치 설정
        zTelePos = new Vector3(leftRushPos.x, leftRushPos.y, bearController.bearMapInfo.mapData.maxPosition.z);
        zWalkRotation = Quaternion.Euler(new Vector3(0, -90f, 0));

        //마지막 위치
        phase2Pos = bearController.bearMapInfo.phase2Position;
        phase2Rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    public override void OnEnter()
    {
        canExit = false;
        canGo = true;
        //맵의 왼쪽으로 빠르게 이동하는 함수
        bearController.SetAnimEvent(LeftRush);
        bearController.SetTrigger("Rush_Start");
    }
    public void LeftRush()
    {
        bearController.StartCoroutine(ProcessLeftRush());
    }

    public void GoMove()
    {
        canGo = true;
        bearController.SetAnimEvent(StopMove);
    }
    public void StopMove()
    {
        canGo = false;
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
            timer += Time.deltaTime;
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

        //걷기 준비
        bearController.myTransform.SetPositionAndRotation(zTelePos, zWalkRotation);

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
            timer += Time.deltaTime;
            progress = timer / walkTime;

            bearController.myTransform.position = Vector3.Lerp(zTelePos, phase2Pos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //페이즈 2 포지션까지 도착 완료

        timer = 0f;
        progress = 0f;

        //회전하기
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / rotateTime;

            bearController.myTransform.rotation = Quaternion.Lerp(zWalkRotation, phase2Rotation, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //회전 완료

        bearController.myTransform.rotation = Quaternion.Euler(Vector3.zero);

        bearController.SetTrigger("Rush_Walk_End");
        canExit = true;
    }




    private IEnumerator ProcessChangePhase2()
    {
        yield break;
    }
}

public class BearState_FinalWalk : BearState
{
    Vector3 startPos;
    Vector3 rightWalkPos;
    Vector3 zTelePos;
    Vector3 phase3Pos;

    Quaternion startRotation;
    Quaternion zWalkRotation;
    Quaternion phase3Rotation;

    float timer;
    float progress;
    float walkTime = 2f;
    float rotateTime = 1f;

    bool canGo;
    public BearState_FinalWalk(BearController _bearController)
    {
        bearController = _bearController;


        startPos = bearController.myTransform.position;
        startRotation = bearController.myTransform.rotation;
        //오른쪽 끝으로 걷는 위치 설정
        rightWalkPos = new Vector3(bearController.bearMapInfo.mapData.maxPosition.x + 3f, bearController.myTransform.position.y, bearController.myTransform.position.z);

        //걷기 후 페이즈 3 위치로 걸어가기위해 순간이동하는 위치 설정
        zTelePos = new Vector3(rightWalkPos.x, rightWalkPos.y, bearController.bearMapInfo.mapData.minPosition.z);
        zWalkRotation = Quaternion.Euler(new Vector3(0, -90f, 0));

        //마지막 위치
        phase3Pos = bearController.bearMapInfo.phase3Position;
        phase3Rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
    }
    public override void OnEnter()
    {
        canExit = false;
        canGo = true;
        RightWalk();
        bearController.SetTrigger("Walk_Start");
    }


    public void GoMove()
    {
        canGo = true;
        bearController.SetAnimEvent(StopMove);
    }
    public void StopMove()
    {
        canGo = false;
        bearController.SetAnimEvent(GoMove);
    }

    public void RightWalk()
    {
        bearController.StartCoroutine(ProcessRightWalk());
    }
    private IEnumerator ProcessRightWalk()
    {
        timer = 0f;
        progress = 0f;

        //몸 돌리기
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / rotateTime;

            bearController.myTransform.rotation = Quaternion.Lerp(startRotation, zWalkRotation, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //몸 돌리기 완료

        timer = 0f;
        progress = 0f;
        bearController.SetAnimEvent(GoMove);
        //오른쪽으로 걸어가기
        while (progress < 1f)
        {
            if (!canGo)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
                continue;
            }
            timer += Time.deltaTime;
            progress = timer / walkTime;

            bearController.myTransform.position = Vector3.Lerp(startPos, rightWalkPos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //걸어가기 종료

        yield return YieldInstructionCache.WaitForEndOfFrame;

        bearController.myTransform.SetPositionAndRotation(zTelePos, phase3Rotation);

        timer = 0f;
        progress = 0f;
        bearController.SetAnimEvent(GoMove);
        //페이즈 3 포지션까지 걷기
        while (progress < 1f)
        {
            if (!canGo)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
                continue;
            }
            timer += Time.deltaTime;
            progress = timer / walkTime;

            bearController.myTransform.position = Vector3.Lerp(zTelePos, phase3Pos, progress);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        //도착 완료

        bearController.SetTrigger("Walk_End");
        canExit = true;
    }




    private IEnumerator ProcessChangePhase2()
    {
        yield break;
    }
}

public class BearState_Roar : BearState
{
    WaitForSeconds waitSec = new WaitForSeconds(1f);
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
                break;

            // 중앙 공격
            case eBearState.Roar_B:
                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);
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
    private IEnumerator ProcessAnimEvent_A()
    {
        //Spawn Roar projectile
        int length = bearController.skillValue.roarRandCount;
        for (int i = 0; i < length; i++)
        {
            Vector3 startPos = bearController.bearMapInfo.projectilePositions[bearController.bearMapInfo.projectileRandArray[i]];
            Vector3 endPos = new Vector3(startPos.x, bearController.bearMapInfo.mapData.minPosition.y, startPos.z);

            RoarProjectile roarProjectile = bearController.roarProjectilePool.SpawnThis();
            roarProjectile.Init(startPos, endPos);
            roarProjectile.Move();
        }

        yield break;
    }
    private IEnumerator ProcessAnimEvent_B()
    {
        bearController.skillObjects.roarEffect.SetActive(true);
        yield return waitSec;
        bearController.skillObjects.roarEffect.SetActive(false);


    }
    //IEnumerator ProcessUpdate()
    //{
    //    while (!bearController.animator.GetCurrentAnimatorStateInfo(0).IsName("Phohyo.Start_Phohyo"))
    //    {
    //        yield return null;
    //    }
    //    Debug.Log("포효 애니메이션 진입");

    //    while (bearController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //    {
    //        yield return null;
    //    }
    //    Debug.Log("포효 애니메이션 끝");
    //    canExit = true;
    //}
}
public class BearState_Strike : BearState
{
    public BearState_Strike(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;

        switch ((eBearState)bearController.stateInfo.stateInt)
        {
            case eBearState.Strike_A:
                ShuffleArray();
                bearController.SetAnimEvent(AnimEvent_A);
                bearController.SetSkillVariety(0);
                break;

            case eBearState.Strike_B:
                bearController.SetAnimEvent(AnimEvent_B);
                bearController.SetSkillVariety(1);
                break;

            case eBearState.Strike_C:
                bearController.SetAnimEvent(AnimEvent_C);
                bearController.SetSkillVariety(0);
                break;

            default:
                break;
        }

        bearController.SetTrigger("Strike_Start");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    private int[] strikePos;
    //랜덤
    private void ShuffleArray()
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

    private void CloneStrikeCube(int _bearBlockIndex)
    {
        GameObject.Instantiate(bearController.skillObjects.strikeCube, bearController.bearMapInfo.bearBlocks[_bearBlockIndex].position.groundCenter, Quaternion.identity);
    }
    //랜덤
    private void AnimEvent_A()
    {
        CloneStrikeCube(strikePos[0]);
        CloneStrikeCube(strikePos[1]);
        CloneStrikeCube(strikePos[2]);
    }

    //퍼지기
    private void AnimEvent_B()
    {
        //strikePos = new int[5] { 0, 1, 2, 3, 4};
        bearController.StartCoroutine(ProcessAnimEvent_B());
    }

    //우다다
    private void AnimEvent_C()
    {
        bearController.StartCoroutine(ProcessAnimEvent_C());
    }


    WaitForSeconds waitBSec = new WaitForSeconds(0.3f);
    WaitForSeconds waitCSec = new WaitForSeconds(0.3f);
    private IEnumerator ProcessAnimEvent_B()
    {

        CloneStrikeCube(1);
        CloneStrikeCube(3);
        yield return waitBSec;
        CloneStrikeCube(0);
        CloneStrikeCube(4);
    }

    private IEnumerator ProcessAnimEvent_C()
    {

        for (int i = 3; i > -1; i--)
        {
            CloneStrikeCube(i);
            yield return waitCSec;

        }

    }

}
public class BearState_Claw : BearState
{
    int random = 0;
    public BearState_Claw(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;

        switch ((eBearState)bearController.stateInfo.stateInt)
        {
            case eBearState.Claw_A:
                bearController.SetAnimEvent(AnimEvent_A);
                bearController.SetSkillVariety(0);
                break;

            case eBearState.Claw_B:

                Vector3 tempClawPos = bearController.skillObjects.clawUnderPosition.position;
                //bearController.skillObjects.claw_B_Effect.transform.position = new Vector3(tempClawPos.x, tempClawPos.y + 1f, tempClawPos.z);
                bearController.skillObjects.claw_B_Effect.transform.position = new Vector3(tempClawPos.x, tempClawPos.y, tempClawPos.z);
                SetRandomVariety();
                bearController.SetAnimEvent(AnimEvent_B);
                break;

            case eBearState.Claw_C:
                bearController.SetSkillVariety(1);
                bearController.SetAnimEvent(AnimEvent_C);
                break;

            default:
                break;
        }
        bearController.SetTrigger("Claw_Start");
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
    public void AnimEvent_C()
    {
        bearController.StartCoroutine(ProcessAnimEvent_C());
    }

    private IEnumerator ProcessAnimEvent_A()
    {
        bearController.skillObjects.claw_A_Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bearController.skillObjects.claw_A_Effect.SetActive(false);
    }

    private void SetRandomVariety()
    {
        random = Random.Range(0, 2);

        if (random == 0)
        {
            bearController.SetSkillVariety(0);
        }
        else
        {
            bearController.SetSkillVariety(0.5f);

        }
    }
    private float rotVal = 70f;
    private IEnumerator ProcessAnimEvent_B()
    {

        if (random == 0)
        {
            bearController.skillObjects.claw_B_Effect.transform.rotation = Quaternion.Euler(0, 0, rotVal);
        }
        else
        {
            bearController.skillObjects.claw_B_Effect.transform.rotation = Quaternion.Euler(0, 0, -rotVal);
        }
        bearController.skillObjects.claw_B_Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bearController.skillObjects.claw_B_Effect.SetActive(false);
    }

    private IEnumerator ProcessAnimEvent_C()
    {
        AnimEvent_A();

        //Spawn Claw projectile

        WaitForSeconds waitDelay = new WaitForSeconds(bearController.skillValue.clawDelay);

        int length = bearController.skillValue.clawCount;

        for (int i = 0; i < length; i++)
        {
            ClawProjectile clawProjectile = bearController.clawProjectilePool.SpawnThis();

            Vector3 startPos = Quaternion.Euler(0, 0, clawProjectile.degree) * bearController.skillObjects.clawUnderPosition.position;
            Vector3 endPos = new Vector3(bearController.bearMapInfo.mapData.minPosition.x, startPos.y, startPos.z);

            clawProjectile.Init(startPos, endPos);
            clawProjectile.Move();

            yield return waitDelay;
        }

        yield break;
    }
}
public class BearState_Smash : BearState
{
    public BearState_Smash(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.bearMapInfo.UpdateProjectileRandArray();
        bearController.SetAnimEvent(ActiveHandRock);
        bearController.SetTrigger("Smash_Start");

        //bearController.skillObjects.smashRock.SetActive(true);
        //bearController.StartCoroutine(ProcessUpdate());
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public void ActiveHandRock()
    {
        bearController.skillObjects.smashRock.transform.SetPositionAndRotation(bearController.skillObjects.handTransform.position, bearController.skillObjects.handTransform.rotation);
        bearController.skillObjects.smashRock.SetActive(true);
        bearController.SetAnimEvent(AnimEvent);
    }
    public void AnimEvent()
    {

        bearController.StartCoroutine(ProcessAnimEvent());
    }

    WaitForSeconds waitSec = new WaitForSeconds(1f);
    private IEnumerator ProcessAnimEvent()
    {
        //바위 없애기
        bearController.skillObjects.smashRock.SetActive(false);


        Vector3 startPos = bearController.skillObjects.smashRock.transform.position;

        //바위 쿠과광
        int length = bearController.skillValue.smashRandCount;
        for (int i = 0; i < length; i++)
        {
            Vector3 midPos = bearController.bearMapInfo.projectilePositions[bearController.bearMapInfo.projectileRandArray[i]];
            // midPos = new Vector3(midPos.x, midPos.y, midPos.z);
            Vector3 endPos = new Vector3(midPos.x, bearController.bearMapInfo.mapData.minPosition.y, midPos.z);

            SmashProjectile smashProjectile = bearController.smashProjectilePool.SpawnThis();
            smashProjectile.Init(startPos, midPos, endPos);
            smashProjectile.Move();
        }

        yield break;
    }
}
public class BearState_Concentrate : BearState
{
    private IEnumerator concentrate;

    private Transform sphereTransform;
    private BearConcentrateHelper helper;
    public BearState_Concentrate(BearController _bearController)
    {
        bearController = _bearController;
        sphereTransform = bearController.skillObjects.concentrateSphere.transform;
        helper = bearController.skillObjects.concentrateHelper;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Concentrate_Start");
        bearController.skillObjects.concentrateSphere.SetActive(true);
        concentrate = ProcessConcentrate();
    }
    public override void OnExit()
    {

    }
    public void AnimEvent()
    {
        helper.StartCheck();
        Debug.Log("StartCheck!");

        bearController.StartCoroutine(ProcessChangeSphere());
        bearController.StartCoroutine(concentrate);
    }
    private IEnumerator ProcessChangeSphere()
    {
        float timer = 0f;
        float maxTime = bearController.skillValue.concentrateTime;
        float progress = 0f;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / maxTime;

            //점점 커지기~
            sphereTransform.localScale = new Vector3(0.4f + progress, 0.4f + progress, 0.4f + progress);

            yield return null;
        }
    }


    private IEnumerator ProcessConcentrate()
    {
        float timer = 0f;
        float maxTime = bearController.skillValue.concentrateTime + 1f;
        float progress = 0f;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / maxTime;

            if (helper.isSucceedParry)
            {
                ChangeStatePowerless();
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        bearController.SetTrigger("Concentrate_End");
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
    WaitForSeconds waitSecBegin;
    WaitForSeconds waitSecEnd;
    public BearState_Powerless(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.SetAnimEvent(AnimEvent_WaitEnd);

        waitSecBegin = new WaitForSeconds(bearController.skillValue.powerlessTime);
        waitSecEnd = new WaitForSeconds(bearController.currentPattern.waitTime);

        bearController.SetTrigger("Powerless_Start");
        bearController.StartCoroutine(ProcessAnimEvent_Begin());
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void AnimEvent_WaitEnd()
    {
        bearController.StartCoroutine(ProcessWaitTime());
    }

    private IEnumerator ProcessAnimEvent_Begin()
    {
        //대기
        yield return waitSecBegin;
        bearController.SetTrigger("Powerless_End");
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
        bearController.SetAnimEvent(AnimEvent);
        bearController.SetTrigger("Die_Start");
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void AnimEvent()
    {
        bearController.animator.enabled = false;
    }
}



#endregion



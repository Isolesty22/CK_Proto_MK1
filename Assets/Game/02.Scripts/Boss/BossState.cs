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
        bearController.SetTrigger("Start_Idle");
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
        bearController.SetSkillAction(SkillAction);
        bearController.SetTrigger("Start_Stamp");
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

    public void SkillAction()
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
    public BearState_Rush(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.SetTrigger("Start_Rush");
        //bearController.StartCoroutine(ProcessDoljin());
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

    private IEnumerator ProcessDoljin()
    {
        float timer = 0f;
        float doljinTime = 5f;
        while (timer < doljinTime)
        {
            timer += Time.deltaTime;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        canExit = true;
    }




    private IEnumerator ProcessChangePhase2()
    {
        yield break;
    }
}
public class BearState_Roar : BearState
{
    public BearState_Roar(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.bearMapInfo.UpdateProjectileRandArray();
        bearController.SetSkillAction(SkillAction);
        bearController.SetTrigger("Start_Roar");
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
    public void SkillAction()
    {

        bearController.StartCoroutine(ProcessSkillAction());
    }

    private IEnumerator ProcessSkillAction()
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

        switch (bearController.stateInfo.stateE)
        {
            case eBossState.BearState_Strike_A:
                ShuffleArray();
                bearController.SetSkillAction(SkillAction_A);
                break;

            case eBossState.BearState_Strike_B:
                bearController.SetSkillAction(SkillAction_B);
                break;

            case eBossState.BearState_Strike_C:
                bearController.SetSkillAction(SkillAction_C);
                break;

            default:
                break;
        }

        bearController.SetTrigger("Start_Strike");
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
    private void SkillAction_A()
    {
        CloneStrikeCube(strikePos[0]);
        CloneStrikeCube(strikePos[1]);
        CloneStrikeCube(strikePos[2]);
    }

    //퍼지기
    private void SkillAction_B()
    {
        //strikePos = new int[5] { 0, 1, 2, 3, 4};
        bearController.StartCoroutine(ProcessSkillAction_B());
    }

    //우다다
    private void SkillAction_C()
    {
        bearController.StartCoroutine(ProcessSkillAction_C());
    }


    WaitForSeconds waitBSec = new WaitForSeconds(0.3f);
    WaitForSeconds waitCSec = new WaitForSeconds(0.3f);
    private IEnumerator ProcessSkillAction_B()
    {

        CloneStrikeCube(1);
        CloneStrikeCube(3);
        yield return waitBSec;
        CloneStrikeCube(0);
        CloneStrikeCube(4);
    }

    private IEnumerator ProcessSkillAction_C()
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
    public BearState_Claw(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;

        switch (bearController.stateInfo.stateE)
        {
            case eBossState.BearState_Claw_A:
                bearController.SetSkillAction(SkillAction_A);
                break;

            case eBossState.BearState_Claw_B:

                Vector3 tempClawPos = bearController.skillObjects.clawUnderPosition.position;
                //bearController.skillObjects.claw_B_Effect.transform.position = new Vector3(tempClawPos.x, tempClawPos.y + 1f, tempClawPos.z);
                bearController.skillObjects.claw_B_Effect.transform.position = new Vector3(tempClawPos.x, tempClawPos.y, tempClawPos.z);

                bearController.SetSkillAction(SkillAction_B);
                break;

            case eBossState.BearState_Claw_C:
                bearController.SetSkillAction(SkillAction_C);
                break;

            default:
                break;
        }
        bearController.SetTrigger("Start_Claw");
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

    public void SkillAction_A()
    {
        bearController.StartCoroutine(ProcessSkillAction_A());
    }
    public void SkillAction_B()
    {
        bearController.StartCoroutine(ProcessSkillAction_B());
    }
    public void SkillAction_C()
    {
        bearController.StartCoroutine(ProcessSkillAction_C());
    }

    private IEnumerator ProcessSkillAction_A()
    {
        bearController.skillObjects.claw_A_Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bearController.skillObjects.claw_A_Effect.SetActive(false);
    }
    private float rotVal = 70f;
    private IEnumerator ProcessSkillAction_B()
    {
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            bearController.skillObjects.claw_B_Effect.transform.rotation = Quaternion.Euler(0, 0, -rotVal);
        }
        else
        {
            bearController.skillObjects.claw_B_Effect.transform.rotation = Quaternion.Euler(0, 0, rotVal);

        }
        bearController.skillObjects.claw_B_Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bearController.skillObjects.claw_B_Effect.SetActive(false);
    }

    private IEnumerator ProcessSkillAction_C()
    {
        SkillAction_A();

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
        bearController.SetSkillAction(SkillAction);
        bearController.SetTrigger("Start_Smash");

        bearController.skillObjects.smashRock.SetActive(true);
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
    public void SkillAction()
    {

        bearController.StartCoroutine(ProcessSkillAction());
    }

    WaitForSeconds waitSec = new WaitForSeconds(1f);
    private IEnumerator ProcessSkillAction()
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


public class BearState_Die : BearState
{
    public BearState_Die(BearController _bearController)
    {
        bearController = _bearController;
    }
    public override void OnEnter()
    {
        canExit = false;
        bearController.SetSkillAction(SkillAction);
        bearController.SetTrigger("Start_Die");
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

    public void SkillAction()
    {
        bearController.gameObject.SetActive(false);
    }
}




#endregion



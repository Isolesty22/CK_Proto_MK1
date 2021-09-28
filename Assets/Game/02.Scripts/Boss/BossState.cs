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
        canExit = true;
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
    IEnumerator ProcessUpdate()
    {
        while (!bearController.animator.GetCurrentAnimatorStateInfo(0).IsName("Phohyo.Start_Phohyo"))
        {
            yield return null;
        }
        Debug.Log("포효 애니메이션 진입");

        while (bearController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        Debug.Log("포효 애니메이션 끝");
        canExit = true;
    }
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
        bearController.SetSkillAction(Skill_Strike);
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

    public void Skill_Strike()
    {
        GameObject.Instantiate(bearController.skillObjects.strikeCube, bearController.bearMapInfo.bearBlocks[0].position.groundCenter, Quaternion.identity);
        GameObject.Instantiate(bearController.skillObjects.strikeCube, bearController.bearMapInfo.bearBlocks[2].position.groundCenter, Quaternion.identity);
        GameObject.Instantiate(bearController.skillObjects.strikeCube, bearController.bearMapInfo.bearBlocks[3].position.groundCenter, Quaternion.identity);
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
}
#endregion



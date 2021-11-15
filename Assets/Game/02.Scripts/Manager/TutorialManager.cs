using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public float scriptTime;

    public ArriveCheck jumpZone;
    public ArriveCheck crouchZone;
    public ArriveCheck attackZone;
    public ArriveCheck lookUpZone;
    public ArriveCheck parryZone;
    public ArriveCheck GoodZone;
    public ArriveCheck heavyAttackZone;
    public ArriveCheck ultimateZone;
    public ArriveCheck EndZone;

    public Transform endPoint;

    private IEnumerator parry;
    private IEnumerator good;
    private IEnumerator heavyAttack;

    void Start()
    {
        //init
        jumpZone.arrive += ArriveJumpZone;
        crouchZone.arrive += ArriveCrouchZone;
        attackZone.arrive += ArriveAttackZone;
        lookUpZone.arrive += ArriveLookUpZone;
        parryZone.arrive += ArriveParryZone;
        GoodZone.arrive += ArriveGoodZone;
        heavyAttackZone.arrive += ArriveHeavyAttackZone;
        ultimateZone.arrive += ArriveUltimateZone;
        EndZone.arrive += ArriveEndZone;

        GameManager.instance.playerController.State.moveSystem = true;
        GameManager.instance.playerController.Stat.hp = 1000;
        GameManager.instance.playerController.Com.pixy.getPixy = false;
        var start = StartScript();
        StartCoroutine(start);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartScript()
    {
        yield return new WaitForSeconds(2f);

        //UIManager.Instance.TalkInfinity(900);

        UIManager.Instance.TalkInfinity(900);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(901);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(902);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(903);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(904);

        GameManager.instance.playerController.State.moveSystem = false;
        GameManager.instance.playerController.Com.pixy.getPixy = true;
    }

    void ArriveJumpZone()
    {
        UIManager.Instance.TalkInfinity(905);

        jumpZone.gameObject.SetActive(false);
    }
    
    void ArriveCrouchZone()
    {
        UIManager.Instance.TalkInfinity(906);

        crouchZone.gameObject.SetActive(false);
    }

    void ArriveAttackZone()
    {
        UIManager.Instance.TalkInfinity(907);

        attackZone.gameObject.SetActive(false);
    }

    void ArriveLookUpZone()
    {
        UIManager.Instance.TalkInfinity(908);

        lookUpZone.gameObject.SetActive(false);
    }
    
    void ArriveParryZone()
    {
        parry = ParryZoneScript();
        StartCoroutine(parry);

        parryZone.gameObject.SetActive(false);
    }

    IEnumerator ParryZoneScript()
    {
        UIManager.Instance.TalkInfinity(909);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(910);
    }

    void ArriveGoodZone()
    {
        StopCoroutine(parry);

        good = GoodZoneScript();
        StartCoroutine(good);

        GoodZone.gameObject.SetActive(false);
    }

    IEnumerator GoodZoneScript()
    {
        GameManager.instance.playerController.FillFullEnerge();

        UIManager.Instance.TalkInfinity(911);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(912);
        //yield return new WaitForSeconds(scriptTime);
        //UIManager.Instance.TalkInfinity(911);
    }

    void ArriveHeavyAttackZone()
    {
        StopCoroutine(good);

        heavyAttack = HeavyAttackZoneScript();
        StartCoroutine(heavyAttack);

        heavyAttackZone.gameObject.SetActive(false);
    }

    IEnumerator HeavyAttackZoneScript()
    {
        GameManager.instance.playerController.InputVal.movementInput = 0f;
        GameManager.instance.playerController.State.moveSystem = true;

        UIManager.Instance.TalkInfinity(913);
        yield return new WaitForSeconds(scriptTime);
        UIManager.Instance.TalkInfinity(914);
        yield return new WaitForSeconds(scriptTime);

        GameManager.instance.playerController.State.moveSystem = false;
    }

    void ArriveUltimateZone()
    {
        StopCoroutine(heavyAttack);

        UIManager.Instance.TalkInfinity(915);

        ultimateZone.gameObject.SetActive(false);
    }
    
    void ArriveEndZone()
    {
        var end = Ending();
        StartCoroutine(end);

        EndZone.gameObject.SetActive(false);
    }

    IEnumerator Ending()
    {
        //GameManager.instance.playerController.InputVal.movementInput = 0f;
        //GameManager.instance.playerController.State.moveSystem = true;
        GameManager.instance.playerController.MoveSystem(endPoint.position);

        yield return new WaitForSeconds(3f);

        SceneChanger.Instance.LoadThisScene(SceneNames.fieldMap);
    }
}

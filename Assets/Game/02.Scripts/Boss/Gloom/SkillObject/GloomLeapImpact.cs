using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomLeapImpact : MonoBehaviour
{
    private PlayerController player;

    private WaitForSeconds waitDuration = null;

    private WaitForSeconds createInterval = new WaitForSeconds(0.2f);

    public GameObject leapImpactFlame;
    public VfxActiveHelper[] vfxHelpers;

    public GloomController gloom;

    public Collider myCollider;

    private int effectsCount;

    private void Start()
    {
        player = GameManager.instance.playerController;

        waitDuration = new WaitForSeconds(gloom.SkillVal.leap.leapImpactDuration);

        myCollider.enabled = false;

        effectsCount = vfxHelpers.Length;

        for (int i = 0; i < effectsCount; i++)
        {
            vfxHelpers[i].gameObject.SetActive(false);

            //지속시간 설정
            vfxHelpers[i].SetActiveTime(waitDuration);
        }
    }

    public void StartImpact()
    {

        StartCoroutine(ProcessImpact());
    }

    private IEnumerator ProcessImpact()
    {
        //임팩트 불꽃 켜기
        leapImpactFlame.SetActive(true);

        for (int i = 0; i < effectsCount; i++)
        {
            vfxHelpers[i].Active();
            yield return createInterval;
        }

        //콜라이더 활성화
        myCollider.enabled = true;

        //지속시간동안 대기
        yield return waitDuration;

        //임팩트 플레임 비활성화
        leapImpactFlame.SetActive(false);

        //콜라이더 비활성화
        myCollider.enabled = false;


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            if (!player.IsInvincible())
            {
                player.Hit();
            }
        }
    }
}

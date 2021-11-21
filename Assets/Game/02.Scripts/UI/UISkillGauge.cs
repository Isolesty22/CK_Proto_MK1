using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 엄청나게 하드코딩된 스킬 게이지바
/// </summary>
public class UISkillGauge : MonoBehaviour
{

    [System.Serializable]
    public struct FlowerValue
    {
        [Tooltip("뾰잉 커브")]
        public AnimationCurve scaleCurve;

        [Tooltip("꽃의 색이 바뀌기만 할 때, 혹은 꽃이 커지거나 작아질 때 슬쩍 들어가는 페이드 효과에 걸리는 시간입니다.")]
        public float time_fade;

        [Space(5)]

        [Tooltip("꽃이 커질 때 걸리는 시간")]
        public float time_on;

        [Tooltip("꽃이 작아질 떄 걸리는 시간")]
        public float time_off;

        [Tooltip("꽃이 주황색으로 변하는 시간")]
        public float time_highlightOn;

        [Tooltip("궁극기를 사용한 이후 주황색 꽃이 사라지는 시간")]
        public float time_highlightOff;

    }

    public FlowerValue flowerValue;
    //public Slider lightGaugeSlider;

    private float maxGauge;
    public float currentGauge;
    private float currentAmount;
    private PlayerController playerController;


    public Image skillGaugeBar;
    public UISkillFlower[] skillFlowers = new UISkillFlower[3];

    private void Start()
    {
        playerController = GameManager.instance.playerController;
        currentGauge = playerController.Stat.pixyEnerge;
        maxGauge = 30f;
        currentAmount = playerController.Stat.pixyEnerge / maxGauge;

        skillFlowers[0].flowerValue = flowerValue;
        skillFlowers[1].flowerValue = flowerValue;
        skillFlowers[2].flowerValue = flowerValue;

        UpdateGauge_New();
    }
    private void Update()
    {
        //if (currentGauge != playerController.Stat.pixyEnerge)
        //{
            UpdateGauge_New();
        //}

    }

    private void UpdateImages()
    {
        currentGauge = playerController.Stat.pixyEnerge;
        currentAmount = currentGauge / maxGauge;
        skillGaugeBar.fillAmount = currentAmount;
    }
    private void UpdateGauge_New()
    {

        //현재 게이지가 실제보다 적을때...켜줘야하나!
        if (currentGauge < playerController.Stat.pixyEnerge)
        {
            if (playerController.Stat.pixyEnerge > 29.95f)
            {
                if (skillFlowers[2].currentState != UISkillFlower.eState.blueOn)
                {
                    skillFlowers[2].SetState(UISkillFlower.eState.blueOn);
                    skillFlowers[2].On();
                    //UpdateImages();
                    return;
                }
                //마지막 꽃 애니메이션이 끝나면 전부 강조표시
                if (skillFlowers[2].currentState == UISkillFlower.eState.blueOn && skillFlowers[2].isPlaying == false)
                {
                    skillFlowers[0].SetState(UISkillFlower.eState.orangeOn);
                    skillFlowers[1].SetState(UISkillFlower.eState.orangeOn);
                    skillFlowers[2].SetState(UISkillFlower.eState.orangeOn);

                    skillFlowers[0].HighlightOn();
                    skillFlowers[1].HighlightOn();
                    skillFlowers[2].HighlightOn();
                    UpdateImages();
                }
                return;
            }
            if (playerController.Stat.pixyEnerge > 19.95f)
            {
                if (skillFlowers[1].currentState != UISkillFlower.eState.blueOn)
                {
                    skillFlowers[1].SetState(UISkillFlower.eState.blueOn);
                    skillFlowers[1].On();
                }
                UpdateImages();
                return;
            }
            if (playerController.Stat.pixyEnerge > 9.95f)
            {
                if (skillFlowers[0].currentState != UISkillFlower.eState.blueOn)
                {
                    skillFlowers[0].SetState(UISkillFlower.eState.blueOn);
                    skillFlowers[0].On();
                }
                UpdateImages();
                return;
            }
        }

        //실제보다 클 때...꺼줘야하나!
        if (currentGauge > playerController.Stat.pixyEnerge)
        {
            if (playerController.Stat.pixyEnerge < 9.95f)
            {
                if (skillFlowers[2].currentState != UISkillFlower.eState.blueOff)
                {
                    skillFlowers[1].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[2].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[1].HighlightOff();
                    skillFlowers[2].HighlightOff();
                }
                if (skillFlowers[0].currentState == UISkillFlower.eState.orangeOn)
                {
                    skillFlowers[0].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[1].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[2].SetState(UISkillFlower.eState.blueOff);

                    skillFlowers[0].HighlightOff();
                    skillFlowers[1].HighlightOff();
                    skillFlowers[2].HighlightOff();
                    UpdateImages();
                    return;
                }

                if (skillFlowers[0].currentState != UISkillFlower.eState.blueOff)
                {
                    skillFlowers[0].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[0].Off();
                }
                UpdateImages();
                return;
            }

            if (playerController.Stat.pixyEnerge < 19.95f)
            {
                if (skillFlowers[1].currentState != UISkillFlower.eState.blueOff)
                {
                    skillFlowers[1].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[1].Off();
                }
                UpdateImages();
                return;
            }
            if (playerController.Stat.pixyEnerge < 29.95f)
            {
                if (skillFlowers[2].currentState == UISkillFlower.eState.orangeOn)
                {
                    skillFlowers[2].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[1].SetState(UISkillFlower.eState.blueOn);
                    skillFlowers[0].SetState(UISkillFlower.eState.blueOn);

                    skillFlowers[2].HighlightOff();
                    skillFlowers[1].HighlightEnd();
                    skillFlowers[0].HighlightEnd();
                    UpdateImages();
                    return;
                }
                if (skillFlowers[2].currentState != UISkillFlower.eState.blueOff)
                {
                    skillFlowers[2].SetState(UISkillFlower.eState.blueOff);
                    skillFlowers[2].Off();
                }
                UpdateImages();
                return;
            }

        }
        UpdateImages();
    }
}

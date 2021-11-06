using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillGauge : MonoBehaviour
{
    public Image skillGaugeBar;
    public Image[] skillImageArr = new Image[3];
    //public Slider lightGaugeSlider;

    private float maxGauge;
    public float currentGauge;
    private float currentAmount;
    private PlayerController playerController;
    private void Start()
    {
        playerController = GameManager.instance.playerController;
        maxGauge = 30f;
        currentAmount = playerController.Stat.pixyEnerge / maxGauge;

        for (int i = 0; i < 3; i++)
        {
            //다 꺼버리기
            skillImageArr[i].enabled = false;
        }

        UpdateGauge();
    }
    private void Update()
    {
        if (currentGauge != playerController.Stat.pixyEnerge)
        {
            UpdateGauge();
        }

    }
    private void UpdateGauge()
    {
        currentGauge = playerController.Stat.pixyEnerge;

        currentAmount = currentGauge / maxGauge;
        skillGaugeBar.fillAmount = currentAmount;

        for (int i = 0; i < 3; i++)
        {
            skillImageArr[i].enabled = true;
        }

        if (currentGauge < 29.95f)
        {
            skillImageArr[2].enabled = false;

            if (currentGauge < 19.95f)
            {
                skillImageArr[1].enabled = false;

                if (currentGauge < 9.95f)
                {
                    skillImageArr[0].enabled = false;
                }
            }
        }

    }
}

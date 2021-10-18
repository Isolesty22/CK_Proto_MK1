using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILightGauge : MonoBehaviour
{
    public Slider lightGaugeSlider;
    public PlayerController playerController;
    private void Start()
    {
        playerController = GameManager.instance.playerController;
    }
    private void Update()
    {
        if (lightGaugeSlider.value != playerController.Stat.pixyEnerge)
        {
            lightGaugeSlider.value = playerController.Stat.pixyEnerge;
        }

    }
    public void OnValueChanged()
    {
        if (lightGaugeSlider.value != playerController.Stat.pixyEnerge)
        {
            playerController.Stat.pixyEnerge = lightGaugeSlider.value;
        }
    }
}

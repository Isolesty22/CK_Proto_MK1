using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIBossHP : MonoBehaviour
{
    public BossController bossController;
    public Image hpImage;
    private float maxHP;
    void Start()
    {
        bossController = FindObjectOfType<BossController>();
        maxHP = bossController.hp;

    }

    private void Update()
    {
        hpImage.fillAmount = bossController.hp / maxHP;
    }
}

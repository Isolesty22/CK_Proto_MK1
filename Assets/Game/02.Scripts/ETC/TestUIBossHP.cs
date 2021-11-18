using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestUIBossHP : MonoBehaviour
{
    public BossController bossController;
    public Image hpImage;
    private float maxHP;
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            bossController = FindObjectOfType<BossController>();
            maxHP = bossController.hp;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        hpImage.fillAmount = bossController.hp / maxHP;
    }
}

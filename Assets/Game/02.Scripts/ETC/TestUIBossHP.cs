using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestUIBossHP : UIBase
{
    public BossController bossController;
    public Image hpImage;
    private float maxHP;


    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        CheckOpen();

        Com.canvas.enabled = false;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            bossController = FindObjectOfType<BossController>();
            maxHP = bossController.hp;
        }
    }
    void Start()
    {
        GameManager.instance.timelineManager.onTimelineEnded += OnTimelineEnded;
    }

    public void OnTimelineEnded()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            StartCoroutine(ProcessOpen());
        }
    }



    private void Update()
    {
        hpImage.fillAmount = bossController.hp / maxHP;
    }
}

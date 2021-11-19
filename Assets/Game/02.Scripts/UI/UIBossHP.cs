using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIBossHP : UIBase
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
        if (SceneManager.GetActiveScene().name == "Stage_02" || SceneManager.GetActiveScene().name == "Stage_0")
        {
            gameObject.SetActive(true);
            GameManager.instance.timelineManager.onTimelineEnded += OnTimelineEnded;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTimelineEnded()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            StartCoroutine(ProcessOpen());
        }
    }

    float timer = 0f;
    float progres = 1f;


    private void Update()
    {
        timer += Time.deltaTime * 0.01f;
        progres = Mathf.Lerp(progres, bossController.hp / maxHP, timer);
        hpImage.fillAmount = progres;

        // hpImage.fillAmount = bossController.hp / maxHP;
    }
}

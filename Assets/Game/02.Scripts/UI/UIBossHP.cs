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
    private string sceneName;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        CheckOpen();
        sceneName = SceneManager.GetActiveScene().name;

        Debug.Log("!!" + sceneName);
        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            gameObject.SetActive(true);
            bossController = FindObjectOfType<BossController>();
            maxHP = bossController.hp;
        }
        else
        {
            gameObject.SetActive(false);
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

    float timer = 0f;
    float progres = 1f;


    public override bool Close()
    {
        if (isOpen)
        {
            StartCoroutine(ProcessClose());
            return true;
        }
        else
        {
            return false;
        }

    }
    protected override IEnumerator ProcessClose()
    {
        yield return StartCoroutine(base.ProcessClose());
        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime * 0.01f;
        progres = Mathf.Lerp(progres, bossController.hp / maxHP, timer);
        hpImage.fillAmount = progres;

        // hpImage.fillAmount = bossController.hp / maxHP;
    }
}

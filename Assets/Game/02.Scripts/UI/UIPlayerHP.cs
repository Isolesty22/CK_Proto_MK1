using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [Tooltip("testCurrentHp=>playerController.Stat.hp")]
    public int testCurrentHP;
    public Image[] hpImageList;

    private PlayerController playerController;

    private int hpCount;

    private int currentHP;
    private void Start()
    {
        Init();
        UpdateUI();
    }

    private void Init()
    {
        //playerController = GameManager.instance.playerController;
        hpCount = testCurrentHP;
        UpdateCurrentHP();
    }

    private void Update()
    {
        if (CheckHP())
        {
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateCurrentHP();

        if (hpCount == currentHP)
        {
            return;
        }

        if (hpCount > currentHP)
        {
            hpImageList[currentHP].gameObject.SetActive(false);
        }
        else
        {
            hpImageList[currentHP].gameObject.SetActive(true);
        }

        hpCount = currentHP;
    }

    private bool CheckHP()
    {
        return testCurrentHP == hpCount;
    }
    private void UpdateCurrentHP()
    {
        currentHP = testCurrentHP;
    }
}

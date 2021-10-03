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
        playerController = GameManager.instance.playerController;
        hpCount = playerController.Stat.hp;
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
            if (currentHP >= 0)
            {
                hpImageList[currentHP].gameObject.SetActive(false);
            }
            else
            {
                UIManager.Instance.OpenLosePopup();
            }
        }
        else
        {
            hpImageList[currentHP].gameObject.SetActive(true);
        }

        hpCount = currentHP;
    }

    private bool CheckHP()
    {
        return playerController.Stat.hp == hpCount;
    }
    private void UpdateCurrentHP()
    {
        currentHP = playerController.Stat.hp;
    }
}

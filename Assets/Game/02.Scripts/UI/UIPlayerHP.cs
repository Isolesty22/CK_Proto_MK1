using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [Tooltip("testCurrentHp=>playerController.Stat.hp")]
    public int testCurrentHP;

    public Sprite hp_on;
    public Sprite hp_off;
    
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

        int length = hpImageList.Length;
        for (int i = 0; i < length; i++)
        {
            hpImageList[i].sprite = hp_on;
        }

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
            if (currentHP > 0)
            {
                hpImageList[currentHP].sprite = hp_off;
            }
            else
            {
                hpImageList[0].sprite = hp_off;
                UIManager.Instance.OpenLosePopup();
            }
        }
        else
        {
            hpImageList[currentHP].sprite = hp_on;
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

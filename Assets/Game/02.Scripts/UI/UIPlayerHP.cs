using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [Tooltip("testCurrentHp=>playerController.Stat.hp")]
    public int testCurrentHP;


    [System.Serializable]
    public class AdditiveImages
    {
        public Image backgroundImage;
        public Image[] hpImageList;


        [Header("HP Sprite")]
        public Sprite hp_on;
        public Sprite hp_off;

        [Header("플레이어 상태 Sprite")]
        public Sprite basicBG;
        public Sprite hurtBG;
    }


    [SerializeField] private AdditiveImages additiveImages = new AdditiveImages();
    public AdditiveImages Images => additiveImages;

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

        int length = Images.hpImageList.Length;
        for (int i = 0; i < length; i++)
        {
            Images.hpImageList[i].sprite = Images.hp_on;
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
                if (currentHP > 3)
                    return;
                Images.hpImageList[currentHP].sprite = Images.hp_off;
                StartCoroutine(ProcessHurt());
            }
            else
            {
                Images.hpImageList[0].sprite = Images.hp_off;
                UIManager.Instance.OpenLosePopup();
            }
        }
        else
        {
            Images.hpImageList[currentHP].sprite = Images.hp_on;
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

    WaitForSeconds waitSec = new WaitForSeconds(1f);
    private IEnumerator ProcessHurt()
    {
        Images.backgroundImage.sprite = Images.hurtBG;
        yield return waitSec;
        Images.backgroundImage.sprite = Images.basicBG;

    }
}

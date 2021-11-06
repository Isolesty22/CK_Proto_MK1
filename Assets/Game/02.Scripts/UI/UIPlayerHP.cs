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

        public Image[] hpImageArr;


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

    private int currentHP;

    private int uiHP;
    private void Start()
    {
        Init();
        UpdateUI();
    }

    private void Init()
    {
        playerController = GameManager.instance.playerController;

        currentHP = 3;
        uiHP = 3;

        int length = Images.hpImageArr.Length;

        for (int i = 0; i < length; i++)
        {
            Images.hpImageArr[i].sprite = Images.hp_on;
        }

        UpdateCurrentHP();
    }

    private void Update()
    {
        if (IsHpChanged())
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        UpdateCurrentHP();

        //해야함 : 디버그용 예외처리. 실제 빌드 시 지울 것 
        if (currentHP > 3)
        {
            return;
        }
        //UI상의 HP와 현재 HP가 같으면 return
        if (currentHP == uiHP)
        {
            return;
        }

        if (currentHP < uiHP)
        {
            if (currentHP > 0)
            {
                if (currentHP > 3)
                    return;
                Images.hpImageArr[currentHP].sprite = Images.hp_off;
                StartCoroutine(ProcessHurt());
            }
            else
            {
                Images.hpImageArr[0].sprite = Images.hp_off;
                StartCoroutine(ProcessOpenLosePopup());
            }
        }
        else
        {
            Images.hpImageArr[currentHP].sprite = Images.hp_on;
        }
        uiHP = currentHP;
    }

    /// <summary>
    ///ui상의 HP와 플레이어의 HP가 다르면 True를 반환합니다.
    /// </summary>

    private bool IsHpChanged()
    {
        return playerController.Stat.hp != uiHP;
    }

    /// <summary>
    /// 플레이어의 HP를 받아서 currentHP에 저장합니다.
    /// </summary>
    private void UpdateCurrentHP()
    {
        currentHP = playerController.Stat.hp;
    }

    WaitForSeconds waitSec = new WaitForSeconds(0.5f);
    private IEnumerator ProcessHurt()
    {
        Images.backgroundImage.sprite = Images.hurtBG;
        yield return waitSec;
        Images.backgroundImage.sprite = Images.basicBG;
    }

    /// <summary>
    /// 특정 시간이 지나면 패배 팝업을 띄웁니다. 
    /// </summary>
    private IEnumerator ProcessOpenLosePopup()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.OpenLosePopup();

    }
}

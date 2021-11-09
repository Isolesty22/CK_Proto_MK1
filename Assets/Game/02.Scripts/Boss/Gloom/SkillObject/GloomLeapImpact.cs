using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomLeapImpact : MonoBehaviour
{

    #region Class
    [System.Serializable]
    public class Values
    {
        [HideInInspector]
        public WaitForSeconds waitDuration = null;

        [HideInInspector]
        public float duration;
    }

    //[System.Serializable]
    //public class Components
    //{
    //}

    //[System.Serializable]
    //public class Effects
    //{
    //    public GameObject[] effects;
    //}

    #endregion

    public GameObject leapImpactFlame;
    public VfxActiveHelper[] vfxHelpers;

    public GloomController gloom;
    [SerializeField]
    private Values _values = new Values();

    //[SerializeField]
    //private Components _components = new Components();

    //[SerializeField]
    //private Effects _effects = new Effects();

    public Values Val => _values;
    //public Components Com => _components;
    //public Effects Effect => _effects;

    public Collider myCollider;

    private int effectsCount;

    private void Awake()
    {

    }
    private void Start()
    {
        player = GameManager.instance.playerController;

        Val.duration = gloom.SkillVal.leap.leapImpactDuration;

        Val.waitDuration = new WaitForSeconds(Val.duration);

        myCollider.enabled = false;

        effectsCount = vfxHelpers.Length;

        for (int i = 0; i < effectsCount; i++)
        {
            vfxHelpers[i].gameObject.SetActive(false);
        }
    }

    public void StartImpact()
    {
        for (int i = 0; i < effectsCount; i++)
        {
            vfxHelpers[i].SetActiveTime(Val.waitDuration);
        }
        StartCoroutine(ProcessImpact());
    }

    private WaitForSeconds createInterval = new WaitForSeconds(0.2f);
    private IEnumerator ProcessImpact()
    {



        //임팩트 불꽃 켜기
        leapImpactFlame.SetActive(true);

        yield return createInterval;

        for (int i = 0; i < effectsCount; i++)
        {
            vfxHelpers[i].Active();
            yield return createInterval;
        }

        myCollider.enabled = true;

        yield return Val.waitDuration;


        leapImpactFlame.SetActive(false);
        myCollider.enabled = false;


    }

    private PlayerController player;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            if (!player.IsInvincible())
            {
                player.Hit();
            }
        }
    }
}

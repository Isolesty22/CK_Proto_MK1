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

    public GameObject[] effects;
    public Collider myCollider;

    private int effectsCount;

    private void Awake()
    {
        myCollider.enabled = false;
        effectsCount = effects.Length;
        for (int i = 0; i < effectsCount; i++)
        {
            effects[i].SetActive(false);
        }
    }
    private void Start()
    {
        player = GameManager.instance.playerController;
        Val.duration = gloom.SkillVal.leap.leapImpactDuration;
        Val.waitDuration = new WaitForSeconds(Val.duration);

        gameObject.SetActive(false);
    }

    //public void Init()
    //{

    //}

    public void StartImpact()
    {
        gameObject.SetActive(true);
        StartCoroutine(ProcessImpact());
    }
    private IEnumerator ProcessImpact()
    {

        for (int i = 0; i < effectsCount; i++)
        {
            effects[i].SetActive(true);
        }
        myCollider.enabled = true;

        yield return Val.waitDuration;

        for (int i = 0; i < effectsCount; i++)
        {
            effects[i].SetActive(false);
        }

        myCollider.enabled = false;
        gameObject.SetActive(false);
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

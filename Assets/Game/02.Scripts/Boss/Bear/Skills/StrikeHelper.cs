using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeHelper : MonoBehaviour
{

    public BoxCollider box;


    private WaitForSeconds waitColliderOn = new WaitForSeconds(3.5f);
    private WaitForSeconds waitColliderOff = new WaitForSeconds(0.3f);
    private WaitForSeconds waitDestroy = new WaitForSeconds(1f);

    public AudioClip clip;

    private void Awake()
    {
        box.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(CoProcessStrike());
    }

    private IEnumerator CoProcessStrike()
    {
        yield return waitColliderOn;
        AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(clip);
        box.enabled = true;
        yield return waitColliderOff;
        box.enabled = false;
        yield return waitDestroy;
        Destroy(gameObject);
    }

}

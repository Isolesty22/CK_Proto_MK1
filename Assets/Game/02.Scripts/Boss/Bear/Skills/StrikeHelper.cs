using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeHelper : MonoBehaviour
{

    public BoxCollider box;


    private WaitForSeconds waitColliderOn = new WaitForSeconds(3.5f);
    private WaitForSeconds waitColliderOff = new WaitForSeconds(0.3f);
    private WaitForSeconds waitDestroy = new WaitForSeconds(1f);

    private AudioSource audioSource;
    private void Awake()
    {

        box.enabled = false;
    }
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 1 * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
        StartCoroutine(CoProcessStrike());
    }

    private IEnumerator CoProcessStrike()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 1 * AudioManager.Instance.currentMasterVolume * AudioManager.Instance.currentSFXVolume;
        yield return waitColliderOn;
        audioSource.Play();
        box.enabled = true;
        yield return waitColliderOff;
        box.enabled = false;
        yield return waitDestroy;
        audioSource.Stop();
        Destroy(gameObject);
    }

}

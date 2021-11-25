using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWall : MonoBehaviour
{

    public ParticleSystem particle;
    public Transform tr;

    private bool isOn = false;

    private IEnumerator coEffect = null;

    private void Awake()
    {
        isOn = false;
        coEffect = CoEffect();
    }
    private void StartParticle()
    {
        if (isOn)
        {
            particle.Stop();
            StopCoroutine(coEffect);
        }
        coEffect = CoEffect();
        StartCoroutine(coEffect);
    }

    WaitForSeconds waitSec = new WaitForSeconds(2f);
    private IEnumerator CoEffect()
    {
        isOn = true;
        particle.gameObject.SetActive(true);
        particle.Play();
        yield return waitSec;

        particle.Stop();

        yield return waitSec;
        particle.gameObject.SetActive(false);
        isOn = false;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(TagName.Player))
        {
            tr.position = collision.GetContact(0).point;
            StartParticle();
        }
    }
}

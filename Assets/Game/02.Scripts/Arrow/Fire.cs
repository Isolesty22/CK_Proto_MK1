using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public void Play()
    {
        var play = CheckTime();
        StartCoroutine(play);
    }

    public IEnumerator CheckTime()
    {
        this.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);

        CustomPoolManager.Instance.ReleaseThis(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParry : MonoBehaviour
{
    float smoothSpeed = 4f;


    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.instance.playerController.Com.pixy.transform.position, Time.deltaTime * smoothSpeed);
    }

    public void Play()
    {
        var play = CheckTime();
        StartCoroutine(play);
    }

    public IEnumerator CheckTime()
    {
        this.GetComponent<ParticleSystem>().Play();

        //var parry = GetParrying();
        //StartCoroutine(parry);

        yield return new WaitForSeconds(2f);

        CustomPoolManager.Instance.ReleaseThis(this);
    }

    public IEnumerator GetParrying()
    {
        yield return new WaitForSeconds(0.2f);

        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, GameManager.instance.playerController.Com.pixy.transform.position, Time.deltaTime * smoothSpeed);

            yield return new WaitForFixedUpdate();
        }
    }
}

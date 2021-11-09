using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GetParry : MonoBehaviour
{
    public float smoothSpeed = 3f;


    private void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, GameManager.instance.playerController.Com.pixy.transform.position, Time.deltaTime * smoothSpeed);
    }

    public void Play()
    {
        var play = CheckTime();
        StartCoroutine(play);
    }

    public IEnumerator CheckTime()
    {
        this.GetComponent<ParticleSystem>().Play();

        var parry = GetParrying();
        StartCoroutine(parry);

        yield return new WaitForSeconds(5f);

        CustomPoolManager.Instance.ReleaseThis(this);
    }

    public IEnumerator GetParrying()
    {
        yield return new WaitForSeconds(0.7f);

        while (true)
        {
            //transform.DOMove(GameManager.instance.playerController.Com.pixy.transform.position, 1f);

            transform.position = Vector3.Lerp(transform.position, GameManager.instance.playerController.Com.pixy.transform.position, Time.fixedDeltaTime * smoothSpeed);

            yield return new WaitForFixedUpdate();
        }
    }
}

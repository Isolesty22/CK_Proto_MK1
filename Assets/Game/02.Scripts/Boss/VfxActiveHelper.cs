using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxActiveHelper : MonoBehaviour
{
    public ParticleSystem particle;
    private WaitForSeconds activeTime = new WaitForSeconds(3f);
    private WaitForSeconds waitTime = new WaitForSeconds(2f);


    /// <summary>
    /// activeTime��ŭ ���ӵ� ��, waitTime �Ŀ� ���ӿ�����Ʈ�� ��Ȱ��ȭ�˴ϴ�.
    /// </summary>
    public void Active()
    {
        gameObject.SetActive(true);
        StartCoroutine(ActiveEffect());
    }
    public void SetActive(bool _b)
    {
        gameObject.SetActive(_b);
    }
    public void SetActiveTime(float _activeTime)
    {
        activeTime = new WaitForSeconds(_activeTime);
    }
    public void SetActiveTime(WaitForSeconds _activeTime)
    {
        activeTime = _activeTime;
    }
    
    public void StartVFX()
    {
        gameObject.SetActive(true);
        particle.Play();
    }

    public void StopVFX()
    {
        particle.Stop();
        StartCoroutine(DisableGameObject());
    }


    private IEnumerator DisableGameObject()
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }
    private IEnumerator ActiveEffect()
    {
        particle.Play();
        yield return activeTime;
        particle.Stop();
        yield return waitTime;
        gameObject.SetActive(false);
    }
}

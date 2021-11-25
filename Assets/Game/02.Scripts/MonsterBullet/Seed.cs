using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float fireSpeed;
    public float range;
    public Vector3 fireDir;
    public Vector3 firePos;

    public GameObject seedModel;
    public ParticleSystem bomb;

    bool isStop;
    public void Initialize()
    {
        isStop = false;
        seedModel.SetActive(true);
        gameObject.GetComponent<SphereCollider>().enabled = true;
        bomb.gameObject.SetActive(false);
        bomb.Stop();
    }

    private void FixedUpdate()
    {
        if((firePos - transform.position).magnitude < range)
        {
            if(!isStop)
                transform.Translate(fireDir * fireSpeed * Time.fixedDeltaTime);
        }
        else
        {
            var despawn = Despawn();
            StartCoroutine(despawn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var despawn = Despawn();
            StartCoroutine(despawn);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            var despawn = Despawn();
            StartCoroutine(despawn);
        }
    }

    public IEnumerator Despawn()
    {
        isStop = true;

        seedModel.SetActive(false);

        bomb.gameObject.SetActive(true);
        if(!bomb.isPlaying)
            bomb.Play();

        yield return null;
        gameObject.GetComponent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(1f) ;

        CustomPoolManager.Instance.ReleaseThis(this);
    }
}

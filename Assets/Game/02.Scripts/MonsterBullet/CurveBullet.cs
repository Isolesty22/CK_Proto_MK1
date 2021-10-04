using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : MonoBehaviour
{
    public Vector3 startPos; 
    public Vector3 endPos; 

    public Vector3 startHeightPos;
    public Vector3 endHeightPos;

    public float duration;

    public float height = 3.5f;

    public bool isRun;

    float t; 

    float startTime;


    void Start()
    {
    }

    public void Initialize()
    {
        startTime = Time.time;
        startHeightPos = startPos + new Vector3(0, 1, 0) * height;
        endHeightPos = endPos + new Vector3(0, 1, 0) * height;
    }
    void Update()
    {
        if (isRun)
        {
            t = (Time.time - startTime) / duration;

            BezierCurve();
        }

        if(Vector3.Distance(transform.position, endPos) < 0.01f)
        {
            isRun = false;
            transform.GetComponent<Rigidbody>().useGravity = true;
        }
    }


    void BezierCurve()
    {
        Vector3 a = Vector3.Lerp(startPos, startHeightPos, t); 
        Vector3 b = Vector3.Lerp(startHeightPos, endHeightPos, t);
        Vector3 c = Vector3.Lerp(endHeightPos, endPos, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 f = Vector3.Lerp(d, e, t);

        transform.position = f;
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
            CustomPoolManager.Instance.ReleaseThis(this);
        }
    }
    public IEnumerator Despawn()
    {
        yield return null;

        CustomPoolManager.Instance.ReleaseThis(this);
    }
}

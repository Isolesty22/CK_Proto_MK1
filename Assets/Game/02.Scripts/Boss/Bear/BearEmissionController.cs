using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEmissionController : MonoBehaviour
{

    public SkinnedMeshRenderer meshRenderer;
    Material material;

    Color baseColor = Color.red;
    Color originalColor;
    Color hitColor = new Color(0.8f,0.8f,0.8f,1f);
    private void Awake()
    {
        material = meshRenderer.material;
        originalColor = material.GetColor("_BaseColor");
    }


    //private void Start()
    //{
    //    material.SetColor("_EmissionColor", Color.red * 255f);
    //}
    //private void Update()
    //{
    //    //float emission = Mathf.PingPong(Time.time * 0.5f, 1f);
    //    float intensity = Mathf.PingPong(Time.time * 500f, 255f);
    //    Color finalColor = baseColor;// * Mathf.LinearToGammaSpace(emission);
    //    //material.SetColor("_EmissionColor", finalColor * (1f + intensity));
    //    material.SetColor("_EmissionColor", finalColor * 10f);
    //}

    public void EmissionOn(float _value)
    {
        material.SetColor("_EmissionColor", baseColor * _value);
    }
    public void EmissionOff()
    {
        material.SetColor("_EmissionColor", baseColor * 0f);
    }

    private IEnumerator hitColorCoroutine = null;
    public void OnHit()
    {
        if (hitColorCoroutine == null)
        {
            hitColorCoroutine = HitColor();
            StartCoroutine(hitColorCoroutine);
        }
    }

    WaitForSeconds waitTime = new WaitForSeconds(0.1f);
    
    public IEnumerator HitColor()
    {
        material.SetColor("_BaseColor", hitColor);
        yield return waitTime;
        material.SetColor("_BaseColor", originalColor);

        hitColorCoroutine = null;
    }
}

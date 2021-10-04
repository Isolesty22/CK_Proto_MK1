using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEmissionController : MonoBehaviour
{

    public SkinnedMeshRenderer meshRenderer;
    Material material;

    private void Awake()
    {
        material = meshRenderer.sharedMaterial;

    }
    Color baseColor = Color.red;


    //private void Start()
    //{
    //    material.SetColor("_EmissionColor", Color.red * 255f);
    //}
    private void Update()
    {
        //float emission = Mathf.PingPong(Time.time * 0.5f, 1f);
        float intensity = Mathf.PingPong(Time.time * 500f, 255f);
        Color finalColor = baseColor;// * Mathf.LinearToGammaSpace(emission);
        material.SetColor("_EmissionColor", finalColor * (1f + intensity));
    }

}

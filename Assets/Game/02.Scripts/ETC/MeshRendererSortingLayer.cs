using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSortingLayer : MonoBehaviour
{

    [SerializeField]
    public string sortingLayerName;

    [SerializeField]
    public int sortingOrder;

    private MeshRenderer meshRenderer = null;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;

    }

    private void OnValidate()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();

        }
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;
    }

}

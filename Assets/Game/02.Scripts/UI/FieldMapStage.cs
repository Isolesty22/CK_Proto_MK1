using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMapStage : MonoBehaviour
{
    [Header("이동할 위치")]
    public Transform stageTransform;
    [Header("스프라이트 렌더러")]
    public SpriteRenderer spriteRenderer;
    public void SetStageGrayScale(float _value)
    {
        spriteRenderer.material.SetFloat("_Greyscale", _value);
        Debug.LogWarning(spriteRenderer.material.GetFloat("_Greyscale"));
    }

}

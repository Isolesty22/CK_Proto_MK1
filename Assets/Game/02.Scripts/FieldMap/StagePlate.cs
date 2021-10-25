using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지를 나타내는 발판입니다.
/// </summary>
public class StagePlate : MonoBehaviour
{
    [Header("이동할 위치")]
    public Transform stageTransform;

    [Header("스프라이트 렌더러")]
    public SpriteRenderer spriteRenderer;
    /// <summary>
    /// 스테이지를 못가게 막습
    /// </summary>
    /// <param name="_value"></param>
    public void SetStageGrayScale(float _value)
    {
        spriteRenderer.material.SetFloat("_Greyscale", _value);
    }
}

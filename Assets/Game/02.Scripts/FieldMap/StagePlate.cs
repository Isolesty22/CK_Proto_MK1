using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지의 위치 정보등을 가지고 있는 발판입니다.
/// </summary>
[SelectionBase]
public class StagePlate : MonoBehaviour
{

    [Tooltip("[임시] 시연용. 스테이지 이름으로 이동합니다.")]
    public string stageName;

    public Transform myTransform;

    [Header("스프라이트 렌더러")]
    public SpriteRenderer spriteRenderer;

    public void SetStageGrayScale(float _value)
    {
        spriteRenderer.material.SetFloat("_Greyscale", _value);
    }

    public Vector3 GetPosition()
    {
        return myTransform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� ��ġ �������� ������ �ִ� �����Դϴ�.
/// </summary>
public class StagePlate : MonoBehaviour
{
    public Transform myTransform;

    [Header("��������Ʈ ������")]
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

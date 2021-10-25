using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� ��Ÿ���� �����Դϴ�.
/// </summary>
public class StagePlate : MonoBehaviour
{
    [Header("�̵��� ��ġ")]
    public Transform stageTransform;

    [Header("��������Ʈ ������")]
    public SpriteRenderer spriteRenderer;
    /// <summary>
    /// ���������� ������ ����
    /// </summary>
    /// <param name="_value"></param>
    public void SetStageGrayScale(float _value)
    {
        spriteRenderer.material.SetFloat("_Greyscale", _value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���Ž� ��ų�� ���Ǵ� ���� �����մϴ�.
/// </summary>
public class SmashHelper : MonoBehaviour
{

    public Transform myTransform;
    

    public SmashRock[] smashRocks;

    [Space(5)]
    public Transform bearHandTransform;
    public Transform pivotTransform;

    public int rockCount { get; private set; }
    private void Awake()
    {
        rockCount = smashRocks.Length;
        for (int i = 0; i < rockCount; i++)
        {
            smashRocks[i].UpdatePivot(pivotTransform.position);
        }
    }
    /// <summary>
    /// �̵��� �ʿ��� ��ġ���� �����ŵ�ϴ�.
    /// </summary>
    /// <param name="_index">�迭 ���� �ε���</param>
    public void UpdateVectors(int _index, Vector3 _start, Vector3 _mid, Vector3 _end)
    {
        smashRocks[_index].UpdateVectors(_start, _mid, _end);
    }
    public Vector3 GetPosition(int _index) => smashRocks[_index].rb.position;

    public void SetParentRocks(Transform _tr)
    {
        for (int i = 0; i < rockCount; i++)
        {
            smashRocks[i].tr.parent = _tr;
        }
    }

    public void StartMove(int _index) => smashRocks[_index].StartMove();
    public void SetActive(bool _b)
    {
        gameObject.SetActive(_b);
    }

    public void SetActiveRocks(bool _b)
    {
        for (int i = 0; i < rockCount; i++)
        {
            smashRocks[i].SetActive(_b);
        }
    }
}

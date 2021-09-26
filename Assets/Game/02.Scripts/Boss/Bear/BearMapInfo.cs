using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(BoxCollider)))]
public class BearMapInfo : MonoBehaviour
{
    [Tooltip("맵 상의 블록 개수")]
    private const int blockCount = 5;

    public BoxCollider mapCollider;
    public Transform myTransform;

    [ReadOnly]
    public Vector3 mapSize;

    [ReadOnly]
    public Vector3 mapPosition;

    [BeginReadOnlyGroup]
    public BearBlock[] bearBlocks = new BearBlock[blockCount];


    public void InitBearBlocks()
    {
        //mapSize, mapPosition 계산
        UpdateMapVector();

        Vector3 tempMin;
        Vector3 tempMax;
        for (int i = 0; i < blockCount; i++)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        UpdateMapVector();
        Gizmos.DrawWireCube(mapPosition, mapSize);
    }

    private void UpdateMapVector()
    {
        mapSize = new Vector3(mapCollider.size.x * myTransform.lossyScale.x,
            mapCollider.size.y * myTransform.lossyScale.y,
            mapCollider.size.z * myTransform.lossyScale.z);
        
        mapPosition = myTransform.TransformPoint(mapCollider.center);

    }

    private void CalcBearBlocks()
    {

    }
}

[System.Serializable]
public class BearBlock
{
    [System.Serializable]
    public class Position
    {
        public Position() { }

        public Position(Vector3 _min, Vector3 _max)
        {
            min = _min;
            max = _max;
        }

        public Vector3 min;
        public Vector3 max;
    }

    public Position position = new Position();

    public void SetPosition(Vector3 _min, Vector3 _max)
    {
        position.min = _min;
        position.max = _max;
    }
}



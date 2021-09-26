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

    [Tooltip("사이즈의 절반값입니다.")]
    private Vector3 mapExtents;

    [BeginReadOnlyGroup]
    public BearBlock[] bearBlocks = new BearBlock[blockCount];


    public void Init()
    {
        //mapSize, mapPosition 계산
        UpdateMapVector();


    }
    private void UpdateMapVector()
    {
        mapSize = new Vector3(mapCollider.size.x * myTransform.lossyScale.x,
            mapCollider.size.y * myTransform.lossyScale.y,
            mapCollider.size.z * myTransform.lossyScale.z);

        mapPosition = myTransform.TransformPoint(mapCollider.center);

        mapExtents = new Vector3(mapSize.x * 0.5f, mapSize.y * 0.5f, mapSize.z * 0.5f);
    }

    private void UpdateBearBlocks()
    {

        //5개로 나누기
        float oneBlockPos = mapPosition.x / blockCount;
        //float oneBlockPos = mapPosition.x * 0.2f;

        Vector3 tempMin = new Vector3(mapPosition.x - mapExtents.x,
            mapPosition.y - mapExtents.y,
            mapPosition.z - mapExtents.z);

        Vector3 tempMax = new Vector3(mapPosition.x + mapExtents.x,
            mapPosition.y + mapExtents.y,
            mapPosition.z + mapExtents.z);

        Vector3 tempDistance = new Vector3(Mathf.Abs(tempMax.x - tempMin.x),
            Mathf.Abs(tempMax.y - tempMin.y), Mathf.Abs(tempMax.z - tempMin.z));

        tempDistance = tempDistance / blockCount;

        bearBlocks[0].SetPosition(tempMin, tempMax);

        for (int i = 1; i < blockCount; i++)
        {

        }
    }

    private void OnDrawGizmos()
    {
        UpdateMapVector();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mapPosition, mapSize);

        Vector3 tempMax = new Vector3(mapPosition.x + mapExtents.x,
             mapPosition.y + mapExtents.y,
             mapPosition.z + mapExtents.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(tempMax, 0.5f);
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



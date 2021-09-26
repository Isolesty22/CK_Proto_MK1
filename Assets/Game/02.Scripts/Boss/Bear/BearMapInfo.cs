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

    [System.Serializable]
    public class MapData
    {
        [ReadOnly]
        public Vector3 size;

        [ReadOnly]
        public Vector3 position;

        [ReadOnly, Tooltip("사이즈의 절반값")] 
        public Vector3 extents;
    }

    [BeginReadOnlyGroup]
    public MapData mapData = new MapData();

    [BeginReadOnlyGroup]
    public BearBlock[] bearBlocks = new BearBlock[blockCount];


    public void Init()
    {
        //mapSize, mapPosition 계산
        UpdateMapVector();
        UpdateBearBlocks();
    }
    private void UpdateMapVector()
    {
        mapData.size = new Vector3(mapCollider.size.x * myTransform.lossyScale.x,
            mapCollider.size.y * myTransform.lossyScale.y,
            mapCollider.size.z * myTransform.lossyScale.z);

        mapData.position = myTransform.TransformPoint(mapCollider.center);

        mapData.extents = new Vector3(mapData.size.x * 0.5f, mapData.size.y * 0.5f, mapData.size.z * 0.5f);
    }

    private void UpdateBearBlocks()
    {

        Vector3 mapMin = new Vector3(mapData.position.x - mapData.extents.x, mapData.position.y - mapData.extents.y, mapData.position.z - mapData.extents.z);
        Vector3 mapMax = new Vector3(mapData.position.x + mapData.extents.x, mapData.position.y + mapData.extents.y, mapData.position.z + mapData.extents.z);

        Vector3 tempMin = mapMin;
        Vector3 tempMax = mapMax;

        Vector3 tempDistance = new Vector3(Mathf.Abs(tempMax.x - tempMin.x), Mathf.Abs(tempMax.y - tempMin.y), Mathf.Abs(tempMax.z - tempMin.z));
        tempDistance = tempDistance / blockCount;


        //첫번째 블록 포지션 계산
        tempMin = new Vector3(tempMin.x, mapMin.y, mapMin.z);
        tempMax = new Vector3(tempMin.x + tempDistance.x, mapMax.y, mapMax.z);

        bearBlocks[0].SetMinMaxPosition(tempMin, tempMax);

        //나머지 블록 포지션 계산
        for (int i = 1; i < blockCount; i++)
        {
            tempMin = new Vector3(bearBlocks[i - 1].position.max.x, mapMin.y, mapMin.z);
            tempMax = new Vector3(mapMin.x + (tempDistance.x * (i + 1)), mapMax.y, mapMax.z);

            bearBlocks[i].SetMinMaxPosition(tempMin, tempMax);
        }

    }
    private void OnDrawGizmos()
    {
        UpdateMapVector();
        UpdateBearBlocks();



        Gizmos.color = Color.red;
        for (int i = 0; i < blockCount; i++)
        {
            Gizmos.DrawLine(bearBlocks[i].position.min, bearBlocks[i].position.max);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(mapData.position, mapData.size);

        Gizmos.color = Color.yellow;
        for (int i = 0; i < blockCount; i++)
        {
            Gizmos.DrawSphere(bearBlocks[i].position.min, 0.1f);

        }

        Gizmos.color = Color.green;
        for (int i = 0; i < blockCount; i++)
        {
            Gizmos.DrawSphere(bearBlocks[i].position.max, 0.1f);

        }


    }
}

[System.Serializable]
public class BearBlock
{
    [System.Serializable]
    public class Position
    {
        public Vector3 min;
        public Vector3 max;

        public Vector3 groundCenter;
    }

    public Position position = new Position();

    public void SetMinMaxPosition(Vector3 _min, Vector3 _max)
    {
        position.min = _min;
        position.max = _max;
    }

    public void SetGroundCenterPosition()
    {

    }
}



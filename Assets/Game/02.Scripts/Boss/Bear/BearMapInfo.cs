using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(BoxCollider)))]
public class BearMapInfo : MonoBehaviour
{
    #region MapDataClass
    [System.Serializable]
    public class MapData
    {
        [ReadOnly]
        public Vector3 size;

        [ReadOnly]
        public Vector3 position;

        [ReadOnly]
        public Vector3 minPosition;

        [ReadOnly]
        public Vector3 maxPosition;

        [ReadOnly, Tooltip("사이즈의 절반값")]
        public Vector3 extents;

        [ReadOnly, Tooltip("블록 하나당 길이")]
        public Vector3 blockLength;
    }
    #endregion

    [Range(0, 30)]
    public int projectilePosCount = 10;

    [HideInInspector]
    public int[] projectileRandArray = new int[] { };

    [Space(20)]

    [Tooltip("맵 상의 블록 개수")]
    private const int blockCount = 5;

    public BoxCollider mapCollider;
    public Transform myTransform;

    [Space(10)]
    public Transform bearTransform;
    public Vector3 phase2Position;
    public Vector3 phase3Position;

    [Space(10)]
    [HideInInspector]
    public int paddingSize = 4;
    [HideInInspector]
    public int rightPadding = 0;
    [HideInInspector]
    public int leftPadding = 4;


    [Space(10)]
    [BeginReadOnlyGroup]
    public MapData mapData = new MapData();

    [BeginReadOnlyGroup]
    public MapBlock[] mapBlocks = new MapBlock[blockCount];
    [EndReadOnlyGroup]

    [HideInInspector]
    public Vector3[] projectilePositions;

    public void Init()
    {
       // Physics.IgnoreLayerCollision(Physics.AllLayers, gameObject.layer);

        //mapSize, mapPosition 계산
        UpdateMapVector();
        UpdatemapBlocks();
        Init_Projectiles();
        Init_PhasePositions();
    }
    public void Init_Projectiles()
    {
        UpdateProjectilePositions();
        InitProjectileRandArray();
        UpdateProjectileRandArray();
    }

    public void Init_PhasePositions()
    {
        phase2Position = new Vector3(mapBlocks[0].positions.groundCenter.x, 0.38f, bearTransform.position.z);
        phase3Position = bearTransform.position;

    }
    private void UpdateMapVector()
    {
        mapData.size = new Vector3(mapCollider.size.x * myTransform.lossyScale.x,
            mapCollider.size.y * myTransform.lossyScale.y,
            mapCollider.size.z * myTransform.lossyScale.z);

        mapData.position = myTransform.TransformPoint(mapCollider.center);

        mapData.extents = new Vector3(mapData.size.x * 0.5f, mapData.size.y * 0.5f, mapData.size.z * 0.5f);

        mapData.minPosition = new Vector3(mapData.position.x - mapData.extents.x, mapData.position.y - mapData.extents.y, mapData.position.z - mapData.extents.z);
        mapData.maxPosition = new Vector3(mapData.position.x + mapData.extents.x, mapData.position.y + mapData.extents.y, mapData.position.z + mapData.extents.z);
    }

    private void UpdatemapBlocks()
    {
        Vector3 tempMin = mapData.minPosition;
        Vector3 tempMax = mapData.maxPosition;

        mapData.blockLength = new Vector3(Mathf.Abs(tempMax.x - tempMin.x), Mathf.Abs(tempMax.y - tempMin.y), Mathf.Abs(tempMax.z - tempMin.z));
        mapData.blockLength = mapData.blockLength / blockCount;


        //첫번째 블록 포지션 계산
        tempMin = new Vector3(tempMin.x, mapData.minPosition.y, mapData.minPosition.z);
        tempMax = new Vector3(tempMin.x + mapData.blockLength.x, mapData.maxPosition.y, mapData.maxPosition.z);

        mapBlocks[0].SetMinMax(tempMin, tempMax);
        mapBlocks[0].SetGroundCenter(CalcGroundCenter(mapBlocks[0].positions));
        mapBlocks[0].SetTopCenter(CalcTopCenter(mapBlocks[0].positions));

        //나머지 블록 포지션 계산
        for (int i = 1; i < blockCount; i++)
        {
            tempMin = new Vector3(mapBlocks[i - 1].positions.max.x, mapData.minPosition.y, mapData.minPosition.z);
            tempMax = new Vector3(mapData.minPosition.x + (mapData.blockLength.x * (i + 1)), mapData.maxPosition.y, mapData.maxPosition.z);

            mapBlocks[i].SetMinMax(tempMin, tempMax);
            mapBlocks[i].SetGroundCenter(CalcGroundCenter(mapBlocks[i].positions));
            mapBlocks[i].SetTopCenter(CalcTopCenter(mapBlocks[i].positions));
        }

    }

    private Vector3 CalcGroundCenter(MapBlock.Positions _bearBlockPosition)
    {
        Vector3 bottomCenter = new Vector3(_bearBlockPosition.min.x + mapData.blockLength.x * 0.5f, _bearBlockPosition.min.y, mapData.position.z);
        return bottomCenter;
    }

    private Vector3 CalcTopCenter(MapBlock.Positions _bearBlockPosition)
    {
        Vector3 bottomCenter = new Vector3(_bearBlockPosition.groundCenter.x, _bearBlockPosition.max.y, mapData.position.z);
        return bottomCenter;
    }
    public void UpdateProjectilePositions()
    {
        projectilePositions = new Vector3[projectilePosCount - paddingSize];

        Vector3 tempMin = mapData.minPosition;
        Vector3 tempMax = mapData.maxPosition;

        float distanceX = (Mathf.Abs(tempMax.x - tempMin.x)) / projectilePosCount;

        //첫번째 가운데 지점 계산
        tempMin = new Vector3((tempMin.x + distanceX * 0.5f) + distanceX * rightPadding, mapData.maxPosition.y, mapData.position.z);


        int length = projectilePositions.Length;

        //첫번째 가운데 지점 설정
        projectilePositions[0] = tempMin;

        for (int i = 1; i < length; i++)
        {
            projectilePositions[i] = new Vector3(projectilePositions[i - 1].x + distanceX, mapData.maxPosition.y, mapData.position.z);
        }

    }

    public void InitProjectileRandArray()
    {
        //배열 초기화--
        projectileRandArray = new int[projectilePosCount - paddingSize];

        int length = projectileRandArray.Length;

        for (int i = 0; i < length; i++)
        {
            projectileRandArray[i] = i;
        }


        //랜덤---
        for (int i = 0; i < length; i++)
        {
            int randIndex = Random.Range(i, length);

            int tempPos = projectileRandArray[i];
            projectileRandArray[i] = projectileRandArray[randIndex];
            projectileRandArray[randIndex] = tempPos;
        }
    }

    public void UpdateProjectileRandArray()
    {
        int length = projectileRandArray.Length;

        //랜덤---
        for (int i = 0; i < length; i++)
        {
            int randIndex = Random.Range(i, length);

            int tempPos = projectileRandArray[i];
            projectileRandArray[i] = projectileRandArray[randIndex];
            projectileRandArray[randIndex] = tempPos;
        }
    }


    private void OnDrawGizmos()
    {
        UpdateMapVector();
        UpdatemapBlocks();
        UpdateProjectilePositions();
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < blockCount; i++)
            {
                Gizmos.DrawLine(mapBlocks[i].positions.min, mapBlocks[i].positions.max);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(mapData.position, mapData.size);

            Gizmos.color = Color.grey;
            for (int i = 0; i < blockCount; i++)
            {
                Gizmos.DrawSphere(mapBlocks[i].positions.groundCenter, 0.1f);
            }

            Gizmos.color = Color.white;
            for (int i = 0; i < blockCount; i++)
            {
                Gizmos.DrawLine(mapBlocks[i].positions.groundCenter, mapBlocks[i].positions.topCenter);
            }
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < projectilePositions.Length; i++)
        {
            Gizmos.DrawSphere(projectilePositions[i], 0.2f);

        }

    }
}

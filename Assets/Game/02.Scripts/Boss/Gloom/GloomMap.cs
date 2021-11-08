using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(BoxCollider)))]
public class GloomMap : MonoBehaviour
{
    #region Class
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

    public class Length
    {
        public int min;
        public int max;
    }
    #endregion


    [Space(20)]

    [Tooltip("맵 상의 블록 개수")]
    private const int blockCount = 7;

    public BoxCollider mapCollider;
    public Transform myTransform;

    public Transform gloomPos_Left;
    public Transform gloomPos_Right;

    [Tooltip("맵의 방향")]
    [HideInInspector]
    public eDiretion mapDirection;

    [Header("낭떠러지 관련")]
    [Tooltip("낭떠러지로 지정할 블록의 인덱스")]
    public int emptyIndex;
    [Tooltip("낭떠러지로 지정할 블록의 땅 판정 위치")]
    public float emptyGroundPosY;

    [Header("양 옆 제외할 블록 개수")]
    [Tooltip("보스의 위치에 따라서 양 옆에 있는 블록을 사용하지 않는데, " +
        "그 때 몇 개 만큼 제외할지의 개수입니다. " +
        "기즈모로 표현되지는 않습니다.")]
    public int exclusiveCount = 1;


    [Header("투사체 등 위치 개수")]
    [Range(0, 30)]
    public int projectilePosCount = 10;

    [HideInInspector]
    public int[] projectileRandArray = new int[] { };

    [Space(10)]
    public MapData mapData = new MapData();
    public Length mapLength = new Length();

    public MapBlock[] mapBlocks = new MapBlock[blockCount];

    [HideInInspector]
    public Vector3[] projectilePositions;

    public void Init()
    {
        //오른쪽 방향으로
        ChangeDirection(eDiretion.Right);

        //mapSize, mapPosition 계산
        UpdateMapVector();
        UpdateMapBlocks();
        Init_Projectiles();
        Init_MapBlocksType();

    }
    public void Init_Projectiles()
    {
        UpdateProjectilePositions();
        InitProjectileRandArray();
        UpdateProjectileRandArray();
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

        Vector3 tempMin = mapData.minPosition;
        Vector3 tempMax = mapData.maxPosition;

        mapData.blockLength = new Vector3(Mathf.Abs(tempMax.x - tempMin.x), Mathf.Abs(tempMax.y - tempMin.y), Mathf.Abs(tempMax.z - tempMin.z));
        mapData.blockLength = mapData.blockLength / blockCount;
    }

    private void UpdateMapBlocks()
    {
        Vector3 tempMin = mapData.minPosition;
        Vector3 tempMax = mapData.maxPosition;

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
        Vector3 originPos = mapBlocks[emptyIndex].positions.groundCenter;
        mapBlocks[emptyIndex].SetGroundCenter(new Vector3(originPos.x, originPos.y+emptyGroundPosY, originPos.z));
    }


    private void Init_MapBlocksType()
    {
        for (int i = 0; i < blockCount; i ++)
        {

            mapBlocks[i].SetOriginType(MapBlock.eType.None);
            mapBlocks[i].SetCurrentTypeToOrigin();
        }

        if (emptyIndex < blockCount && emptyIndex >= 0)
        {
            mapBlocks[emptyIndex].SetOriginType(MapBlock.eType.Empty);
            mapBlocks[emptyIndex].SetCurrentTypeToOrigin();
        }
        else
        {
            Debug.LogWarning("낭떠러지는 블록의 범위 내여야 합니다. 설정하지 못했습니다.");
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
        projectilePositions = new Vector3[projectilePosCount];

        Vector3 tempMin = mapData.minPosition;
        Vector3 tempMax = mapData.maxPosition;

        float distanceX = (Mathf.Abs(tempMax.x - tempMin.x)) / projectilePosCount;

        //첫번째 가운데 지점 계산
        tempMin = new Vector3((tempMin.x + distanceX * 0.5f), mapData.maxPosition.y, mapData.position.z);


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
        projectileRandArray = new int[projectilePosCount];

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


    /// <summary>
    /// 맵의 방향을 바꿉니다. 보스의 현재 위치를 받습니다.
    /// </summary>
    /// <param name="_dir">보스의 현재 위치</param>
    public void ChangeDirection(eDiretion _dir)
    {
        switch (_dir)
        {
            case eDiretion.Left:
                mapLength.min = exclusiveCount;
                mapLength.max = blockCount;
                break;

            case eDiretion.Right:
                mapLength.min = 0;
                mapLength.max = blockCount - exclusiveCount;

                break;
        }

    }

    private void OnDrawGizmos()
    {
        UpdateMapVector();
        UpdateMapBlocks();
        UpdateProjectilePositions();
        {

            for (int i = 0; i < blockCount; i++)
            {
                if (mapBlocks[i].currentType == MapBlock.eType.Empty)
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawLine(mapBlocks[i].positions.min, mapBlocks[i].positions.max);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(mapData.position, mapData.size);

            Gizmos.color = Color.grey;
            for (int i = 0; i < blockCount; i++)
            {
                Gizmos.DrawSphere(mapBlocks[i].positions.groundCenter, 0.1f);
            }

            for (int i = 0; i < blockCount; i++)
            {
                //만약 빈공간이면
                if (mapBlocks[i].currentType == MapBlock.eType.Empty)
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawLine(mapBlocks[i].positions.groundCenter, mapBlocks[i].positions.topCenter);
            }
        }



        for (int i = 0; i < projectilePositions.Length; i++)
        {             

                Gizmos.color = Color.green;


            Gizmos.DrawSphere(projectilePositions[i], 0.2f);

        }

    }
}



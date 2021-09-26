using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(BoxCollider)))]
public class BearMapInfo : MonoBehaviour
{
    public BoxCollider mapCollider;

    [ReadOnly]
    public Vector3 mapSize;

    [ReadOnly]
    public Vector3 mapPosition;

    [BeginReadOnlyGroup]
    public BearBlock[] bearBlocks = new BearBlock[5];

    public void InitBearBlocks()
    {
        CalcMapVector();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        CalcMapVector();
        Gizmos.DrawWireCube(mapPosition, mapSize);
    }

    private void CalcMapVector()
    {
        mapSize = new Vector3(mapCollider.size.x * transform.lossyScale.x, mapCollider.size.y * transform.lossyScale.y, mapCollider.size.z * transform.lossyScale.z);
        mapPosition = new Vector3(mapCollider.center.x + transform.localPosition.x, mapCollider.center.y + transform.localPosition.y, mapCollider.center.z + transform.localPosition.z);
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



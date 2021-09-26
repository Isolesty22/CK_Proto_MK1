using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BearMapInfo : MonoBehaviour
{
    public BoxCollider mapCollider;

    [BeginReadOnlyGroup]
    public BearBlock[] bearBlocks = new BearBlock[5];
    public void InitBearBlocks()
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BearMapInfo :  MonoBehaviour
{
    [ReadOnly]
    public BearBlock[] bearBlocks = new BearBlock[5];
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
            max = _max;
            min = _min;
        }

        public Vector3 max;
        public Vector3 min;
    }

    public Position position = null;
}

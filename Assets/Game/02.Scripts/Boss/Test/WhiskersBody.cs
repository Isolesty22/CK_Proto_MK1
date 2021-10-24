using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
/// <summary>
/// Gloom 수염의 라인입니다.
/// </summary>
public class WhiskersBody : MonoBehaviour
{
    [System.Serializable]
    public class Components
    {
        public LineRenderer lineRenderer;
        public Transform targetDirection;
        public Transform waveDirection;
    }

    [System.Serializable]
    public class Values
    {
        public int length;
        public float targetDist;
        public float smoothSpeed;
        public float trailSpeed;

        public float waveMagnitude; // 흔들림의 파형이 커집니다.
        public float waveSpeed; //흔들리는 스피드입니다.
    }




    [SerializeField]
    private Components _components = new Components();
    [SerializeField]
    private Values _values = new Values();
    private Vector3[] _segmentPoses;

    public Components Com => _components;
    public Values Val => _values;

    public Vector3[] segmentPoses;

    public void Init()
    {
        Com.lineRenderer.positionCount = Val.length;
        segmentPoses = new Vector3[Val.length];
        _segmentPoses = new Vector3[Val.length];
    }
}

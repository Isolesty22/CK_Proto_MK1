using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
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
        public float targetDistance;
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

    [HideInInspector]
    public Vector3[] segmentPoses;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Com.lineRenderer.positionCount = Val.length;
        segmentPoses = new Vector3[Val.length];
        _segmentPoses = new Vector3[Val.length];

        //Com.waveDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * Val.waveSpeed) * Val.waveMagnitude);
        segmentPoses[0] = Com.targetDirection.position;


        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] + Com.targetDirection.right * Val.targetDistance;
            //var bodyPartsLength = Mathf.Clamp(i, 0, bodyParts.Length);
            //bodyParts[bodyPartsLength - 1].transform.position = segmentPoses[bodyPartsLength];
        }

        Com.lineRenderer.SetPositions(segmentPoses);

    }


    private void Update()
    {
        MoveLines();
        
    }
    //private void FixedUpdate()
    //{
    //    MoveLines();
    //}
    private void MoveLines()
    {
        Com.waveDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * Val.waveSpeed) * Val.waveMagnitude);
        segmentPoses[0] = Com.targetDirection.position;


        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + Com.targetDirection.right * Val.targetDistance,
                ref _segmentPoses[i],
                Val.smoothSpeed + i / Val.trailSpeed);

            //if (bodyParts.Length != 0)
            //{
            //    var bodyPartsLength = Mathf.Clamp(i, 0, bodyParts.Length);
            //    bodyParts[bodyPartsLength - 1].transform.position = segmentPoses[bodyPartsLength];
            //}
        }
        Com.lineRenderer.SetPositions(segmentPoses);
    }

//    private void OnDrawGizmos()
//    {

//#if UNITY_EDITOR
//        if (!UnityEditor.EditorApplication.isPlaying)
//        {
//            Com.lineRenderer.SetPosition(0, Com.targetDirection.position);
//        }
//#endif
//    }
}

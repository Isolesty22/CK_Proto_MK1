using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTargetFollwer : MonoBehaviour
{
    public Transform target;
    public Transform myTransform;
    private void Awake()
    {
        myTransform.position = target.position;
    }
    private void Start()
    {
        myTransform.position = target.position;
    }
    // Update is called once per frame
    void Update()
    {
        myTransform.SetPositionAndRotation(target.position, target.rotation);
    }
}

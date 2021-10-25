using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour

{

    public Transform myTransform;
    private void FixedUpdate()
    {


        myTransform.Rotate(new Vector3(0f, -5f, 0f));

    }
}

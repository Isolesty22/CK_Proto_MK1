using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatChecker : MonoBehaviour
{

    public Renderer r;
    // Start is called before the first frame update
    void Start()
    {

        r.material.renderQueue = 3000;
        //Debug.Log(gameObject.name + " : " + );
    }


}

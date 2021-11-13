using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start");
        GameManager.instance.playerController.MoveSystem(Vector3.zero);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

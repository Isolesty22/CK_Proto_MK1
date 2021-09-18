using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{

    BearStateMachine bearStateMachine;

    private void Init()
    {
        bearStateMachine = new BearStateMachine(this);
    }
}

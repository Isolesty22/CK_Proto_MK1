//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{

}


[Serializable]
public class StateInfo
{
    [Tooltip("현재 페이즈")]
    public ePhase phase;
    [Tooltip("현재 상태")]
    public string state = "";
}

[Serializable]
public class BossPhaseValue
{
    public float phase2;
    public float phase3;
}
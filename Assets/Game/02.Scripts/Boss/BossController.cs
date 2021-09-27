//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

}

[System.Serializable]
public class StateInfo
{
    [Tooltip("현재 페이즈")]
    public string phase = "";
    [Tooltip("현재 상태")]
    public string state = "";
}

[System.Serializable]
public class BossPhaseValue
{
    public float phase2;
    public float phase3;
}
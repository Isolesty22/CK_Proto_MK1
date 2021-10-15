using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AnimationEventListener : MonoBehaviour
{
    private Action eventAction;
    public void CallEvent()
    {
        eventAction.Invoke();
    }
}

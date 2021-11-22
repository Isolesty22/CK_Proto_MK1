using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AnimationEventListener : MonoBehaviour
{
    private Action eventAction;
    public void SetEvent(Action _event)
    {
        eventAction = _event;
    }
    public void AddEvent(Action _event)
    {
        eventAction += _event;
    }
    public void CallEvent()
    {
        eventAction?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class KeyOption
{
    //default
  
    [ReadOnly]
    public KeyCode moveRight = KeyCode.RightArrow;
    [ReadOnly]
    public KeyCode moveLeft = KeyCode.LeftArrow;
    [ReadOnly]
    public KeyCode crouch = KeyCode.DownArrow;
    [ReadOnly]
    public KeyCode lookUp = KeyCode.UpArrow;
    [ReadOnly]
    public KeyCode attack = KeyCode.Z;
    [ReadOnly]
    public KeyCode jump = KeyCode.X;
    [ReadOnly]
    public KeyCode counter = KeyCode.C;

    public KeyOption(KeyOption _data)
    {
        this.moveRight = _data.moveRight;
        this.moveLeft = _data.moveLeft;
        this.crouch = _data.crouch;
        this.lookUp = _data.lookUp;
        this.attack = _data.attack;
        this.jump = _data.jump;
        this.counter = _data.counter;
    }

    public KeyOption()
    {
        moveRight = KeyCode.RightArrow;
        moveLeft = KeyCode.LeftArrow;
        crouch = KeyCode.DownArrow;
        lookUp = KeyCode.UpArrow;
        attack = KeyCode.Z;
        jump = KeyCode.X;
        counter = KeyCode.C;
    }

    public void CopyData(KeyOption _data)
    {
        this.moveRight = _data.moveRight;
        this.moveLeft = _data.moveLeft;
        this.crouch = _data.crouch;
        this.lookUp = _data.lookUp;
        this.attack = _data.attack;
        this.jump = _data.jump;
        this.counter = _data.counter;
    }

    public bool IsEquals(KeyOption _data)
    {
        return moveRight == _data.moveRight &&
               moveLeft == _data.moveLeft &&
               crouch == _data.crouch &&
               lookUp == _data.lookUp &&
               attack == _data.attack &&
               jump == _data.jump &&
               counter == _data.counter;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOption
{
    //default
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode crouch = KeyCode.DownArrow;
    public KeyCode lookUp = KeyCode.UpArrow;
    public KeyCode attack = KeyCode.Z;
    public KeyCode jump = KeyCode.X;
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

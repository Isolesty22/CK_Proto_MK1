using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public string name;
    public int hp;
    public int maxHp;

    public void Initialize()
    {
        hp = maxHp;
    }
}

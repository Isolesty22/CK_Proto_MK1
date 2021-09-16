using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Utility
{
    public static void KillTween(Tween tween)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }
}

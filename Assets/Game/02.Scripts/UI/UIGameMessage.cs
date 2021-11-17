using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameMessage : UIBase
{
    private WaitForSeconds waitSec;
    private IEnumerator open;
    private IEnumerator close;
    private IEnumerator openUseDuration;
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        Com.canvas.enabled = false;
        CheckOpen();

        fadeDuration = 0.3f;
        waitSec = new WaitForSeconds(3f);

        open = ProcessOpen();
        close = ProcessClose();
       // openUseDuration = ProcessOpenUseDuration();
    }
}

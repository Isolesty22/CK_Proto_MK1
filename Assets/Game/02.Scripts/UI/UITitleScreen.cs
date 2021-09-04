using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타이틀 화면을 담당한다-!! 
/// </summary>
public class UITitleScreen : UIBase
{


    public override void Init()
    {
        base.Init();
    }

    public IEnumerator ProcessWaitPressAnyKey()
    {
        while (true)
        {
            //아무 키나 눌렀을 경우
            if (Input.anyKeyDown)
            {
                break;
            }

            yield return null;
        }
    }
    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;
    }
    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        return true;
    }

    //protected override IEnumerator ProcessOpen()
    //{
    //    return base.ProcessOpen();
    //}
    //protected override IEnumerator ProcessClose()
    //{
    //    return base.ProcessClose();
    //}
}

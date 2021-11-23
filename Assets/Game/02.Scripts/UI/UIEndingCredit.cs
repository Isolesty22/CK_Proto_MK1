using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndingCredit : UIBase
{

    private void Start()
    {
        UIManager.Instance.AddDict(this);
    }
}

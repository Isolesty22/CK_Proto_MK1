using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTutorialLoader : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.LoadData_Talk("Stage_00"));
        DataManager.Instance.OnSceneLoadEnded();

    }
    private int tempTalkCode = 900;
    public void Button_Talk()
    {
        UIManager.Instance.TalkInfinity(tempTalkCode);
        tempTalkCode += 1;
    }

}

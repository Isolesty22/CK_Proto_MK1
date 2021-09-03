using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{

    public void TestFunc()
    {
        if (SceneChanger.Instance == null)
        {
            Debug.LogWarning("SceneChanger is Null");
        }

        StartCoroutine(SceneChanger.Instance.LoadThisScene_Joke("TestHomeScene"));
    }
}

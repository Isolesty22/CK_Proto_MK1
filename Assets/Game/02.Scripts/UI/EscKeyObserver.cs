using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ESC키 누르는거 확인용
/// </summary>
public class EscKeyObserver : MonoBehaviour
{
    public Image escImage;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escImage.color = Color.gray;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            escImage.color = Color.white;
        }
    }
}

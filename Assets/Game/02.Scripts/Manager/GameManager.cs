using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CameraManager cameraManager;
    public PlayerController playerController;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;

            //   GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }

}

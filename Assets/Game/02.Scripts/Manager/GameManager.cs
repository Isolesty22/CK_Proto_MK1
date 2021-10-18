using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CameraManager cameraManager
    {
        get
        {
            if (_cameraManager == null)
            {
                _cameraManager = FindObjectOfType<CameraManager>();
            }
            return _cameraManager;
        }
        set
        {
            _cameraManager = value;
        }
    }
    private CameraManager _cameraManager; 
    public PlayerController playerController
    {
        get
        {
            if (_playerController == null)
            {
                _playerController = FindObjectOfType<PlayerController>();
            }
            return _playerController;
        }
        set
        {
            _playerController = value;
        }

    }
    private PlayerController _playerController; 

    //해야함 : OnLevelWasLoaded 사용하지 말기
    private void OnLevelWasLoaded_(int level)
    {
        Debug.Log("Test");
        if (instance == null || instance != this)
        {
            instance = this;
            //   GameObject.DontDestroyOnLoad(this.gameObject);
        }

        if (cameraManager == null)
        {
            cameraManager = FindObjectOfType<CameraManager>();
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }
    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;

            //   GameObject.DontDestroyOnLoad(this.gameObject);
        }

        if (cameraManager == null)
        {
            cameraManager = FindObjectOfType<CameraManager>();
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

}

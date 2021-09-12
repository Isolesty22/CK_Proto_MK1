using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform pivot;
    public Camera mainCam;

    private float cameraHalfWidth = 0f;
    private float cameraHalfHeight = 0f;

    public float limitMinX;
    public float limitMaxX;
    public float limitMinY;
    public float limitMaxY;

    public float smoothSpeed =1f;
    public Vector3 curVelocity;

    public Vector3 offset = new Vector3(0, 1, -8);



    private void Awake()
    {
        mainCam = Camera.main;

        mainCam.transform.localPosition = offset;

        cameraHalfWidth = mainCam.aspect * mainCam.orthographicSize;
        cameraHalfHeight = mainCam.orthographicSize;
    }

    private void FixedUpdate()
    {
        var target = GameManager.instance.playerController.transform.position;

        mainCam.transform.localPosition = offset;

        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            0);                                                                                // Z

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}

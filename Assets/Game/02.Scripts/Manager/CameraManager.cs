using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Transform pivot;
    public Camera mainCam;
    public Camera vfxCam;

    private float cameraHalfWidth = 0f;
    private float cameraHalfHeight = 0f;

    public float limitMinX;
    public float limitMaxX;
    public float limitMinY;
    public float limitMaxY;

    public float followUp;

    public float smoothSpeed = 1f;
    public Vector3 curVelocity;

    public Vector3 offset = new Vector3(0, 1, -8);

    [System.Serializable]
    public class CameraShakeValue
    {
        public float time;
        public float amplitude;
        public float frequency;
    }
    public CameraShakeValue shakeValue;
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin vcamNoise;

    private IEnumerator shakeCoroutine = null;
    private void Awake()
    {
        //mainCam = Camera.main;

        mainCam.transform.localPosition = offset;
        vfxCam.transform.localPosition = offset;

        cameraHalfWidth = mainCam.aspect * mainCam.orthographicSize;
        cameraHalfHeight = mainCam.orthographicSize;

        vcamNoise = vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    //private void FixedUpdate()
    //{
    //    var target = GameManager.instance.playerController.transform.position;

    //    mainCam.transform.localPosition = offset;
    //    vfxCam.transform.localPosition = offset;

    //    Vector3 desiredPosition = new Vector3(
    //        Mathf.Clamp(target.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
    //        0, // Y
    //        0);                                                                                // Z

    //    Vector3 desiredPositionUp = new Vector3(
    //        0,   // X
    //        Mathf.Clamp(target.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
    //        0);
    //    //    Vector3 desiredPosition = new Vector3(
    //    //Mathf.Clamp(target.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
    //    //0, // Y
    //    //0);

    //    Vector3 finalPosition = desiredPosition;

    //    transform.position = Vector3.Lerp(transform.position, desiredPositionUp, Time.deltaTime * smoothSpeed);

    //    //if (target.y > transform.position.y + followUp)
    //    //{
    //    //    Debug.Log("work");
    //    //    transform.position = Vector3.Lerp(transform.position, desiredPositionUp, Time.deltaTime * smoothSpeed);
    //    //}
    //    //else
    //    //{
    //    //    transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    //    //}

    //}

    float timer = 0f;

    public void SetShakeValue(float _amplitude,float _frequency)
    {
        shakeValue.amplitude = _amplitude;
        shakeValue.frequency = _frequency;
        vcamNoise.m_AmplitudeGain = shakeValue.amplitude;
        vcamNoise.m_FrequencyGain = shakeValue.frequency;
    }
    private IEnumerator ProcessCameraShake()
    {
        vcamNoise.m_AmplitudeGain += 1f;
        vcamNoise.m_FrequencyGain += 1f;

        timer = 0f;
        float progress = 0f;
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / shakeValue.time;
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        vcamNoise.m_AmplitudeGain -= 1f;
        vcamNoise.m_FrequencyGain -= 1f;
        shakeCoroutine = null;
    }

    public void ShakeCamera()
    {
        if (shakeCoroutine == null)
        {
            shakeCoroutine = ProcessCameraShake();
            StartCoroutine(shakeCoroutine);
        }
        else
        {
            timer = 0f;
        }
    }

}


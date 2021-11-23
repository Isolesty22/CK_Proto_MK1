using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GloomResonanceScreen : MonoBehaviour
{

    [HideInInspector]
    public GloomController gloom;

    public Image darkImage;
    public Image faceImage;


    [System.Serializable]
    public struct TimeValue
    {
        [Header("까만화면")]
        public float dark_begin;
        public float dark_duration;
        public float dark_end;

        [Header("얼굴")]
        public float face_begin;
        public float face_duration;
        public float face_end;
    }

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private TimeValue timeVal;
    private WaitForSeconds waitTime = null;
    private float progress = 0f;
    private float timer = 0f;

    private void Awake()
    {
        canvas.enabled = false;
        waitTime = new WaitForSeconds(timeVal.face_duration);
    }
    public void StartResonanceScreen()
    {
        StartCoroutine(CoResonanceScreen());
    }

    private Color32 blackZero = new Color32(0, 0, 0, 0);
    private Color32 blackOne = new Color32(0, 0, 0, 255);

    private Color32 whiteZero = new Color32(255, 255, 255, 0);
    private Color32 whiteOne = new Color32(255, 255, 255, 255);
    private IEnumerator CoResonanceScreen()
    {
        darkImage.color = blackZero;
        faceImage.color = whiteZero;
        canvas.enabled = true;

        ClearTimer();
        while (timer < timeVal.dark_begin)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.dark_begin;

            darkImage.color = Color32.Lerp(blackZero, blackOne, progress);
            yield return null;
        }


        yield return waitTime;
        ClearTimer();
        while (timer < timeVal.face_begin)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.face_begin;

            faceImage.color = Color32.Lerp(whiteZero, whiteOne, progress);
            yield return null;
        }
        gloom.audioSource.PlayOneShot(gloom.audioClips.resonanaceScreen);
        yield return waitTime;

        ClearTimer();
        while (timer < timeVal.face_end)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.face_end;
            faceImage.color = Color32.Lerp(whiteOne, whiteZero, progress);
            darkImage.color = Color32.Lerp(blackOne, blackZero, progress);
            yield return null;
        }
        if (!GameManager.instance.playerController.IsInvincible())
        {
            GameManager.instance.playerController.Hit();
        }


        canvas.enabled = false;
    }

    private void ClearTimer()
    {
        timer = 0f;
        progress = 0f;
    }

}

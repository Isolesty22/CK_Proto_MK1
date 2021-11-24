using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GloomResonanceScreen : MonoBehaviour
{

    [HideInInspector]
    public GloomController gloom;

    public Image darkImage;
    public Image faceImage;

    public Volume volume;
    private Vignette vignette;
    private float originSmooth;
    private float originIntensity;
    public WaitForSeconds waitFaceTime { get; private set; }
    public WaitForSeconds waitEndTime { get; private set; }
    [System.Serializable]
    public struct TimeValue
    {
        [Header("까만화면")]
        public float dark_begin;
        public float dark_duration;

        [Header("얼굴")]
        public float face_begin;
        public float face_duration;
        public float face_end;

        [HideInInspector]
        public float commingDarkTime;
    }

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private TimeValue timeVal;

    private IEnumerator currentCoroutine;
    private WaitForSeconds waitTime = null;
    private float progress = 0f;
    private float timer = 0f;

    private void Awake()
    {
        canvas.enabled = false;
        waitTime = new WaitForSeconds(timeVal.face_duration);
        waitFaceTime = new WaitForSeconds(
            timeVal.dark_begin +
            timeVal.dark_duration +
            timeVal.face_begin + timeVal.face_duration
            );
        waitEndTime = new WaitForSeconds(
            timeVal.face_duration +
            timeVal.face_end
            );

        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            originSmooth = vignette.smoothness.value;
            originIntensity = vignette.intensity.value;
        }
        else
        {
            Debug.LogWarning("공명에 필요한 비네트를 찾을 수 없었습니다.");

        }
    }
    public void StartResonanceScreen()
    {
        //어두워지는 시간 설정
        timeVal.commingDarkTime = gloom.SkillVal.resonance.resonanceTime;

        currentCoroutine = CoCommingDark();
        StartCoroutine(currentCoroutine);
    }
    public void StopResoanceScreen()
    {
        StopCoroutine(currentCoroutine);
        ClearTimer();
        SetVignetteValue(originIntensity, originSmooth);
        vignette.rounded.value = false;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        darkImage.color = blackZero;
        faceImage.color = whiteZero;
        canvas.enabled = false;
    }

    private Color32 blackZero = new Color32(0, 0, 0, 0);
    private Color32 blackOne = new Color32(0, 0, 0, 255);

    private Color32 whiteZero = new Color32(255, 255, 255, 0);
    private Color32 whiteOne = new Color32(255, 255, 255, 255);


    /// <summary>
    /// 비네팅을 점점 어둡게 합니다.
    /// </summary>
    private IEnumerator CoCommingDark()
    {
        ClearTimer();

        vignette.rounded.value = true;

        if (gloom.diretion == eDirection.Right)
        {
            vignette.center.value = new Vector2(0.7f, 0.5f);
        }
        else
        {
            vignette.center.value = new Vector2(0.3f, 0.5f);

        }
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.commingDarkTime;

            float tempIntensity = Mathf.Lerp(0f, 1f, progress);
            float tempSmooth = Mathf.Lerp(originSmooth, 1f, progress);

            SetVignetteValue(tempIntensity, tempSmooth);
            yield return null;
        }
        currentCoroutine = CoResonanceScreen();
        StartCoroutine(currentCoroutine);
    }

    /// <summary>
    /// 얼굴을 띄웁니다.
    /// </summary>
    private IEnumerator CoResonanceScreen()
    {
        darkImage.color = blackZero;
        faceImage.color = whiteZero;
        canvas.enabled = true;

        //검은 화면 켜기
        ClearTimer();
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.dark_begin;

            darkImage.color = Color32.Lerp(blackZero, blackOne, progress);
            yield return null;
        }

        yield return waitTime;

        //얼굴 켜기
        ClearTimer();
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.face_begin;

            faceImage.color = Color32.Lerp(whiteZero, whiteOne, progress);
            yield return null;
        }

        //쿵 효과음
        gloom.audioSource.PlayOneShot(gloom.audioClips.resonanaceScreen);

        //비네트를 원래대로 복귀
        SetVignetteValue(originIntensity, originSmooth);
        vignette.rounded.value = false;
        vignette.center.value = new Vector2(0.5f, 0.5f);

        yield return waitTime;

        //화면 끄기
        ClearTimer();
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / timeVal.face_end;
            faceImage.color = Color32.Lerp(whiteOne, whiteZero, progress);
            darkImage.color = Color32.Lerp(blackOne, blackZero, progress);
            yield return null;
        }
        canvas.enabled = false;
    }

    private void ClearTimer()
    {
        timer = 0f;
        progress = 0f;
    }

    private void SetVignetteValue(float _intensity, float _smoothness)
    {
        vignette.intensity.value = _intensity;
        vignette.smoothness.value = _smoothness;
    }
}

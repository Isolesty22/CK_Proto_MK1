using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlashScreen : UIBase
{

    [System.Serializable]
    public struct FlashValue
    {

        [Header("페이드 관련")]

        [Tooltip("startSpeed초 동안 페이드인 합니다.")]
        public float startSpeed;
        [Tooltip("페이드 지속 시간")]
        public float waitTime;
        [Tooltip("endSpeed초 동안 페이드아웃 합니다.")]
        public float endSpeed;

        [Header("깜빡임 관련")]
        [Tooltip("화면을 몇 번 깜빡일 것인지 정합니다.")]
        public int blinkCount;
        [Tooltip("화면이 깜빡이는 간격입니다. 하얀 화면이 blinkInterval마다 켜집니다.")]
        public float blinkInterval;
        [Tooltip("화면이 깜빡이는 속도입니다. 하얀 화면이 blinkSpeed초동안 유지됩니다.")]
        public float blinkSpeed;
    }

    [SerializeField]
    private FlashValue flashVal;
    private WaitForSeconds waitTime = null;
    private float progress = 0f;
    private float timer = 0f;

    private void Awake()
    {
        Com.canvas.enabled = false;
        waitTime = new WaitForSeconds(flashVal.waitTime);
    }
    private void Start()
    {
        UIManager.Instance.AddDict(this);
        gameObject.SetActive(false);
    }
    public void StartFlashScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(CoFlashScreen());
    }
    private IEnumerator CoFlashScreen()
    {
        Com.canvasGroup.alpha = 0f;
        Com.canvas.enabled = true;

        //-----------------
        //페이드인
        //-----------------
        ClearTimer();
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flashVal.startSpeed;

            Com.canvasGroup.alpha = progress;
            yield return null;
        }

        Com.canvasGroup.alpha = 1f;

        yield return waitTime;

        //-------------------
        //페이드아웃
        //-------------------
        ClearTimer();

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / flashVal.endSpeed;

            Com.canvasGroup.alpha = 1f - progress;
            yield return null;
        }

        Com.canvas.enabled = false;
        gameObject.SetActive(false);
    }

    private void ClearTimer()
    {
        timer = 0f;
        progress = 0f;
    }

}

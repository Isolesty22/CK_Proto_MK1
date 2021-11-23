using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlashScreen : UIBase
{

    [System.Serializable]
    public struct FlashValue
    {

        [Header("���̵� ����")]

        [Tooltip("startSpeed�� ���� ���̵��� �մϴ�.")]
        public float startSpeed;
        [Tooltip("���̵� ���� �ð�")]
        public float waitTime;
        [Tooltip("endSpeed�� ���� ���̵�ƿ� �մϴ�.")]
        public float endSpeed;

        [Header("������ ����")]
        [Tooltip("ȭ���� �� �� ������ ������ ���մϴ�.")]
        public int blinkCount;
        [Tooltip("ȭ���� �����̴� �����Դϴ�. �Ͼ� ȭ���� blinkInterval���� �����ϴ�.")]
        public float blinkInterval;
        [Tooltip("ȭ���� �����̴� �ӵ��Դϴ�. �Ͼ� ȭ���� blinkSpeed�ʵ��� �����˴ϴ�.")]
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
        //���̵���
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
        //���̵�ƿ�
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

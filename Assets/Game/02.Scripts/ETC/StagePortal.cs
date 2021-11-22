using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ���ӸŴ����� StageClear�� ȣ���մϴ�. 
/// </summary>
public class StagePortal : MonoBehaviour
{
    public Collider col;

    public CanvasGroup canvasGroup;
    public Image image;


    private RectTransform rectTransform;
    [Tooltip("true�� ���, 2�ʰ� �ɾ �� ���������� �̵��մϴ�.")]
    public bool moveOnEnter;

    [Tooltip("false�� ���, Awake�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.")]
    public bool activeOnAwake;
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        if (activeOnAwake)
        {
            gameObject.SetActive(true);
            rectTransform = image.rectTransform;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            col.enabled = false;

            if (moveOnEnter)
            {
                StartCoroutine(CoStageClear());
                StartCoroutine(CoFadeClearImage());
            }
            else
            {
                GameManager.instance.StageClear();
            }
        }
    }

    Vector2 leftPos = new Vector2(-2000f, 0f);
    Vector2 endPos = new Vector2(0f, 0f);
    private IEnumerator CoFadeClearImage()
    {
        rectTransform.anchoredPosition = leftPos;

//canvasGroup.alpha = 0f;

        float progress = 0f;
        float timer = 0f;
        canvasGroup.alpha = 1f;
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / 0.5f;

            rectTransform.anchoredPosition = Vector2.Lerp(leftPos, endPos, progress);
            //canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }
    }
    private IEnumerator CoStageClear()
    {
        GameManager.instance.playerController.State.isCrouching = false;
        GameManager.instance.playerController.State.isLookUp = false;
        GameManager.instance.playerController.State.isAttack = false;

        //UI ����
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Close();

        //���������� �̵�
        GameManager.instance.cameraManager.vcam.Follow = null;
        GameManager.instance.playerController.State.moveSystem = true;
        GameManager.instance.playerController.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(2f);
        GameManager.instance.StageClear();
    }
}

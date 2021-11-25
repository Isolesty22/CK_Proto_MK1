using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ���ӸŴ����� StageClear�� ȣ���մϴ�. 
/// </summary>
public class StagePortal : MonoBehaviour
{

    public GameObject parentObject;
    public Collider col;

    public CanvasGroup canvasGroup;
    public Image image;

    private readonly float talkTime = 3f;

    private RectTransform rectTransform;
    [Tooltip("true�� ���, 2�ʰ� �ɾ �� ���������� �̵��մϴ�.")]
    public bool moveOnEnter;

    [Tooltip("false�� ���, Awake�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.")]
    public bool activeOnAwake;

    private WaitForSeconds waitSec;

    private string nowSceneName;
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        if (activeOnAwake)
        {
            parentObject.SetActive(true);
            rectTransform = image.rectTransform;
        }
        else
        {
            parentObject.SetActive(false);
        }

        waitSec = new WaitForSeconds(talkTime);
    }

    public void Active()
    {
        parentObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            col.enabled = false;
            nowSceneName = SceneChanger.Instance.GetNowSceneName();

            if (moveOnEnter)
            {
                switch (nowSceneName)
                {
                    case SceneNames.stage_01:
                        StartCoroutine(CoStage01Clear());
                        break;

                    case SceneNames.stage_03:
                        StartCoroutine(CoStage03Clear());
                        break;
                }
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
    private IEnumerator CoStage01Clear()
    {
        ActiveMoveSystem(true);

        //UI ����
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Close();

        //ī�޶� ����
        GameManager.instance.cameraManager.vcam.Follow = null;

        AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(AudioManager.Instance.clipDict_SFX["Bear_ForwardRoar"]);
        yield return StartCoroutine(CoWaitTalkEnd());

        //��ȭ �߱�
        UIManager.Instance.Talk(DataManager.Instance.stageCode, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());

        UIManager.Instance.Talk(DataManager.Instance.stageCode + 1, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());


        //���������� �̵�
        StartCoroutine(CoFadeClearImage());
        GameManager.instance.playerController.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(1.3f);
        GameManager.instance.StageClear();
    }

    private IEnumerator CoStage03Clear()
    {
        ActiveMoveSystem(true);

        //UI ����
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Close();

        //ī�޶� ����
        GameManager.instance.cameraManager.vcam.Follow = null;

        //��ȭ �߱�
        UIManager.Instance.Talk(DataManager.Instance.stageCode, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());

        UIManager.Instance.Talk(DataManager.Instance.stageCode + 1, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());
        yield return new WaitForSeconds(0.6f);


        //���������� �̵�
        StartCoroutine(CoFadeClearImage());
        GameManager.instance.playerController.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(1.3f);
        GameManager.instance.StageClear();
    }



    private WaitForSeconds waitSsec = new WaitForSeconds(0.1f);
    private IEnumerator CoWaitTalkEnd()
    {
        float timer = 0f;

        while (timer < talkTime)
        {
            timer += Time.deltaTime;

            if (IsInputSkipKey())
            {
                UIManager.Instance.TalkEnd();
                yield return waitSsec;
                yield break;
            }
            yield return null;
        }
    }

    private bool IsInputSkipKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActiveMoveSystem(bool _b)
    {
        if (_b)
        {
            GameManager.instance.playerController.InputVal.movementInput = 0f;

            GameManager.instance.playerController.State.isCrouching = false;
            GameManager.instance.playerController.State.isLookUp = false;
            GameManager.instance.playerController.State.isAttack = false;

            GameManager.instance.playerController.State.moveSystem = true;
        }
        else
        {
            GameManager.instance.playerController.State.moveSystem = false;
        }
    }
}

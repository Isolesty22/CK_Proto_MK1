using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���ӸŴ����� StageClear�� ȣ���մϴ�. 
/// </summary>
public class StagePortal : MonoBehaviour
{
    public Collider col;
    private PlayerController player;

    [Tooltip("true�� ���, 2�ʰ� �ɾ �� ���������� �̵��մϴ�.")]
    public bool moveOnEnter;

    [Tooltip("false�� ���, Awake�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.")]
    public bool activeOnAwake;
    private void Awake()
    {
        if (activeOnAwake)
        {
            gameObject.SetActive(true);
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

    private void Start()
    {
        player = GameManager.instance.playerController;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            col.enabled = false;

            if (moveOnEnter)
            {
                StartCoroutine(CoStageClear());
            }
            else
            {
                GameManager.instance.StageClear();

            }
        }
    }

    private IEnumerator CoStageClear()
    {

        player.State.isCrouching = false;
        player.State.isLookUp = false;
        player.State.isAttack = false;

        //UI ����
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Close();

        //���������� �̵�
        GameManager.instance.cameraManager.vcam.Follow = null;
        player.State.moveSystem = true;
        player.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(2f);
        GameManager.instance.StageClear();
    }
}

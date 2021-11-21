using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���ӸŴ����� StageClear�� ȣ���մϴ�. 
/// </summary>
public class StagePortal : MonoBehaviour
{
    public Collider col;

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

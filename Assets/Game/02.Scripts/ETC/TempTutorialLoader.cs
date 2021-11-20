using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTutorialLoader : MonoBehaviour
{
    private static TempTutorialLoader instance;
    public static TempTutorialLoader Instance;


    private PlayerController player;


    private UIPlayerHP uiPlayer;
    public string currentCoName { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("�̹� instance�� �����մϴ�." + this);
            if (Instance != this) //�� �ڽ��� �ν��Ͻ��� �ƴ� ���
            {
                Debug.Log(this + " : �� �̻�, �� ���輱������ ������ �� ���� �� ����... �ȳ�.");
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.LoadData_Talk("Stage_00"));
        DataManager.Instance.UpdateTalkData();

        yield return null;
        player = GameManager.instance.playerController;
        uiPlayer = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        StartCoroutine(CoBeginTutorial());

    }

    //------------------------------------------------------

    public KeyOption key
    {
        get
        {
            return DataManager.Instance.currentData_settings.keySetting;
        }
    }


    private Dictionary<string, TutorialCollider> tcDict = new Dictionary<string, TutorialCollider>();
    private Dictionary<string, IEnumerator> coDict = new Dictionary<string, IEnumerator>();

    private const float talkTime = 3f;
    private WaitForSeconds wait3sec = new WaitForSeconds(3f);
    private WaitForSeconds waitSsec = new WaitForSeconds(0.1f);
    private UIGameMessage gameMessage;

    public void AddDict(string _name, TutorialCollider _tc)
    {
        tcDict.Add(_name, _tc);
    }

    public void CloseCollider(string _name)
    {
        tcDict[_name].Close();
    }

    public void StartCoPrac(string _coName)
    {
        StopCoroutine(currentCoName);
        currentCoName = _coName;
        StartCoroutine(_coName);
    }
    private void CanMove(bool _b)
    {
        if (_b)
        {
            uiPlayer.Open();
        }
        else
        {
            uiPlayer.Close();
        }

        player.State.isCrouching = false;
        player.InputVal.movementInput = 0f;
        player.State.moveSystem = !_b;
    }
    /// <summary>
    /// Ʃ�丮�� ó�� �������� �� �ߴ� ���
    /// </summary>
    private IEnumerator CoBeginTutorial()
    {
        //CloseCollider("CoBeginTutorial");
        yield return YieldInstructionCache.WaitForEndOfFrame;

        gameMessage = UIManager.Instance.GetUI("UIGameMessage") as UIGameMessage;
        gameMessage.SetWaitTime(100f);

        MessageOpen("[�����̽� ��]Ű�� ��ȭ�� ��ŵ�� �� �ֽ��ϴ�.");

        CanMove(false);
        Talk("����, �ڱ�Ұ��� �����߳�! �� �̸��� ��̿���.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("���� �ǵ����� ���ؼ�, ��ȭ�� ���� �ʿ���.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("������! �ʿ��� �� ��ȭ�� ���� ���� �����ٰ�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("�ƹ�����, ó������ ���� �Ϻ��� �ٷ��� �������?");
        yield return StartCoroutine(CoWaitTalkEnd());


        Talk("�������� �ϳ��� �����غ���.");
        yield return new WaitForSeconds(1f);

        StartCoroutine(CoPrac_Move());

    }


    private IEnumerator CoPrac_Move()
    {
        //CloseCollider("CoPrac_Move");
        GameManager.instance.playerController.Com.pixy.getPixy = true;
        Talk("�ϴ�, ���� �� ����������?");
        MessageOpen("ȭ��ǥ [��],[��] Ű�� �̵��� �� �ֽ��ϴ�.");
        CanMove(true);

        while (true)
        {

            if (GetKey(key.moveRight) || GetKey(key.moveLeft))
            {
                break;
            }
            yield return null;
        }
        //TalkEnd();
        Talk("����! �̴�� ����~�� �볪�� �ִ� �� ���� ������!");
    }

    private IEnumerator CoPrac_Jump()
    {
        CloseCollider("CoPrac_Jump");

        MessageOpen("[X]Ű�� ������ �� �ֽ��ϴ�.");

        Talk("�� ������� �پ���� �� �����ž�. �� �� �پ��?");

        while (!GetKey(key.jump))
        {
            yield return null;
        }

        Talk("��, ������ �����Ѱ����ס�.\n��Ǻ���غ����� �Ҿ��ߴµ� ��������.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("��? �ƴϾ�! �ƹ� ���� ���߾�^_^!");
        yield return StartCoroutine(CoWaitTalkEnd());

        TalkInfinity("�� �� ������ ������? ^_^");
        
        MessageClose();
    }

    private IEnumerator CoPrac_Crouch()
    {
        CloseCollider("CoPrac_Crouch");

        MessageClose();
        CanMove(false);
        Talk("���� ���� �� ���ס�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        MessageOpen("[��]Ű�� ��ũ�� �� �ֽ��ϴ�.");
        CanMove(true);

        Talk("���Ǿƴ� ������ �ξ� ��ġ�� ũ�ϱ�, \n���� ��ũ���� ���������Ұž�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        while (!GetKey(key.crouch))
        {
            yield return null;
        }
        TalkInfinity("����, õõ�� ��������.");
    }

    private IEnumerator CoPrac_Attack_Default()
    {
        MessageClose();
        CloseCollider("CoPrac_Attack_Default");
        Talk("��;�, �䳢��!");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("�Ѹ���� �� �����غ�����,\n������ �̹� ��ҿ� �������Ⱦ.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("���Ǿ�, ��ȭ�� ���� ����� ����.");

        MessageOpen("[Z]Ű�� ������ �� �ֽ��ϴ�.");

        while (!GetKey(key.attack))
        {
            yield return null;
        }

        //Talk("��ȭ�� ȭ���̶��, ���ִ°�!");
    }

    private IEnumerator CoPrac_Attack_Up()
    {
        CloseCollider("CoPrac_Attack_Up");
        MessageOpen("[��]Ű�� ���� ������ �� �ֽ��ϴ�. \n[Z]Ű�� �Բ� ����ϸ� ���� ���� ������ �� �ֽ��ϴ�.");

        while (!GetKey(key.lookUp))
        {
            yield return null;
        }


        //Talk("���Ǿƴ� ������ �� �ٷ�±���? �ɼ��غ���.");
    }

    private IEnumerator CoPrac_Parrying()
    {
        CloseCollider("CoPrac_Parrying");

        Talk("�̰�, �� �� �ڴٰ� ������ �� �ִ� ���̰� �ƴϳס�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("�� ���ù����� ���ð��� ���̴�? \n���� ��� �ڴٸ� �Ѿ �� �����ž�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("������! ��ȭ�� ���� ����Ѵٸ�\n��ó���� ������ �� �����ž�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        MessageOpen("���� ��, ���� ����� �� [X]Ű�� ����ϸ� \n'�и�'�� �Ͽ� ���� ������ �� �� �ֽ��ϴ�. ");

        while (!GameManager.instance.playerController.State.isParrying)
        {
            yield return null;
        }

        Talk("���Ҿ�! �̴�� ����������.");

        MessageOpen("'�и�'�� ���� ��� ������\n�� ���̰� �����ؼ� ����� �� �ֽ��ϴ�. ");
    }

    private IEnumerator CoPrac_Attack_Power()
    {
        CloseCollider("CoPrac_Attack_Power");

        Talk("�� �ӿ��� ����� ��ȭ�� ���� ������ �ٽ� ���ƿ��� ��. \n������?");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("��ȯ�� ���� �������� ��Ƶ��״�, \n���Ǿư� �ʿ��� �� ���ϸ� ������ �ɸ��� ���� �غ���.");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("���� ��Ƽ� �� �� �غ���?");
        yield return StartCoroutine(CoWaitTalkEnd());

        Talk("���� �볪���� ������ ��ȭ�� ���� ��ȯ���Ѻ�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        MessageOpen("���Ϳ��� ������ ���߽�Ű�ų� '�и�'�� ������Ű��\n���� ��� UI�� ��ȭ �������� ȹ���� �� �ֽ��ϴ�.");
        yield return StartCoroutine(CoWaitTalkEnd());
        MessageOpen("�������� ������ ȹ���Ͽ� ���� �� ���̾� �� ������, \n��̿��� '������'�� ����� �� �ֽ��ϴ�.");
        while (!GameManager.instance.playerController.Com.pixy.isAttack)
        {
            yield return null;
        }
        MessageOpen("�������� ���ͳ� ������ ������ �� �ֽ��ϴ�. ");

        Talk("����? �� �׳� ����ٴϱ⸸ �ϴ� ������Ʈ�� �ƴϾ�!");
    }

    private IEnumerator CoPrac_Attack_Ult()
    {

        CloseCollider("CoPrac_Attack_Ult");

        Talk("���� ���ݸ� �� ������,\n ��� �ͺ��� ����� �� �� �� �־�.");
        yield return StartCoroutine(CoWaitTalkEnd());

        yield return null;

        MessageOpen("��ȭ �������� ������ ä�� ���� ��Ȳ������ �����,\n ��̿��� '�ñر�'�� ����� �� �ֽ��ϴ�.");

        while (!GameManager.instance.playerController.Com.pixy.isUlt)
        {
            yield return null;
        }

        MessageOpen("�ñر⸦ ����ϰ� ���� ������ ��ȭ �������� ȹ���� �� �����ϴ�.");

        Talk("�����! �� ��ȭ���ѹ����ھ�!");
    }

    private void Talk(string _str)
    {
        UIManager.Instance.Talk(_str, talkTime);
    }
    private void TalkInfinity(string _str)
    {
        UIManager.Instance.TalkInfinity(_str);
    }
    private void TalkEnd()
    {
        UIManager.Instance.TalkEnd();
    }


    private IEnumerator CoWaitTalkEnd()
    {
        float timer = 0f;

        while (timer < talkTime)
        {
            timer += Time.deltaTime;
            if (IsInputSkipKey())
            {
                yield return waitSsec;
                yield break;
            }
            yield return null;
        }
    }


    private void MessageOpen(string _str)
    {
        gameMessage.Open(_str);
    }

    private void MessageClose()
    {
        gameMessage.Close();
    }
    private bool GetKey(KeyCode _keyCode) => Input.GetKey(_keyCode);
    private bool GetKeyDown(KeyCode _keyCode) => Input.GetKey(_keyCode);

    /// <summary>
    /// ��ȭ�� ��ŵ�� �� �ִ� Ű�� ���ȴ°�?
    /// </summary>
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

    private void OnDestroy()
    {
        coDict = null;
        tcDict = null;
    }
}

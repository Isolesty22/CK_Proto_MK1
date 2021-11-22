using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTutorialLoader : MonoBehaviour
{
    public static TempTutorialLoader Instance;

    private PlayerController player;
    [Tooltip("조작 불가 텍스트")]
    public GameObject uiText_control;

    private UIPlayerHP uiPlayer;
    public string currentCoName { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
            if (Instance != this) //나 자신이 인스턴스가 아닐 경우
            {
                Debug.Log(this + " : 더 이상, 이 세계선에서는 존재할 수 없을 것 같아... 안녕.");
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(DataManager.Instance.LoadData_Talk("Stage_00"));
        DataManager.Instance.UpdateTalkData();

        yield return null;
        currentTalkCode = 900;
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
        CloseCollider(_coName);
    }
    private void CanMove(bool _b)
    {
        if (_b)
        {
            if (!uiPlayer.isOpen)
            {
                uiPlayer.Open();
            }

        }
        else
        {
            if (uiPlayer.isOpen)
            {
                uiPlayer.Close();
            }

        }

        player.State.isCrouching = false;
        player.State.isLookUp = false;
        player.State.isAttack = false;
        player.InputVal.movementInput = 0f;
        player.State.moveSystem = !_b;
        uiText_control.SetActive(!_b);
    }

    /// <summary>
    /// _uib가 true일 경우 플레이어 UI를 함께 열거나 닫습니다.
    /// </summary>
    private void CanMove(bool _b, bool _uib)
    {

        if (_uib)
        {
            if (_b)
            {
                uiPlayer.Open();
            }
            else
            {
                uiPlayer.Close();
            }
        }

        player.State.isCrouching = false;
        player.State.isLookUp = false;
        player.State.isAttack = false;
        player.InputVal.movementInput = 0f;
        player.State.moveSystem = !_b;
        uiText_control.SetActive(!_b);
    }


    /// <summary>
    /// 튜토리얼에 처음 입장했을 때 뜨는 대사
    /// </summary>
    private IEnumerator CoBeginTutorial()
    {
        //CloseCollider("CoBeginTutorial");
        yield return YieldInstructionCache.WaitForEndOfFrame;

        gameMessage = UIManager.Instance.GetUI("UIGameMessage") as UIGameMessage;
        gameMessage.SetWaitTime(100f);

        MessageOpen("[스페이스 바]키로 대화를 스킵할 수 있습니다.");

        CanMove(false);
        ////Talk("아차, 자기소개를 깜빡했네! 내 이름은 루미에야 어쩌구");
        Talk(900);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("숲을 되돌리기 위해선, 정화의 힘이 필요해. 어쩌구저쩌구");
        Talk(901);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("걱정마! 너에게 내 정화의 힘을 조금 나눠줄게.");
        Talk(902);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("아무래도, 처음부터 힘을 완벽히 다루기는 힘들겠지?");
        Talk(903);
        yield return StartCoroutine(CoWaitTalkEnd());


        //Talk("차근차근 하나씩 연습해보자.");
        Talk(904);
        yield return new WaitForSeconds(1f);

        StartCoroutine(CoPrac_Move());

    }


    private IEnumerator CoPrac_Move()
    {
        //CloseCollider("CoPrac_Move");
        GameManager.instance.playerController.Com.pixy.getPixy = true;
        //Talk("일단, 몸을 좀 움직여볼까?");
        Talk(905);
        MessageOpen("화살표 [←],[→] 키로 이동할 수 있습니다.");
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
        Talk(906);
        //Talk("좋아! 이대로 저어~기 통나무 있는 곳 까지 가보자!");
    }

    private IEnumerator CoPrac_Jump()
    {
        MessageOpen("[X]키로 점프할 수 있습니다.");

        //Talk("이 정도라면 뛰어넘을 수 있을거야. 한 번 뛰어볼까?");
        Talk(907);
        while (!GetKey(key.jump))
        {
            yield return null;
        }

        //Talk("흠, 사지는 멀쩡한가보네….\n비실비실해보여서 불안했는데 말이지….");
        Talk(908);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("앗? 아니야! 아무 말도 안했어(*^_^*)!");
        Talk(909);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("좀 더 앞으로 가볼까?(*^_^*)");
        Talk(910);

        MessageClose();
    }

    private IEnumerator CoPrac_Crouch()
    {
        MessageClose();
        CanMove(false);
        //Talk("여긴 길이 좀 낮네…. 어쩌구 대충 낮다는 내용");
        Talk(911);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("이피아는 나보다 훨씬 덩치가 크니까, \n몸을 웅크려서 지나가야할거야.");
        Talk(912);
        MessageOpen("화살표 [↓]키로 웅크릴 수 있습니다.");
        CanMove(true);

        while (!GetKey(key.crouch))
        {
            yield return null;
        }
        //Talk("좋아, 천천히 지나가자.");
        Talk(913);
    }

    private IEnumerator CoPrac_Attack_Default()
    {
        MessageClose();

        CanMove(false);
        //Talk("우와앗, 토끼다!");
        Talk(914);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("안보인다구? 나도 안보여…나중에 해달라고 하자….");
        Talk(915);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("아무튼! 겉모습은 좀 멀쩡해보여도,\n정신은 이미 어둠에 물들어버렸어….");
        Talk(916);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("이피아, 정화의 힘을 사용할 때야! 저 녀석을 '정화'해버려!");
        Talk(917);
        yield return StartCoroutine(CoWaitTalkEnd());

        MessageOpen("[Z]키로 공격할 수 있습니다.");
        CanMove(true);
        while (!GetKey(key.attack))
        {
            yield return null;
        }

        //Talk("후…주님, 한마리 더 보냅니다.");
        Talk(918);
        yield return wait3sec;
        MessageClose();
    }

    private IEnumerator CoPrac_Attack_Up()
    {
        CanMove(false);
        MessageClose();

        //Talk("우왓! 이피아, 위쪽을 봐!");
        Talk(919);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("저 거미도 이미 어둠에 물들어버린 것 같아.");
        Talk(920);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("어서 주님 곁으로 보내주자.");
        Talk(921);
        yield return StartCoroutine(CoWaitTalkEnd());

        CanMove(true);
        MessageOpen("[↑]키로 위를 조준할 수 있습니다. \n[Z]키를 함께 사용하여 위를 향해 공격할 수 있습니다.");

        while (!GetKey(key.lookUp))
        {
            yield return null;
        }

        //Talk("크큭…어떠냐, 정화의 힘이….");
        Talk(922);
        MessageClose();
    }

    private IEnumerator CoPrac_Parrying()
    {

        MessageClose();

        CanMove(false);

        //Talk("이건, 한 번 뛴다고 지나갈 수 있는 높이가 아니네….");
        Talk(923);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("저 무시무시한 가시공이 보이니? \n저걸 딛고 뛴다면 넘어갈 수 있을거야.");
        Talk(924);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("걱정마! 정화의 힘을 사용한다면\n상처없이 지나갈 수 있을거야. 어쩌구저쩌구!!");
        Talk(925);

        MessageOpen("점프 중, 적과 닿았을 때 [X]키를 사용하면 \n'패링'으로 연속 점프를 할 수 있습니다. ");
        CanMove(true);

        while (!player.State.isParrying)
        {
            yield return null;
        }

        //Talk("좋았어! 이대로 지나가보자.");
        Talk(926);
        MessageOpen("'패링'은 땅에 닿기 전까지\n몇 번이고 연속해서 사용할 수 있습니다. ");
    }

    private IEnumerator CoPrac_Attack_Power()
    {

        CanMove(false);
        MessageClose();
        //Talk("숲 속에서 사용한 정화의 힘은 나에게 다시 돌아오게 돼.");
        Talk(927);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("몰랐다구? 당연하지! 방금 급조한 설정이니까!!");
        Talk(928);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("반환된 힘은 차곡차곡 모아둘테니, \n이피아가 필요할 때 말하면 도움이 될만한 일을 해볼게.");
        Talk(929);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("시험 삼아서 한 번 해볼까?");
        Talk(930);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("싫다구? 싫어도 해야해. 숲 망하는 꼴 보고싶어?");
        Talk(931);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("우선, 앞의 통나무를 때려서 정화의 힘을 반환시켜봐.");
        Talk(932);
        yield return StartCoroutine(CoWaitTalkEnd());
        TalkEnd();

        MessageOpen("몬스터에게 공격을 적중시키거나 '패링'을 성공시키면 \n정화 게이지를 획득할 수 있습니다.");
        yield return StartCoroutine(CoWaitTalkEnd());

        MessageOpen("정화 게이지는 좌측 상단 UI에서 확인할 수 있습니다.\n 통나무를 때려서 정화 게이지를 획득해보세요.");
        CanMove(true);
        while (player.Stat.pixyEnerge < 9.8f)
        {
            yield return null;
        }

        CanMove(false, false);
        MessageOpen("게이지를 일정량 획득할 때마다 좌측 상단 UI에 꽃이 한 송이씩 피어납니다.");
        yield return StartCoroutine(CoWaitTalkEnd());
        MessageOpen("꽃을 한 송이 소모하여 \n루미에의 '강공격'을 사용할 수 있습니다.");
        yield return StartCoroutine(CoWaitTalkEnd());
        //Talk("좋았어. 뭔가 보여드리겠습니다.");
        Talk(933);
        MessageOpen("[C]키로 강공격을 사용하세요.");

        CanMove(true, false);

        while (!player.Com.pixy.isAttack)
        {
            yield return null;
        }

        MessageClose();
        //Talk("봤지? 난 그냥 졸졸 따라다니기만 하는 마스코트가 아니야!");
        Talk(934);
    }

    private IEnumerator CoPrac_Attack_Ult()
    {
        CanMove(false);
        //Talk("힘을 조금만 더 모으면,\n 방금 것보다 대단한 걸 할 수 있어.");
        Talk(935);
        yield return StartCoroutine(CoWaitTalkEnd());
        MessageOpen("정화 게이지를 끝까지 채워 꽃이 주황색으로 물들면,\n 루미에의 '궁극기'를 사용할 수 있습니다.");
        yield return StartCoroutine(CoWaitTalkEnd());
        CanMove(true);

        MessageOpen("정화 게이지를 끝까지 채워보세요.");
        while (player.Stat.pixyEnerge < 29.8f)
        {
            yield return null;
        }
        //Talk("준비는 끝났어! 말만 해!");
        Talk(936);
        MessageOpen("[V] 키로 궁극기를 사용하세요.");

        while (!player.Com.pixy.isUlt)
        {
            yield return null;
        }

        MessageOpen("궁극기를 사용하고 있을 때에는 정화 게이지를 획득할 수 없습니다.");

        //Talk("후, 봤냐? 이게 내 힘이다.\n이피아는 인간이라 이런거 못하지?");
        Talk(937);
    }
    private IEnumerator CoPrac_End()
    {
        CanMove(false);
        MessageClose();

        //Talk("연습은 이걸로 끝이야. 더 이상은 시간이 없어….");
        Talk(938);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("이피아, 이제는 한 번만 실수해도 모가지야 모가지.");
        Talk(939);
        yield return StartCoroutine(CoWaitTalkEnd());

        //Talk("잘 좀 하자? ^^");
        Talk(940);
        player.State.moveSystem = true;
        player.InputVal.movementInput = 1f;

        GameManager.instance.cameraManager.vcam.Follow = null;
        yield return wait3sec;
        GameManager.instance.StageClear();


    }
    private int currentTalkCode = 900;
    private void Talk(int _CODE)
    {
        UIManager.Instance.Talk(_CODE, talkTime);
    }
    private void Talk(string _str)
    {
        UIManager.Instance.Talk(currentTalkCode, talkTime);
        currentTalkCode += 1;
        //UIManager.Instance.Talk(_str, talkTime);
    }
    private void TalkInfinity(string _str)
    {
        UIManager.Instance.TalkInfinity(currentTalkCode);
        currentTalkCode += 1;
        //UIManager.Instance.TalkInfinity(_str);
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
    /// 대화를 스킵할 수 있는 키가 눌렸는가?
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
        StopCoroutine(currentCoName);
        Instance = null;
    }
}

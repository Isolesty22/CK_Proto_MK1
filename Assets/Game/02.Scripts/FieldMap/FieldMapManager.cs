using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using eState = StageSelector.eState;

public class FieldMapManager : MonoBehaviour
{
    public ScrollRect scrollRect;
    public FieldDoorSelector selector;
    public FieldDoor[] fieldDoors;
    public FieldDoor selectedDoor { get; private set; }

    private Dictionary<int, FieldDoor> fieldDoorDict = new Dictionary<int, FieldDoor>();

    private Action enterStageAction = null;

    private DataManager dataManager;

    private KeyOption keyOption;

    private bool canDetectKey = true;

    private int minStageNumber;

    private UIGameMessage gameMessage = null;

    private void Awake()
    {

    }
    private IEnumerator Start()
    {

        Init();

        //로딩이 끝날 때 까지 대기
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);
        CheckOpenDoor();
        //CheckOpenDoor();

    }
    private void Update()
    {
        if (canDetectKey)
        {
            DetectKey();
        }
    }

    public void Init()
    {
        keyOption = new KeyOption();
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
            keyOption = dataManager.currentData_settings.keySetting;
        }

        minStageNumber = fieldDoors[0].stageNumber;
        //딕셔너리에 추가
        Init_Dict();

        //스크롤에 필요한 값 체크
        for (int i = 0; i < fieldDoors.Length; i++)
        {
            fieldDoors[i].scrollPosition = GetDoorScrollPosition(fieldDoors[i]);
        }

        //열렸나 안열렸냐를 체크
        for (int i = 1; i <= fieldDoors.Length; i++)
        {
            if (fieldDoorDict[i].stageNumber <= dataManager.currentData_player.finalStageNumber + 1)
            {
                fieldDoorDict[i].mode = FieldDoor.eMode.Open;
            }
            fieldDoorDict[i].Init();
        }

        MoveSelector(dataManager.currentData_player.currentStageNumber);
    }

    private void Init_Dict()
    {
        fieldDoorDict = new Dictionary<int, FieldDoor>();
        for (int i = 0; i < fieldDoors.Length; i++)
        {
            fieldDoorDict.Add(fieldDoors[i].stageNumber, fieldDoors[i]);
        }
    }
    /// <summary>
    /// 문을 열어야하는지 검사하고, 문을 엽니다.
    /// </summary>
    public void CheckOpenDoor()
    {
        //현재 클리어한 스테이지 번호가 제일 큰 클리어 넘버보다 높을 떄
        if (dataManager.currentClearStageNumber > dataManager.currentData_player.finalStageNumber)
        {
            //제일 큰 클리어 넘버를 현재 클리어한 넘버로 변경
            dataManager.currentData_player.finalStageNumber = dataManager.currentClearStageNumber;
            dataManager.currentData_player.finalStageName = SceneNames.GetSceneNameUseStageNumber(dataManager.currentClearStageNumber);

            //저장
            StartCoroutine(dataManager.SaveCurrentData(DataName.player));

            //마지막 스테이지면 저장만하고 아무것도 안함...
            if (dataManager.currentClearStageNumber == 4)
            {  
                //4스테이지로 스크롤
                UpdateScrollPosition(4);
                Scroll();
            }
            else
            {
                //다음 스테이지로 스크롤
                UpdateScrollPosition(dataManager.currentClearStageNumber + 1);
                Scroll();

                //문 열기 연출
                OpenDoor(dataManager.currentClearStageNumber + 1);
            }
        }
        else
        {
            //현재 스테이지로 스크롤
            UpdateScrollPosition(dataManager.currentClearStageNumber);
            Scroll();
        }

    }

    public void OpenDoor(int _stageNumber)
    {
        FieldDoor door = null;
        if (!fieldDoorDict.TryGetValue(_stageNumber, out door))
        {
            Debug.Log("최대 스테이지입니다. 아마도...");
            return;
        }
        if (fieldDoorDict[_stageNumber].mode == FieldDoor.eMode.Lock)
        {
            fieldDoorDict[_stageNumber].Open();
        }
    }

    /// <summary>
    /// 해당 번호의 문을 선택합니다.
    /// </summary>
    public void SelectDoor(int _stageNumber)
    {
        selectedDoor = fieldDoorDict[_stageNumber];
        enterStageAction = selectedDoor.Button_EnterStage;
    }

    /// <summary>
    /// 셀렉터를 이동시킵니다.
    /// </summary>
    public void MoveSelector(eDiretion _dir)
    {
        bool moveSucceed = false;
        int moveStageNumber = -1;

        if (gameMessage == null)
        {
            gameMessage = UIManager.Instance.GetUI("UIGameMessage") as UIGameMessage;
        }

        if (_dir == eDiretion.Right)
        {
            moveStageNumber = selectedDoor.stageNumber + 1;

            if (moveStageNumber > 4)
            {
                gameMessage.Open("더 이상 이동할 수 없습니다.");
                return;
            }

            FieldDoor tempDoor = fieldDoorDict[moveStageNumber];
            //이동하려는 문이 잠겨있는 상태일때 
            if (tempDoor.mode == FieldDoor.eMode.Lock)
            {
                gameMessage.Open("아직은 이동할 수 없습니다.");
            }
            else
            {
                selector.MovePosition(tempDoor.selectTransform.position);
                SelectDoor(moveStageNumber);
                moveSucceed = true;
            }
        }
        else
        {
            moveStageNumber = selectedDoor.stageNumber - 1;

            if (moveStageNumber < minStageNumber)
            {
                gameMessage.Open("더 이상 이동할 수 없습니다.");
                return;
            }

            FieldDoor tempDoor = fieldDoorDict[moveStageNumber];

            //이동하려는 문이 잠겨있는 상태일때 
            if (tempDoor.mode == FieldDoor.eMode.Lock)
            {
                gameMessage.Open("이동할 수 없습니다.\n이전 스테이지를 클리어해주세요.");
            }
            else
            {
                selector.MovePosition(tempDoor.selectTransform.position);
                SelectDoor(moveStageNumber);
                moveSucceed = true;
            }
        }

        if (moveSucceed)
        {
            UpdateScrollPosition(selectedDoor.stageNumber);
            Scroll();
        }
    }


    private float scrollTimer = 0f;
    private float scrollProgress = 0f;
    private Vector2 endScrollPosition;
    private Vector2 startScrollPosition;
    private bool isScroll = false;
    private void UpdateScrollPosition(int _stageNumber)
    {
        if (_stageNumber == 0)
        {
            startScrollPosition = scrollRect.content.anchoredPosition;
            endScrollPosition = fieldDoorDict[1].scrollPosition;
        }
        else
        {
            startScrollPosition = scrollRect.content.anchoredPosition;
            endScrollPosition = fieldDoorDict[_stageNumber].scrollPosition;

        }

    }
    private Vector2 GetDoorScrollPosition(FieldDoor _door)
    {

        float xPos = 0 - (scrollRect.viewport.localPosition.x + _door.rectTransform.position.x);

        if (xPos > 0f)
        {
            xPos = 0f;
        }

        return new Vector2(xPos, 0f);
    }
    private void ClearScrollTimer() { scrollProgress = 0f; scrollTimer = 0f; }
    private void Scroll()
    {
        if (isScroll)
        {
            ClearScrollTimer();
        }
        else
        {
            StartCoroutine(CoScroll());
        }
    }
    private IEnumerator CoScroll()
    {
        isScroll = true;
        ClearScrollTimer();
        while (scrollProgress < 1f)
        {
            // Canvas.ForceUpdateCanvases();
            scrollTimer += Time.deltaTime;
            scrollProgress = scrollTimer / 0.5f;

            scrollRect.content.anchoredPosition = Vector2.Lerp(startScrollPosition, endScrollPosition, scrollProgress);
            yield return null;

        }
        isScroll = false;
    }
    public void MoveSelector(int _stageNumber)
    {
        FieldDoor door = null;
        //딕셔너리에 있다면
        if (fieldDoorDict.TryGetValue(_stageNumber, out door))
        {
            selector.MovePosition(door.selectTransform.position);
            SelectDoor(_stageNumber);
        }
        else
        {
            Debug.LogWarning("등록되지 않은 스테이지 넘버입니다 : " + _stageNumber);
        }


    }

    private void DetectKey()
    {
        //오른쪽 이동
        if (Input.GetKeyDown(keyOption.moveRight))
        {
            MoveSelector(eDiretion.Right);
        }
        //왼쪽 이동
        if (Input.GetKeyDown(keyOption.moveLeft))
        {
            MoveSelector(eDiretion.Left);
        }


        //스테이지 입장
        if (Input.GetKeyDown(keyOption.attack) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            //더이상 키 입력을 받지 않음
            canDetectKey = false;

            enterStageAction?.Invoke();
        }
    }

    /// <summary>
    /// [테스트용] 모든 스테이지를 해금합니다.
    /// </summary>
    public void Button_UnlockAllStage()
    {
        selectedDoor = fieldDoors[0];
        enterStageAction = selectedDoor.Button_EnterStage;
        selector.MovePosition(selectedDoor.selectTransform.position);
        for (int i = 0; i < fieldDoors.Length; i++)
        {
            fieldDoors[i].Open();
        }

    }
}


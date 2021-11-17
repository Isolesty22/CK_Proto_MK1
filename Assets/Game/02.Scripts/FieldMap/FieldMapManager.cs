using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using eState = StageSelector.eState;

public class FieldMapManager : MonoBehaviour
{

    public FieldDoorSelector selector;
    public FieldDoor[] fieldDoors;

    private Dictionary<int, FieldDoor> fieldDoorDict = new Dictionary<int, FieldDoor>();

    private Action enterStageAction = null;

    public FieldDoor selectedDoor { get; private set; }

    private DataManager dataManager;

    private KeyOption keyOption;

    private bool canDetectKey = true;

    private int minStageNumber;

    private UIGameMessage gameMessage = null;
    private IEnumerator Start()
    {

        Init();

        //로딩이 끝날 때 까지 대기
        SceneChanger.Instance.OnScenenLoadEnded += OnSceneLoadEnded;
        //yield return new WaitWhile(() => SceneChanger.Instance.isLoading);
        yield return null;

        gameMessage = UIManager.Instance.GetUI("UIGameMessage") as UIGameMessage;

        //CheckOpenDoor();

    }

    public void OnSceneLoadEnded()
    {
        SceneChanger.Instance.OnScenenLoadEnded -= CheckOpenDoor;
        CheckOpenDoor();
    }



    public void Init()
    {
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
            keyOption = dataManager.currentData_settings.keySetting;

        }
        else
        {
            Debug.LogWarning("데이터매니저가 존재하지 않아 플레이 데이터를 불러오지 못했습니다.");
            keyOption = new KeyOption();
        }

        minStageNumber = fieldDoors[0].stageNumber;

        //딕셔너리에 추가
        Init_Dict();

        for (int i = 0; i < fieldDoors.Length; i++)
        {
            if (fieldDoors[i].stageNumber <= dataManager.currentData_player.finalStageNumber + 1)
            {
                fieldDoors[i].mode = FieldDoor.eMode.Open;
            }
            fieldDoors[i].Init();
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
    /// 문을 열어야하는지 검사합니다.
    /// </summary>
    public void CheckOpenDoor()
    {

        Debug.Log("Check Open Door");
        //현재 클리어한 스테이지 번호가 저장된 스테이지 번호보다 높을 떄
        if (dataManager.currentClearStageNumber > dataManager.currentData_player.finalStageNumber)
        {
            //문 열기 연출
            OpenDoor(dataManager.currentClearStageNumber + 1);
        }
        else
        {
            //아무것도 안함
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
        if (fieldDoors[_stageNumber].mode == FieldDoor.eMode.Lock)
        {
            fieldDoorDict[_stageNumber].Open();
        }

        //스테이지 클리어 여부를 저장
        dataManager.currentData_player.finalStageNumber = dataManager.currentClearStageNumber;
        dataManager.currentData_player.finalStageName = SceneNames.GetSceneNameUseStageNumber(dataManager.currentClearStageNumber);

        dataManager.SaveCurrentData(DataName.player);
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
        int moveStageNumber = -1;

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
                gameMessage.Open("이동할 수 없습니다.\n이전 스테이지를 클리어해주세요.");
            }
            else
            {
                selector.MovePosition(tempDoor.selectTransform.position);
                SelectDoor(moveStageNumber);
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
            }
        }
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

    private void Update()
    {
        if (canDetectKey)
        {
            DetectKey();
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


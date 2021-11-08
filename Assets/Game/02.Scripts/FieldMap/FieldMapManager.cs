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

    [Tooltip("현재 선택된 문")]
    public FieldDoor selectedDoor;


    private DataManager dataManager;

    private KeyOption keyOption;




    private IEnumerator Start()
    {
        //로딩이 끝날 때 까지 대기
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);

        Init();
        CheckOpenDoor();

    }

    public void Init()
    {
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
        }


        //딕셔너리에 추가
        for (int i = 0; i < fieldDoors.Length; i++)
        {
            fieldDoorDict.Add(fieldDoors[i].stageNumber, fieldDoors[i]);
        }

        keyOption = dataManager.currentData_settings.keySetting;
        MoveSelector(dataManager.currentData_player.currentStageNumber);
    }

    /// <summary>
    /// 문을 열어야하는지 검사합니다.
    /// </summary>
    public void CheckOpenDoor()
    {
        //현재 클리어한 스테이지 번호가 저장된 스테이지 번호보다 높을 떄
        if (dataManager.currentClearStageNumber > dataManager.currentData_player.finalStageNumber)
        {
            //문 열기 연출
            OpenDoor(dataManager.currentClearStageNumber);
        }
        else
        {
            //아무것도 안함
        }

    }

    public void OpenDoor(int _stageNumber)
    {
        fieldDoorDict[_stageNumber].Open();

        //스테이지 클리어 여부를 저장
        dataManager.currentData_player.finalStageNumber = _stageNumber;
        dataManager.currentData_player.finalStageName = SceneNames.GetSceneNameUseStageNumber(_stageNumber);

        dataManager.SaveCurrentData(DataManager.fileName_player);
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
        int minStageNumber = 1;
        int moveStageNumber = -1;
        if (selectedDoor.stageNumber + 1 > fieldDoors.Length || selectedDoor.stageNumber - 1 < minStageNumber)
        {
            Debug.LogWarning("스테이지의 끝에 도달했습니다. 이동할 수 없습니다.");
        }

        if (_dir == eDiretion.Right)
        {
            moveStageNumber = selectedDoor.stageNumber + 1;
            FieldDoor tempDoor = fieldDoorDict[moveStageNumber];
            //이동하려는 문이 잠겨있는 상태일때 
            if (tempDoor.mode == FieldDoor.eMode.Lock)
            {
                Debug.LogWarning("잠겨있어서 이동할 수 없습니다.");
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
            FieldDoor tempDoor = fieldDoorDict[moveStageNumber];

            //이동하려는 문이 잠겨있는 상태일때 
            if (tempDoor.mode == FieldDoor.eMode.Lock)
            {
                Debug.LogWarning("잠겨있어서 이동할 수 없습니다.");
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
            Debug.LogWarning("등록되지 않은 스테이지 넘버입니다.");
        }
    }

    private void Update()
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
            enterStageAction?.Invoke();
        }
    }

}


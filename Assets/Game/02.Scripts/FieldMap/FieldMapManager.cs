using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eState = StageSelector.eState;

public class FieldMapManager : MonoBehaviour
{

    [Header("이피아")]
    public StageSelector stageSelector;

    private DataManager dataManager;

    [Header("현재 스테이지 번호")]
    public int currentStageNumber;

    [Header("발판 배열"), Tooltip("StageSelector는 해당 발판들의 위치로 이동합니다.")]
    public StagePlate[] stagePlates;

    public KeyOption keyOption;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //데이터매니저에서 스테이지 데이터 가져오기
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
            currentStageNumber = dataManager.currentData_player.currentStageNumber;
            keyOption = dataManager.currentData_settings.keySetting;
        }
        else
        {
            //못가져오면 0
            Debug.LogError("DataManager Instance가 null입니다. currentStageNumber Set 0");
            currentStageNumber = 0;
            keyOption = new KeyOption();
        }


        //이피아 시작위치 설정
        stageSelector.SetPosition(stagePlates[currentStageNumber].GetPosition());
        // ipiaTransform.position = stageList[currentStageNumber].stageTransform.position;
        //StageGrayScale_Legacy();
        //StartCoroutine(ProcessInputMoveKey());
    }
   

    public void DetectMoveKey()
    {

    }
    public void MoveStage(int stageNumber)
    {
        //이미 움직이고 있는 상태라면
        if (stageSelector.state == eState.Move)
        {
            stageSelector.ChangeDestinationPos(stagePlates[stageNumber].GetPosition());
        }
        else
        {
            stageSelector.StartProcessMove();
        }
    }
}

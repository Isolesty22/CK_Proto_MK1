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

    [SerializeField]
    private int maxStageNumber;

    [SerializeField]
    [Tooltip("이동해야할 스테이지 번호")]
    private int moveStageNumber;

    public int prevStageNumber;

    [Header("발판 배열"), Tooltip("StageSelector는 해당 발판들의 위치로 이동합니다.")]
    public StagePlate[] stagePlates;

    public KeyOption keyOption;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DetectMoveKey();
    }
    public void Init()
    {
        //데이터매니저에서 스테이지 데이터 가져오기
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
            currentStageNumber = dataManager.currentData_player.currentStageNumber;
            maxStageNumber = dataManager.currentData_player.finalStageNumber + 2;
            prevStageNumber = currentStageNumber - 1;
            keyOption = dataManager.currentData_settings.keySetting;
        }
        else
        {
            //못가져오면 0
            Debug.LogError("DataManager Instance가 null입니다. currentStageNumber Set 0");
            currentStageNumber = 0;
            maxStageNumber = currentStageNumber + 5;
            prevStageNumber = currentStageNumber - 1;
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
        //뒤로가기
        if (Input.GetKeyDown(keyOption.moveLeft))
        {

            //움직이고 있을 때
            if (stageSelector.state == eState.Move && prevStageNumber == currentStageNumber + 1)
            {
                return;
            }

            if (currentStageNumber - 1 >= 0)
            {
                prevStageNumber = currentStageNumber;
                moveStageNumber = currentStageNumber - 1;
                MoveStage(moveStageNumber);
                return;
            }
            Debug.LogWarning("더 이상 뒤로 갈 수 없음.");
        }
        //앞으로 가기
        if (Input.GetKeyDown(keyOption.moveRight))
        {

            //이전 스테이지가 지금 스테이지보다 
            if (stageSelector.state == eState.Move && prevStageNumber == currentStageNumber - 1)
            {
                return;
            }


            //최종 클리어 스테이지보다 한 칸 더 갈 수 있어야함
            if (currentStageNumber + 1 < maxStageNumber)
            {
                prevStageNumber = currentStageNumber;
                moveStageNumber = currentStageNumber + 1;
                MoveStage(moveStageNumber);
                return;
            }
            Debug.LogWarning("더 이상 앞으로 갈 수 없음.");
        }
    }
    public void MoveStage(int stageNumber)
    {
        //이미 움직이고 있는 상태라면
        if (stageSelector.state == eState.Move)
        {
            stageSelector.ChangeDestinationPos(stagePlates[stageNumber].GetPosition());
            currentStageNumber = moveStageNumber;
        }
        else
        {
            Debug.Log("Move Stage!");
            stageSelector.SetStartPos(stageSelector.Com.rigidBody.position);
            stageSelector.SetDestinationPos(stagePlates[stageNumber].GetPosition());
            Debug.Log(stagePlates[stageNumber].GetPosition());
            stageSelector.StartProcessMove();
            currentStageNumber = moveStageNumber;
        }
    }
}


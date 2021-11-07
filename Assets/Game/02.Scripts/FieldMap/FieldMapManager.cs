using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eState = StageSelector.eState;

public class FieldMapManager : MonoBehaviour
{

    public FieldDoor[] fieldDoors;

    [Tooltip("현재 선택된 문")]
    public FieldDoor selectedDoor;

    #region Legacy(3D Field)
    [Header("이피아")]
    public StageSelector stageSelector;


    [Header("현재 스테이지 번호")]
    public int currentStageNumber;


    public int prevStageNumber;

    [Header("발판 배열"), Tooltip("StageSelector는 해당 발판들의 위치로 이동합니다.")]
    public StagePlate[] stagePlates;

    public KeyOption keyOption;


    [Tooltip("스테이지 02까지를 잇는 길")]
    public GameObject Stage02Road;
    private DataManager dataManager;

    private int maxStageNumber;

    [Tooltip("이동해야할 스테이지 번호")]
    private int moveStageNumber;

    private bool isEnter;
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DetectEnterKey();
        DetectMoveKey();
    }
    public void Init()
    {
        //데이터매니저에서 스테이지 데이터 가져오기
        if (DataManager.Instance != null)
        {
            dataManager = DataManager.Instance;
            currentStageNumber = dataManager.currentData_player.currentStageNumber;

            //최종 클리어한 스테이지보다 한칸 더 갈 수 있어야함
            maxStageNumber = dataManager.currentData_player.finalStageNumber + 1;
            prevStageNumber = currentStageNumber - 1;
            keyOption = dataManager.currentData_settings.keySetting;
        }
        else
        {
            //못가져오면 0
            Debug.LogError("DataManager Instance가 null입니다. currentStageNumber Set 0");
            currentStageNumber = 0;
            maxStageNumber = 2;
            prevStageNumber = currentStageNumber - 1;
            keyOption = new KeyOption();
        }

        //maxStageNumber = 1;

        //입장한 적 없음
        isEnter = false;

        //이피아 시작위치 설정
        stageSelector.SetPosition(stagePlates[currentStageNumber].GetPosition());
        // ipiaTransform.position = stageList[currentStageNumber].stageTransform.position;
        //StageGrayScale_Legacy();
        //StartCoroutine(ProcessInputMoveKey());

        if (maxStageNumber > 1)
        {
            Stage02Road.SetActive(true);
        }
        else
        {
            Stage02Road.SetActive(false);
        }
    }

    /// <summary>
    /// 움직임에 필요한 키 입력을 감지
    /// </summary>
    public void DetectMoveKey()
    {
        //뒤로가기
        if (Input.GetKeyDown(keyOption.moveLeft))
        {

            //움직이고 있을 때 두 칸 이상 못 움직임
            if (stageSelector.state == eState.Move && prevStageNumber == currentStageNumber + 1)
            {
                return;
            }

            if (currentStageNumber - 1 >= 0 && currentStageNumber < stagePlates.Length)
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

            //움직이고 있을 때 두 칸 이상 못 움직임
            if (stageSelector.state == eState.Move && prevStageNumber == currentStageNumber - 1)
            {
                return;
            }

            //최종 클리어 스테이지보다 한 칸 더 갈 수 있어야함
            if (currentStageNumber + 1 < maxStageNumber && currentStageNumber < stagePlates.Length)
            {
                prevStageNumber = currentStageNumber;
                moveStageNumber = currentStageNumber + 1;
                MoveStage(moveStageNumber);
                return;
            }
            Debug.LogWarning("더 이상 앞으로 갈 수 없음.");
        }
    }


    /// <summary>
    /// 입장에 필요한 키 입력을 감지
    /// </summary>
    public void DetectEnterKey()
    {
        if (Input.GetKeyDown(keyOption.attack) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isEnter || stageSelector.state == eState.Move)
            {
                return;
            }

            isEnter = true;

            DataManager.Instance.currentData_player.currentStageNumber = currentStageNumber;
            DataManager.Instance.SaveCurrentData(DataManager.fileName_player);

            //원래는 이름이 아니라 currentStageNumber로 이동해야하지만, 임시로...
            SceneChanger.Instance.LoadThisScene(stagePlates[currentStageNumber].stageName);
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

    #endregion
}


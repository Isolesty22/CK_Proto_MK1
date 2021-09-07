using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 필드맵 UI
/// </summary>
public class UIFieldMap : MonoBehaviour
{
    public Transform ipiaTransform;

    [SerializeField]
    private int currentStageNumber;

    [SerializeField, Tooltip("이동키를 입력할 수 있는 상태인가?")]
    private bool canInputKey = true;

    public List<FieldMapStage> stageList = new List<FieldMapStage>();



    private void Start()
    {
        if (DataManager.Instance != null)
        {
            currentStageNumber = DataManager.Instance.currentData_player.currentStageNumber;
        }
        else
        {
            Debug.LogError("DataManager Instance가 null입니다. 또 뭔가 깜빡했나보네요...");
            currentStageNumber = 0;
        }

        //이피아 위치 설정
        ipiaTransform.position = stageList[currentStageNumber].stageTransform.position;
        StageGrayScale_Legacy();
        StartCoroutine(ProcessInputMoveKey());
    }


    private IEnumerator ProcessInputMoveKey()
    {
        while (true)
        {
            if (!canInputKey)
            {
                yield return null;
                continue;
            }

            //엔터키
            if (Input.GetKeyDown(KeyCode.Return))
            {
                canInputKey = false;
                SceneChanger.Instance.LoadThisScene(GetSceneNameUseStageNumber(currentStageNumber));
                yield break;
            }

            //왼쪽
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                yield return TryMoveCharacter("Left");

            }

            //오른쪽
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                yield return TryMoveCharacter("Right");
            }

            yield return null;
        }
    }

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float moveSpeed = 2f;

    private DataManager dataManager = null;
    private IEnumerator TryMoveCharacter(string _moveDir)
    {

        int moveDir = 0;

        //이동 방향 정하기
        switch (_moveDir)
        {
            case "Right":
                moveDir = 1;
                break;

            case "Left":
                moveDir = -1;
                break;

            default:
                break;
        }

        dataManager = DataManager.Instance;

        int moveNumber = moveDir + currentStageNumber;

        //해당 넘버의 스테이지로 이동할 수 없다면
        if (!CanMoveThisNumber(moveNumber))
        {
            //부들부들 떨리는 효과
            originalPosition = ipiaTransform.position;

            ipiaTransform.position = new Vector2(ipiaTransform.position.x, ipiaTransform.position.y + 0.1f);
            yield return YieldInstructionCache.WaitForFixedUpdate;

            ipiaTransform.position = originalPosition;
            yield return YieldInstructionCache.WaitForFixedUpdate;

            ipiaTransform.position = new Vector2(ipiaTransform.position.x, ipiaTransform.position.y - 0.1f);
            yield return YieldInstructionCache.WaitForFixedUpdate;


            //원래 위치로 설정
            ipiaTransform.position = originalPosition;
            yield break;
        }


        //지나갈 수 있다면...지나가야지!!

        //이동키 입력 방지
        canInputKey = false;

        //현재 위치 설정
        originalPosition = ipiaTransform.position;

        //목표 위치 설정 
        targetPosition = stageList[moveNumber].stageTransform.position;

        float timer = 0f;
        // float distance = 25252;
        float progress = 0f;
        //일정 거리 안에 들어올 때 까지
        while (progress < 1f)
        {
            timer += Time.smoothDeltaTime;
            progress = timer / moveSpeed;
            ipiaTransform.position = Vector3.Lerp(originalPosition, targetPosition, progress);//Vector2.MoveTowards(originalPosition, targetPosition, moveSpeed);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //혹시 모르니까 정확하게 딱! 해주기
        ipiaTransform.position = targetPosition;

        //현재 스테이지 넘버를 움직인 스테이지 넘버로 변경
        currentStageNumber = moveNumber;

        //이동키 입력 가능
        canInputKey = true;
        yield break;
    }


    /// <summary>
    /// 해당 번호의 스테이지로 이동할 수 있는가?
    /// 제일 마지막에 클리어한 스테이지의 다음 스테이지까지 이동이 가능합니다.
    /// </summary>
    private bool CanMoveThisNumber(int _moveNumber)
    {
        //0보다 작거나, 클리어하지 못한 스테이지거나, 전체 스테이지 수보다 크면
        if (_moveNumber < 0
            || _moveNumber > dataManager.currentData_player.finalStageNumber + 1
            || _moveNumber > stageList.Count - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    /// <summary>
    /// 가지 못하는 스테이지의 색상을 변경합니다.
    /// </summary>
    private void StageGrayScale_Legacy()
    {
        //마지막으로 클리어한 스테이지의 다음 스테이지까지는 갈 수 있어야함
        for (int i = DataManager.Instance.currentData_player.finalStageNumber + 2; i < stageList.Count; i++)
        //for (int i = 0 + 2; i < stageList.Count; i++)
        {
            stageList[i].SetStageGrayScale(1f);
        }
    }

    /// <summary>
    /// 번호를 주면 그에 맞는 씬 이름을 반환합니다.
    /// </summary>
    private string GetSceneNameUseStageNumber(int _number)
    {
        string str = "Stage_";
        switch (_number)
        {
            case 0:
                return str + "00";
            case 1:
                return str + "01";
            case 2:
                return str + "02";
            case 3:
                return str + "03";
            case 4:
                return str + "04";
            default:
                return str + "00";
        }
    }
}

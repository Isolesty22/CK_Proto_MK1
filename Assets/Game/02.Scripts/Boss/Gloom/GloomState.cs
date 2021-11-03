using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomState : BossState
{
    protected GloomController gloom;
}

public class GloomState_Idle : GloomState
{
    public GloomState_Idle(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = true;
    }
}
public class GloomState_Chase : GloomState
{
    public GloomState_Chase(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}
public class GloomState_Leap : GloomState
{

    #region definition
    private class Position
    {
        public Vector3 start;
        public Vector3 end;

        public Vector3 startTop;
        public Vector3 endTop;
    }
    private class Rotation
    {
        public Quaternion start;
        public Quaternion end;
    }
    #endregion

    private eDiretion endDirection;

    private Position pos = new Position();
    private Rotation rot = new Rotation();

    private GloomController.SkillValues.LeapPattern leapValue;
    public GloomState_Leap(GloomController _gloomController)
    {
        gloom = _gloomController;
        leapValue = gloom.SkillVal.leapPattern;
    }
    public override void OnEnter()
    {
        canExit = false;


        //현재 위치가 오른쪽이면
        if (gloom.diretion == eDiretion.Right)
        {
            //오른쪽에서 왼쪽으로 이동하게 설정
            pos.start = gloom.Com.gloomMap.gloomPos_Right.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Left.position;

            rot.start = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, -90, 0f));

            gloom.ChangeDirection(eDiretion.Left);
            endDirection = eDiretion.Left;

        }
        //왼쪽이면
        else
        {
            //왼쪽에서 오른쪽으로 이동하게 설정
            pos.start = gloom.Com.gloomMap.gloomPos_Left.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Right.position;

            rot.start = Quaternion.Euler(new Vector3(0f, -90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            gloom.ChangeDirection(eDiretion.Right);
            endDirection = eDiretion.Right;
        }

        //상승 위치 설정
        pos.startTop = new Vector3(pos.start.x, pos.start.y + leapValue.upPosValue, pos.start.z);
        pos.endTop = new Vector3(pos.end.x, pos.end.y + leapValue.upPosValue, pos.end.z);

        gloom.SetTrigger("Leap_Start");
        gloom.SetAnimEvent(AnimEvent_Jump);
    }

    public void AnimEvent_Jump()
    {
        gloom.StartCoroutine(ProcessAnimEvent_Jump());

    }
    //public void AnimEvent_Fall()
    //{
    //    gloom.StartCoroutine(ProcessAnimEvent_Fall());
    //}
    private IEnumerator ProcessAnimEvent_Jump()
    {
        float timer = 0f;
        float progress = 0f;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / leapValue.upTime;

            //gloom.myTransform.SetPositionAndRotation(Vector3.Lerp(pos.start, pos.startTop, progress),
            //    Quaternion.Lerp(rot.start, rot.end, progress));

            gloom.myTransform.position = Vector3.Lerp(pos.start, pos.startTop, progress);
            //gloom.myTransform.rotation = Quaternion.Euler((Vector3.Lerp(rot.start, rot.end, progress)));
            yield return null;
        }

        //gloom.myTransform.SetPositionAndRotation(pos.startTop, rot.start);

        gloom.myTransform.position = pos.startTop;
        //점프가 끝나면 착지 이벤트 실행
        //gloom.SetAnimEvent(AnimEvent_Fall);
        gloom.StartCoroutine(ProcessAnimEvent_Fall());
    }
    private IEnumerator ProcessAnimEvent_Fall()
    {
        float timer = 0f;
        float progress = 0f;

        gloom.SetTrigger("Leap_End");
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / leapValue.downTime;

            gloom.myTransform.SetPositionAndRotation(Vector3.Lerp(pos.endTop, pos.end, progress),
                Quaternion.Lerp(rot.start, rot.end, progress));

            //gloom.myTransform.position = Vector3.Lerp(pos.start, pos.startTop, progress);
            //gloom.myTransform.rotation = Quaternion.Euler((Vector3.Lerp(rot.start, rot.end, progress)));
            yield return null;
        }

        gloom.myTransform.SetPositionAndRotation(pos.end, rot.end);

        if (endDirection == eDiretion.Right)
        {
            if (gloom.ContainsThornVineDict(6))
            {
                gloom.SkillObj.aliveThornVineDict[6].StartDie();
            }
        }
        else
        {
            if (gloom.ContainsThornVineDict(0))
            {
                gloom.SkillObj.aliveThornVineDict[0].StartDie();
            }
        }

        canExit = true;
        yield break;
    }
}
public class GloomState_Threat : GloomState
{
    public GloomState_Threat(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;

        gloom.SetTrigger("Threat_Start");
    }
}
public class GloomState_ThornPath : GloomState
{
    private eDiretion diretion;
    private int[] blockArr;
    public GloomState_ThornPath(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;

        //현재 보스의 방향을 가져옴
        diretion = gloom.diretion;

        //사용할 수 있는 블록 리스트를 가져옴
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.ExcludeVine).ToArray();

        //블록 인덱스를 랜덤하게 섞음
        ShuffleArray();
        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornPath_Start");
    }

    public void AnimEvent()
    {
        //가져온게 없으면
        if (blockArr.Length == 0)
        {
            //아무것도 안함
            return;
        }
        gloom.StartCoroutine(ProcessAnimEvent());
    }
    private IEnumerator ProcessAnimEvent()
    {
        //2개 이상 있다면 for문으로 돌리기 
        if (blockArr.Length > 1)
        {
            for (int i = 0; i < 2; i++)
            {

                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //풀에서 꺼냄
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

                //초기화
                thornVine.gloom = gloom;
                thornVine.Init();
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
        //1개 밖에 없다면 그냥 지정해서 소환
        else
        {
            MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[0]];

            //풀에서 꺼냄
            GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

            //초기화
            thornVine.gloom = gloom;
            thornVine.Init();
            thornVine.SetValues(block, blockArr[0], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
            thornVine.UpdateEndPosition();

            thornVine.StartGrow();
            yield return null;
        }


    }

    private void ShuffleArray()
    {
        //남은 칸이 두 칸 이하라서 섞을 필요가 없으면 아무것도 안함
        if (blockArr.Length <= 2)
        {
            return;
        }

        int length = blockArr.Length;

        for (int i = 0; i < length; i++)
        {
            int randIndex = Random.Range(i, length);

            int tempPos = blockArr[i];
            blockArr[i] = blockArr[randIndex];
            blockArr[randIndex] = tempPos;
        }
    }
}
public class GloomState_Obstruct : GloomState
{
    /// <summary>
    /// 사용 가능한 인덱스들이 담긴 리스트
    /// </summary>
    private List<int> usableIndex;

    /// <summary>
    /// 이미 사용한 인덱스들이 담긴 리스트
    /// </summary>
    private List<int> usedIndex;

    private int currentIndex;
    public GloomState_Obstruct(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
        currentIndex = -1;
        usableIndex = new List<int> { 0, 1, 2 };
        usedIndex = new List<int>();
        gloom.SetTrigger("Obstruct_Start");
        gloom.StartCoroutine(ProcessSkill());
    }

    private IEnumerator ProcessSkill()
    {
        yield return null;

        for (int i = 0; i < 9; i++)
        {
            Debug.Log("방해 인덱스 : " + GetUsablePositionIndex());
            yield return null;
        }
    }

    /// <summary>
    /// 사용 가능한 인덱스를 반환하고, 해당 인덱스를 '이미 사용한 인덱스 리스트'에 넣습니다.
    /// </summary>
    /// <returns></returns>
    private int GetUsablePositionIndex()
    {
        int currentPosIndex = -1;
        if (usableIndex.Count > 1)
        {
            //랜덤한 인덱스를 가져옴
            currentIndex = Random.Range(0, usableIndex.Count);

            //usableIndex 속의 값을 PosIndex에 넣음
            currentPosIndex = usableIndex[currentIndex];

            usedIndex.Add(currentPosIndex);
            usableIndex.RemoveAt(currentIndex);
        }
        else // 사용할 수 있는 인덱스가 하나밖에 없으면
        {

            //일단 남은 하나를 할당시켜줌 
            currentPosIndex = usableIndex[0];
            currentIndex = 0;

            //사용한 인덱스에 추가
            usedIndex.Add(currentPosIndex);

            //사용 불가능하게 설정
            usableIndex.RemoveAt(0);

            //usedIndex에 먼저 담겨있던 두 개의 인덱스를 사용 가능하게 변경
            usableIndex.Add(usedIndex[0]);
            usableIndex.Add(usedIndex[1]);

            //usedIndex에 먼저 담겨있던 두 개의 인덱스를 삭제
            usedIndex.RemoveRange(0, 2);
        }

        return currentPosIndex;
    }

}
public class GloomState_ThornForest : GloomState
{
    private eDiretion diretion;
    private int[] blockArr;
    public GloomState_ThornForest(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;

        //현재 보스의 방향을 가져옴
        diretion = gloom.diretion;

        //사용할 수 있는 블록 리스트를 가져옴
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.Default).ToArray();

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornForest_Start");
    }


    public void AnimEvent()
    {
        gloom.StartCoroutine(ProcessAnimEvent());
    }
    private IEnumerator ProcessAnimEvent()
    {
        int length;
        //보스가 오른쪽에 있으면
        if (diretion == eDiretion.Right)
        {
            length = blockArr.Length;
            for (int i = 0; i < 3; i++)
            {
                // MapBlock block = gloom.Com.gloomMap.mapBlocks[i];
                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //사용 중이라면 아무것도 안함
                if (block.currentType == MapBlock.eType.Used)
                {
                    continue;
                }


                //사용 중이 아니라면

                //풀에서 꺼냄
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();
                //초기화
                thornVine.gloom = gloom;
                thornVine.Init();
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
        //왼쪽일때
        else
        {
            length = blockArr.Length - 1;
            for (int i = length; i > length - 3; i--)
            {
                // MapBlock block = gloom.Com.gloomMap.mapBlocks[i];
                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //사용 중이라면 아무것도 안함
                if (block.currentType == MapBlock.eType.Used)
                {
                    continue;
                }

                //사용 중이 아니라면

                //풀에서 꺼냄
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

                //초기화
                thornVine.gloom = gloom;
                thornVine.Init();

                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
    }
}
public class GloomState_Summon : GloomState
{
    public GloomState_Summon(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}
public class GloomState_Berserk : GloomState
{
    public GloomState_Berserk(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}
public class GloomState_Die : GloomState
{
    public GloomState_Die(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
    }
}


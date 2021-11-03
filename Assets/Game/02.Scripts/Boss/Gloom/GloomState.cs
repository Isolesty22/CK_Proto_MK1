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


        //���� ��ġ�� �������̸�
        if (gloom.diretion == eDiretion.Right)
        {
            //�����ʿ��� �������� �̵��ϰ� ����
            pos.start = gloom.Com.gloomMap.gloomPos_Right.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Left.position;

            rot.start = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, -90, 0f));

            gloom.ChangeDirection(eDiretion.Left);
            endDirection = eDiretion.Left;

        }
        //�����̸�
        else
        {
            //���ʿ��� ���������� �̵��ϰ� ����
            pos.start = gloom.Com.gloomMap.gloomPos_Left.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Right.position;

            rot.start = Quaternion.Euler(new Vector3(0f, -90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            gloom.ChangeDirection(eDiretion.Right);
            endDirection = eDiretion.Right;
        }

        //��� ��ġ ����
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
        //������ ������ ���� �̺�Ʈ ����
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

        //���� ������ ������ ������
        diretion = gloom.diretion;

        //����� �� �ִ� ��� ����Ʈ�� ������
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.ExcludeVine).ToArray();

        //��� �ε����� �����ϰ� ����
        ShuffleArray();
        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornPath_Start");
    }

    public void AnimEvent()
    {
        //�����°� ������
        if (blockArr.Length == 0)
        {
            //�ƹ��͵� ����
            return;
        }
        gloom.StartCoroutine(ProcessAnimEvent());
    }
    private IEnumerator ProcessAnimEvent()
    {
        //2�� �̻� �ִٸ� for������ ������ 
        if (blockArr.Length > 1)
        {
            for (int i = 0; i < 2; i++)
            {

                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //Ǯ���� ����
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

                //�ʱ�ȭ
                thornVine.gloom = gloom;
                thornVine.Init();
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
        //1�� �ۿ� ���ٸ� �׳� �����ؼ� ��ȯ
        else
        {
            MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[0]];

            //Ǯ���� ����
            GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

            //�ʱ�ȭ
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
        //���� ĭ�� �� ĭ ���϶� ���� �ʿ䰡 ������ �ƹ��͵� ����
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
    /// ��� ������ �ε������� ��� ����Ʈ
    /// </summary>
    private List<int> usableIndex;

    /// <summary>
    /// �̹� ����� �ε������� ��� ����Ʈ
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
            Debug.Log("���� �ε��� : " + GetUsablePositionIndex());
            yield return null;
        }
    }

    /// <summary>
    /// ��� ������ �ε����� ��ȯ�ϰ�, �ش� �ε����� '�̹� ����� �ε��� ����Ʈ'�� �ֽ��ϴ�.
    /// </summary>
    /// <returns></returns>
    private int GetUsablePositionIndex()
    {
        int currentPosIndex = -1;
        if (usableIndex.Count > 1)
        {
            //������ �ε����� ������
            currentIndex = Random.Range(0, usableIndex.Count);

            //usableIndex ���� ���� PosIndex�� ����
            currentPosIndex = usableIndex[currentIndex];

            usedIndex.Add(currentPosIndex);
            usableIndex.RemoveAt(currentIndex);
        }
        else // ����� �� �ִ� �ε����� �ϳ��ۿ� ������
        {

            //�ϴ� ���� �ϳ��� �Ҵ������ 
            currentPosIndex = usableIndex[0];
            currentIndex = 0;

            //����� �ε����� �߰�
            usedIndex.Add(currentPosIndex);

            //��� �Ұ����ϰ� ����
            usableIndex.RemoveAt(0);

            //usedIndex�� ���� ����ִ� �� ���� �ε����� ��� �����ϰ� ����
            usableIndex.Add(usedIndex[0]);
            usableIndex.Add(usedIndex[1]);

            //usedIndex�� ���� ����ִ� �� ���� �ε����� ����
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

        //���� ������ ������ ������
        diretion = gloom.diretion;

        //����� �� �ִ� ��� ����Ʈ�� ������
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
        //������ �����ʿ� ������
        if (diretion == eDiretion.Right)
        {
            length = blockArr.Length;
            for (int i = 0; i < 3; i++)
            {
                // MapBlock block = gloom.Com.gloomMap.mapBlocks[i];
                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //��� ���̶�� �ƹ��͵� ����
                if (block.currentType == MapBlock.eType.Used)
                {
                    continue;
                }


                //��� ���� �ƴ϶��

                //Ǯ���� ����
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();
                //�ʱ�ȭ
                thornVine.gloom = gloom;
                thornVine.Init();
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thornPattern.hp, gloom.SkillVal.thornPattern.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
        //�����϶�
        else
        {
            length = blockArr.Length - 1;
            for (int i = length; i > length - 3; i--)
            {
                // MapBlock block = gloom.Com.gloomMap.mapBlocks[i];
                MapBlock block = gloom.Com.gloomMap.mapBlocks[blockArr[i]];

                //��� ���̶�� �ƹ��͵� ����
                if (block.currentType == MapBlock.eType.Used)
                {
                    continue;
                }

                //��� ���� �ƴ϶��

                //Ǯ���� ����
                GloomThornVine thornVine = gloom.Pool.thornVine.SpawnThis();

                //�ʱ�ȭ
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


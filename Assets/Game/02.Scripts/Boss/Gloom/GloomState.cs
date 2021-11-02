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
    public GloomState_Leap(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;
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
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.Default).ToArray();

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornPath_Start");
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
                thornVine.Init();
                thornVine.SetValues(block, gloom.SkillVal.thornForest.hp, gloom.SkillVal.thornForest.waitTime, block.position.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
                yield return null;
            }
        }
        //�����϶�
        else
        {
            length = blockArr.Length-1;
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
                thornVine.Init();
                thornVine.SetValues(block, gloom.SkillVal.thornForest.hp, gloom.SkillVal.thornForest.waitTime, block.position.groundCenter);
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


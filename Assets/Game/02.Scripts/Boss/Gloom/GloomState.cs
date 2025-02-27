﻿using System.Collections;
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
    private int count;
    private WaitForSeconds waitSec;

    private Vector3 startPos;
    private Rigidbody playerRB;
    public GloomState_Chase(GloomController _gloomController)
    {
        gloom = _gloomController;
        count = gloom.SkillVal.chase.count;
        waitSec = new WaitForSeconds(gloom.SkillVal.chase.intevar);
        playerRB = GameManager.instance.playerController.Com.rigidbody;
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoSkill();
        gloom.TalkOnce(408);
        //UIManager.Instance.Talk("이피아! 도망쳐!");

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("Chase_Start");
    }

    public void AnimEvent()
    {
        gloom.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoSkill()
    {

        // startPos = gloom.SkillObj.chaseTransform.position,playerRB.position
        for (int i = 0; i < count; i++)
        {
            GloomChaseBullet bullet = gloom.Pool.chaseBullet.SpawnThis(gloom.SkillObj.sphereTransform.position);

            //사운드 출력
            gloom.audioSource.PlayOneShot(gloom.audioClips.chase);

            bullet.Init(gloom);
            bullet.SetPosition(gloom.SkillObj.sphereTransform.position, playerRB.position);
            bullet.Move();

            yield return waitSec;
        }
        gloom.SetTrigger("Chase_End");
        //canExit = true;
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

    private eDirection endDirection;

    private Position pos = new Position();
    private Rotation rot = new Rotation();
    private WaitForSeconds downAnimTime = null;
    private float fDownAnimTime = 0;

    private GloomController.SkillValues.LeapPattern leapValue;
    public GloomState_Leap(GloomController _gloomController)
    {
        gloom = _gloomController;
        leapValue = gloom.SkillVal.leap;
        downAnimTime = new WaitForSeconds(leapValue.downTime - leapValue.downAnimTime);
        fDownAnimTime = leapValue.downTime - leapValue.downAnimTime;
    }
    public override void OnEnter()
    {
        canExit = false;


        //현재 위치가 오른쪽이면
        if (gloom.diretion == eDirection.Right)
        {
            //오른쪽에서 왼쪽으로 이동하게 설정
            pos.start = gloom.Com.gloomMap.gloomPos_Right.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Left.position;

            rot.start = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, -90, 0f));

            endDirection = eDirection.Left;

        }
        //왼쪽이면
        else
        {
            //왼쪽에서 오른쪽으로 이동하게 설정
            pos.start = gloom.Com.gloomMap.gloomPos_Left.position;
            pos.end = gloom.Com.gloomMap.gloomPos_Right.position;

            rot.start = Quaternion.Euler(new Vector3(0f, -90f, 0f));
            rot.end = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            endDirection = eDirection.Right;
        }

        //상승 위치 설정
        pos.startTop = new Vector3(pos.start.x, pos.start.y + leapValue.upPosValue, pos.start.z);
        pos.endTop = new Vector3(pos.end.x, pos.end.y + leapValue.upPosValue, pos.end.z);


        gloom.SetAnimEvent(AnimEvent_Jump);
        gloom.SetTrigger("Leap_Start");
    }

    public void AnimEvent_Jump()
    {
        currentCoroutine = CoAnimEvent_Jump();
        gloom.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoAnimEvent_Jump()
    {
        float timer = 0f;
        float progress = 0f;

        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.jump);

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / leapValue.upTime;

            gloom.myTransform.position = Vector3.Lerp(pos.start, pos.startTop, progress);

            yield return null;
        }


        gloom.myTransform.position = pos.startTop;

        //벽 위치 변경
        //gloom.Com.gloomMap.UpdateWall(endDirection);

        //점프가 끝나면 착지 이벤트 실행
        //gloom.SetAnimEvent(AnimEvent_Fall);

        yield return new WaitForSeconds(1f);

        currentCoroutine = CoAnimEvent_FallDelayTime();
        gloom.StartCoroutine(currentCoroutine);
    }

    /// <summary>
    /// 일정시간 이후에 Leap_End 애니메이션으로 전환합니다.
    /// </summary>
    private IEnumerator DelayLeapEndAnimation()
    {
        yield return downAnimTime;

        //사운드 출력

    }
    /// <summary>
    /// End애니메이션을 자동이 아닌 특정 시간 이후에 실행합니다.
    /// </summary>
    private IEnumerator CoAnimEvent_FallDelayTime()
    {

        //착지 자리에 있는 덩쿨 없애기 
        if (endDirection == eDirection.Right)
        {
            if (gloom.ContainsThornVineDict(6))
            {
                gloom.aliveThornVineDict[6].StartDelayDie();
            }
        }
        else
        {
            if (gloom.ContainsThornVineDict(0))
            {
                gloom.aliveThornVineDict[0].StartDelayDie();
            }
        }


        float timer = 0f;
        float progress = 0f;

        //일정 시간 이후에 착지 애니메이션 실행
        //gloom.StartCoroutine(DelayLeapEndAnimation());
        bool canStartFallAnim = true;
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / leapValue.downTime;

            gloom.myTransform.SetPositionAndRotation(
                Vector3.Lerp(pos.endTop, pos.end, progress),
                Quaternion.Lerp(rot.start, rot.end, progress));

            if (canStartFallAnim)
            {
                if (timer > fDownAnimTime)
                {
                    canStartFallAnim = false;
                    gloom.audioSource.PlayOneShot(gloom.audioClips.land);
                    gloom.audioSource.PlayOneShot(gloom.audioClips.fire);
                    gloom.audioSource.PlayOneShot(gloom.audioClips.fire2);

                    gloom.SetTrigger("Leap_End");
                }

            }

            yield return null;
        }

        gloom.myTransform.SetPositionAndRotation(pos.end, rot.end);
        //gloom.SetTrigger("Leap_End");

        gloom.SkillObj.leapImpact.StartImpact();
        GameManager.instance.cameraManager.ShakeCamera();
        //방향 바꿈 판정
        gloom.ChangeDirection(endDirection);
        canExit = true;
    }
}
public class GloomState_Resonance : GloomState
{

    private HeadParryingHelper helper;

    private GloomController.SkillValues.ResonancePattern skillVal;
    private GloomResonanceScreen resonanceScreen;
    public GloomState_Resonance(GloomController _gloomController)
    {
        gloom = _gloomController;
        skillVal = gloom.SkillVal.resonance;
        helper = gloom.SkillObj.resonanceHelper;
        resonanceScreen = gloom.SkillObj.resonanceScreen;
        resonanceScreen.gloom = _gloomController;
    }

    public override void OnEnter()
    {
        canExit = false;


        gloom.TalkOnce(404);

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("Resonance_Start");

    }
    public override void OnExit()
    {
        ReadyToExit();

        gloom.audioSource.loop = false;
        gloom.audioSource.Stop();
        gloom.SkillObj.resonanceScreen.StopResoanceScreen();
    }

    public void AnimEvent()
    {
        helper.StartCheck();

        currentCoroutine = CoSkill();
        gloom.StartCoroutine(currentCoroutine);
    }
    private IEnumerator CoSkill()
    {
        float timer = 0f;
        // float summonInterval = gloom.SkillVal.resonance.createInterval;
        float summonInterval = 0f;


        gloom.SkillObj.resonanceSphere.SetActive(true);

        //사운드 출력
        gloom.audioSource.loop = true;
        gloom.audioSource.clip = gloom.audioClips.resonanace;
        gloom.audioSource.Play();

        gloom.StartInvincible();

        resonanceScreen.StartResonanceScreen();

        while (timer < skillVal.resonanceTime)
        {
            timer += Time.deltaTime;

            //구슬 패링에 성공했을 경우
            if (helper.isSucceedParry)
            {

                //검사 끝내기
                helper.EndCheck();

                gloom.TalkOnce(405);
                //무력화상태 진입
                ChangeStatePowerless();
                //사운드 출력
                gloom.audioSource.Stop();
                gloom.audioSource.PlayOneShot(gloom.audioClips.parrying);
                yield break;
            }

            //summonInterval초마다 투사체 소환
            if (timer > summonInterval)
            {
                //투사체 소환
                SummonBullet();

                //mod를 쓰고싶었으나 float라서 고려해야할게 너무 많았다.
                summonInterval += skillVal.createInterval;
            }

            yield return null;
        }


        //GameManager.instance.playerController.InputVal.movementInput = 0f;
        //GameManager.instance.playerController.State.moveSystem = true;

        ////얼굴 켜기
        //resonanceScreen.StartResonanceScreen();
        gloom.audioSource.Stop();
        yield return resonanceScreen.waitFaceTime;

        //데미지 주기
        yield return resonanceScreen.waitFaceDuration;
        GameManager.instance.playerController.Hit();
        ReadyToExit();

        yield return resonanceScreen.waitEndTime;
        gloom.SetTrigger("Resonance_End");
        gloom.TalkOnce(406);
        //GameManager.instance.playerController.State.moveSystem = false;

    }

    private Vector3 bulletRot = new Vector3(0f, 0f, -90f);

    /// <summary>
    /// 패링 가능한 투사체를 소환합니다(방해 투사체와 동일)
    /// </summary>
    private void SummonBullet()
    {
        int i = gloom.Com.gloomMap.index.min;
        int length = gloom.Com.gloomMap.index.max;
        for (; i < length; i++)
        {
            MapBlock block = gloom.Com.gloomMap.mapBlocks[i];

            //낭떠러지일 경우
            if (block.currentType == MapBlock.eType.Empty)
            {
                //소환 안함
                continue;
            }

            //이펙트가 땅에 묻히지 않게 0.01f만큼 띄워줌
            Vector3 spawnPos = block.positions.groundCenter + new Vector3(0f, 0.01f, 0f);

            //위를 바라보고 있는 상태로 소환
            GloomObstructSign sign = gloom.Pool.obstructSign.SpawnThis(spawnPos, new Vector3(0f, 0f, 90f), null);

            //투사체 회전값 조절
            sign.SetBulletRotation(bulletRot);
            sign.Init(gloom, spawnPos, block.positions.topCenter, skillVal.moveTime);

            //사운드 출력
            gloom.audioSource.PlayOneShot(gloom.audioClips.resonanceArrow);
        }
    }

    private void ChangeStatePowerless()
    {
        ReadyToExit();

        gloom.audioSource.loop = false;
        gloom.audioSource.Stop();
        gloom.ChangeState((int)eGloomState.Powerless);
    }

    /// <summary>
    /// 상태를 전환하기 전 끝내야하는 일 
    /// </summary>
    private void ReadyToExit()
    {
        helper.EndCheck();
        gloom.EndInvincible();

        gloom.SkillObj.resonanceSphere.SetActive(false);
    }



}
public class GloomState_Threat : GloomState
{


    private WaitForSeconds waitSec;
    public GloomState_Threat(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(0.4f);
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoSkill();

        gloom.SetTrigger("Threat_Start");
        gloom.SetAnimEvent(AnimEvent);
    }

    public void AnimEvent()
    {
        gloom.StartCoroutine(currentCoroutine);

    }

    private IEnumerator CoSkill()
    {
        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.weild);

        gloom.SkillObj.threat.SetActive(true);
        yield return waitSec;
        gloom.SkillObj.threat.SetActive(false);
    }
}
public class GloomState_ThornPath : GloomState
{
    private eDirection diretion;
    private int[] blockArr;

    private WaitForSeconds waitSec;
    public GloomState_ThornPath(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(gloom.SkillVal.thorn.waitTime);
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoSkill();


        //현재 보스의 방향을 가져옴
        diretion = gloom.diretion;

        //사용할 수 있는 블록 리스트를 가져옴
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.ExcludeVine).ToArray();

        //블록 인덱스를 랜덤하게 섞음
        ShuffleArray();

        //애니메이션 설정
        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornPath_Start");
    }

    public void AnimEvent()
    {
        //가져온게 없으면
        if (blockArr.Length == 0)
        {
            //바로 End
            gloom.SetTrigger("ThornPath_End");
            return;
        }
        gloom.StartCoroutine(currentCoroutine);
    }
    private IEnumerator CoSkill()
    {
        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.magic);

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
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thorn.hp, gloom.SkillVal.thorn.waitTime, block.positions.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
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
            thornVine.SetValues(block, blockArr[0], gloom.SkillVal.thorn.hp, gloom.SkillVal.thorn.waitTime, block.positions.groundCenter);
            thornVine.UpdateEndPosition();

            thornVine.StartGrow();
        }

        //가시가 자라나기 전까지 대기
        yield return waitSec;

        //끝나는 애니메이션 출력
        gloom.SetTrigger("ThornPath_End");

        //가시 자라는 소리 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.thorn);
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

    private Vector3 endPos;
    private Vector3[] endPosArr = null;

    private Vector3 rot;
    private Vector3 bulletRot;

    private WaitForSeconds waitSec = null;

    private float bulletMoveTime;
    public GloomState_Obstruct(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(gloom.SkillVal.obstruct.createInterval);
        bulletMoveTime = gloom.SkillVal.obstruct.moveTime;
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoSkill();

        currentIndex = -1;
        usableIndex = new List<int> { 0, 1, 2 };
        usedIndex = new List<int>();

        //방향에 따라 투사체 설정
        if (gloom.diretion == eDirection.Right)
        {
            endPos = gloom.Com.gloomMap.mapData.minPosition;
            endPos -= gloom.SkillVal.extendEndPos;
            rot = new Vector3(0f, -10f, 0f);
            bulletRot = new Vector3(0f, 0f, 0f);
        }
        else
        {
            endPos = gloom.Com.gloomMap.mapData.maxPosition;
            endPos += gloom.SkillVal.extendEndPos;
            rot = new Vector3(0f, 10f, 0f);
            bulletRot = new Vector3(0f, -180f, 0f);
        }

        endPosArr = new Vector3[] {
                new Vector3(endPos.x,gloom. SkillVal.obstruct.positions[0].y,gloom.SkillVal.obstruct.positions[0].z),
                new Vector3(endPos.x,gloom. SkillVal.obstruct.positions[1].y,gloom.SkillVal.obstruct.positions[1].z),
                new Vector3(endPos.x,gloom.SkillVal.obstruct.positions[2].y,gloom.SkillVal.obstruct.positions[2].z)
            };

        gloom.SetTrigger("Obstruct_Start");

        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.scratchGround);

        gloom.SetAnimEvent(AnimEvent);
    }

    public void AnimEvent()
    {
        gloom.StartCoroutine(currentCoroutine);
    }


    private IEnumerator CoSkill()
    {
        yield return null;

        for (int i = 0; i < 9; i++)
        {
            int index = GetUsablePositionIndex();
            Vector3 startPos = gloom.SkillVal.obstruct.positions[index];


            GloomObstructSign sign = gloom.Pool.obstructSign.SpawnThis(startPos, rot, null); ;
            sign.SetBulletRotation(bulletRot);
            sign.Init(gloom, startPos, endPosArr[index], bulletMoveTime);
            //사운드 출력
            gloom.audioSource.PlayOneShot(gloom.audioClips.resonanceArrow);
            //GloomObstructBullet bullet = gloom.Pool.obstructBullet.SpawnThis(startPos);
            //bullet.Init(gloom, startPos, endPosArr[index]);
            //bullet.Move();

            yield return waitSec;
        }

        canExit = true;
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
    private eDirection diretion;
    private int[] blockArr;

    private WaitForSeconds waitSec;
    public GloomState_ThornForest(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(gloom.SkillVal.thorn.waitTime);
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoSkill();
        //현재 보스의 방향을 가져옴
        diretion = gloom.diretion;

        //사용할 수 있는 블록 리스트를 가져옴
        blockArr = gloom.GetUsableBlockList(GloomController.eUsableBlockMode.Default).ToArray();

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("ThornForest_Start");
    }
    public void AnimEvent()
    {
        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.magic);

        //가져온게 없으면
        if (blockArr.Length == 0)
        {
            //바로 End
            gloom.SetTrigger("ThornForest_End");
            return;
        }
        gloom.StartCoroutine(currentCoroutine);
    }
    private IEnumerator CoSkill()
    {
        int length;
        //보스가 오른쪽에 있으면
        if (diretion == eDirection.Right)
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
                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thorn.hp, gloom.SkillVal.thorn.waitTime, block.positions.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
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

                thornVine.SetValues(block, blockArr[i], gloom.SkillVal.thorn.hp, gloom.SkillVal.thorn.waitTime, block.positions.groundCenter);
                thornVine.UpdateEndPosition();

                thornVine.StartGrow();
            }
        }
        yield return waitSec;
        gloom.SetTrigger("ThornForest_End");
        gloom.audioSource.PlayOneShot(gloom.audioClips.thorn);
    }
}
public class GloomState_Wave : GloomState
{

    private Vector3 startPos;
    private Vector3 endPos;

    private WaitForSeconds waitSec;

    public GloomState_Wave(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(4.2f);
    }
    public override void OnEnter()
    {
        canExit = false;
        currentCoroutine = CoEndAudio();
        if (gloom.diretion == eDirection.Right)
        {
            endPos = gloom.Com.gloomMap.mapData.minPosition;
            endPos -= gloom.SkillVal.extendEndPos;
        }
        else
        {
            endPos = gloom.Com.gloomMap.mapData.maxPosition;
            endPos += gloom.SkillVal.extendEndPos;
        }

        gloom.SetAnimEvent(AnimEvent);

        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.longHowling);

        gloom.SetTrigger("Wave_Start");
    }

    public void AnimEvent()
    {
        startPos = gloom.SkillObj.waveTransform.position;
        GloomWaveBullet upBullet = gloom.Pool.waveBullet.SpawnThis(startPos);
        GloomWaveBullet downBullet = gloom.Pool.waveBullet.SpawnThis(startPos);

        upBullet.Init(gloom, GloomWaveBullet.eMode.Up);
        upBullet.SetPosition(startPos, endPos);

        downBullet.Init(gloom, GloomWaveBullet.eMode.Down);
        downBullet.SetPosition(startPos, endPos);

        upBullet.Move();
        downBullet.Move();

        //사운드 출력
        gloom.audioSource.loop = true;
        gloom.audioSource.clip = gloom.audioClips.wave;
        gloom.audioSource.Play();

        gloom.StartCoroutine(currentCoroutine);
    }

    private IEnumerator CoEndAudio()
    {
        yield return waitSec;
        gloom.audioSource.Stop();
    }

    public override void OnExit()
    {
        gloom.audioSource.Stop();
    }

}
public class GloomState_Advance : GloomState
{
    private Vector3 startPos;
    private Vector3 endPos;

    public GloomLightning lightning = null;
    public GloomState_Advance(GloomController _gloomController)
    {
        gloom = _gloomController;
        lightning = gloom.SkillObj.gloomLightning;
        lightning.moveTime = gloom.SkillVal.advance.moveTime;

    }
    public override void OnEnter()
    {
        canExit = false;

        currentCoroutine = CoSkill();
        beginLightning = lightning.CoBeginMove();
        moveLightning = lightning.CoMove();
        lightning.Init();

        //오른쪽에 있으면
        if (gloom.diretion == eDirection.Right)
        {
            startPos = gloom.Com.gloomMap.mapBlocks[gloom.Com.gloomMap.index.max - 1].positions.topCenter;
            endPos = gloom.Com.gloomMap.mapBlocks[gloom.Com.gloomMap.index.min + 1].positions.topCenter;

        }
        else
        {
            startPos = gloom.Com.gloomMap.mapBlocks[gloom.Com.gloomMap.index.min].positions.topCenter;
            endPos = gloom.Com.gloomMap.mapBlocks[gloom.Com.gloomMap.index.max - 2].positions.topCenter;
        }

        gloom.SetAnimEvent(AnimEvent);
        gloom.SetTrigger("Advance_Start");
        UIManager.Instance.Talk("반대쪽으로 도망치자!");
    }

    public void AnimEvent()
    {
        gloom.StartCoroutine(currentCoroutine);
    }


    private IEnumerator beginLightning;
    private IEnumerator moveLightning;
    private IEnumerator CoSkill()
    {
        lightning.gameObject.SetActive(true);

        lightning.SetMoveSpherePosition(gloom.SkillObj.sphereTransform.position, startPos);
        //lightning.Init_Position();

        yield return gloom.StartCoroutine(beginLightning);

        lightning.SetMovePosition(startPos, endPos);
        // lightning.Init_Position();
        yield return gloom.StartCoroutine(moveLightning);

        lightning.gameObject.SetActive(false);

        gloom.SetTrigger("Advance_End");
    }

    public override void OnExit()
    {
        gloom.StopCoroutine(beginLightning);
        gloom.StopCoroutine(moveLightning);
        lightning.gameObject.SetActive(false);
        AudioManager.Instance.Audios.audioSource_SFX.Stop();
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
        gloom.SkillObj.berserk.StartBerserk();
        gloom.TalkOnce(407);
        gloom.StartCoroutine(CoWait());
    }


    private IEnumerator CoWait()
    {
        yield return new WaitForSeconds(2f);
        canExit = true;
    }

    //public override void OnExit()
    //{
    //}
}
public class GloomState_Powerless : GloomState
{

    private WaitForSeconds waitSec;
    public GloomState_Powerless(GloomController _gloomController)
    {
        gloom = _gloomController;
        waitSec = new WaitForSeconds(gloom.SkillVal.resonance.powerlessTime);
    }

    public override void OnEnter()
    {
        canExit = false;


        gloom.TalkOnce(405);
        currentCoroutine = CoPowerless();
        gloom.StartCoroutine(currentCoroutine);

        gloom.SetTrigger("Powerless_Start");
    }

    private IEnumerator CoPowerless()
    {
        yield return waitSec;
        gloom.SetTrigger("Powerless_End");
    }
}
public class GloomState_Die : GloomState
{
    private WaitForSeconds waitForMoviePlay = new WaitForSeconds(2f);
    public GloomState_Die(GloomController _gloomController)
    {
        gloom = _gloomController;
    }
    public override void OnEnter()
    {
        canExit = false;

        //쉐이크 끝나게...제발
        GameManager.instance.cameraManager.vcamNoise.enabled = false;

        //사운드 출력
        gloom.audioSource.PlayOneShot(gloom.audioClips.death);

        gloom.TalkOnce(412);

        //광폭화 종료
        gloom.SkillObj.berserk.EndBerserk();

        //애니메이션 전환
        gloom.SetTrigger("Die_Start");
        gloom.StartCoroutine(ProcessDie());
    }
    public void AnimEvent()
    {
        gloom.animator.enabled = false;
        gloom.SkillObj.whiskers.SetActive(false);

        //gloom.Com.bodyCollider.enabled = false;
        gloom.Com.wallCollider.enabled = false;

        gloom.StartCoroutine(CoEnding());

    }


    private IEnumerator ProcessDie()
    {
        //Death로 전환될 때 까지 대기!!
        yield return new WaitUntil(() => gloom.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"));

        //이벤트 설정
        gloom.SetAnimEvent(AnimEvent);


    }
    private IEnumerator CoEnding()
    {
        Data_Player tempData = new Data_Player();
        tempData.currentStageName = SceneNames.stage_04;
        tempData.currentStageNumber = 4;
        tempData.finalStageName = SceneNames.stage_04;
        tempData.finalStageNumber = 4;
        DataManager.Instance.currentClearStageNumber = 4;
        DataManager.Instance.currentData_player.CopyData(tempData);
        yield return gloom.StartCoroutine(DataManager.Instance.SaveCurrentData(DataName.player));
        yield return new WaitForSeconds(2f);


        SceneChanger.Instance.LoadThisScene("Ending");

        //uiMovie.gameObject.SetActive(true);
        //uiMovie.onMovieEnded += UiMovie_onMovieEnded;

        //Time.timeScale = 0f;
        //uiMovie.StartCoroutine(uiMovie.playMovie);
    }
}


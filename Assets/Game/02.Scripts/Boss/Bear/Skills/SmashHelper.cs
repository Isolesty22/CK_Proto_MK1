using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 곰의 스매시 스킬에 사용되는 돌을 관리합니다.
/// </summary>
public class SmashHelper : MonoBehaviour
{

    public Transform myTransform;
    public SmashRock[] smashRocks;


    public Transform[] transforms;
    private void Awake()
    {
        for (int i = 0; i < smashRocks.Length; i++)
        {
            smashRocks[i].UpdateVectors(transforms[0].position, transforms[1].position, transforms[2].position);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < smashRocks.Length; i++)
        {
            smashRocks[i].StartMove();
            yield return new WaitForSeconds(0.3f);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomBerserk : MonoBehaviour
{

    [Tooltip("발 부분에 생기는 불 이펙트입니다.")]
    public ParticleSystem[] toeFlames;
    public SkinnedMeshRenderer meshRenderer;


    [ColorUsage(true, true)]
    public Color originColor;
    [ColorUsage(true, true)]
    public Color berserkColor;

    private Material maneMat;
    private int toeFlamesLength;

    private void Awake()
    {
        //두번째 마테리얼 받아오기
        maneMat = meshRenderer.materials[1];
        maneMat.SetColor("_EmissionColor", originColor);

        toeFlamesLength = toeFlames.Length;
        for (int i = 0; i < toeFlamesLength; i++)
        {
            toeFlames[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 이펙트를 활성화시킵니다.
    /// </summary>
    public void StartBerserk()
    {
        for (int i = 0; i < toeFlamesLength; i++)
        {
            toeFlames[i].gameObject.SetActive(true);
            toeFlames[i].Play();
        }
        maneMat.SetColor("_EmissionColor", berserkColor);
    }

    /// <summary>
    /// 이펙트를 비활성화시킵니다.
    /// </summary>
    public void EndBerserk()
    {
        for (int i = 0; i < toeFlamesLength; i++)
        {
            toeFlames[i].Stop();
        }
        maneMat.SetColor("_EmissionColor", originColor);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임 사운드 관리
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }
    #endregion

    [Serializable]
    public class AudioSourceCol
    {
        public AudioSource audioSource_BGM;
        public AudioSource audioSource_EVM;
        public AudioSource audioSource_SFX;
    }
    [SerializeField] private AudioSourceCol audioSource = new AudioSourceCol();

    public AudioSourceCol AudioSources => audioSource;


    //[Serializable]
    //public class AudioClipDictonaries
    //{
    public Dictionary<AudioClip, string> clipDict_BGM = new Dictionary<AudioClip, string>();
    public Dictionary<AudioClip, string> clipDict_EVM = new Dictionary<AudioClip, string>();
    public Dictionary<AudioClip, string> clipDict_SFX = new Dictionary<AudioClip, string>();
    //}
    //[SerializeField] private AudioClipDictonaries audioClipDictonaries = new AudioClipDictonaries();

    //public AudioClipDictonaries ClipDict => audioClipDictonaries;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
    }


    public void Init()
    {

    }

}

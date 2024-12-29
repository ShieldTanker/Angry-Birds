using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region 싱글톤
    static SoundManager instance;
    public static SoundManager SM { get { return instance; } set { instance = value; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public AudioSource audioSource;
    [SerializeField] AudioClip titleBGM;

    public List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadVolumeScale();

        SetBGM(titleBGM);
    }

    #region 볼륨 관련
    public float volume;
    public void SetVolume(float volumeScale)
    {
        PlayerPrefs.SetFloat("volumeScale", volumeScale);
        volume = volumeScale;
        audioSource.volume = volume;
    }

    public void LoadVolumeScale()
    {
        volume = PlayerPrefs.GetFloat("volumeScale", 1);
        audioSource.volume = volume;
    }
    #endregion

    #region 오디오 클립 설정 관련
    public void SetBGM(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.Log("asdasdasd");
            return;
        }

        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public int SetRandomAudioIdx(AudioClip[] stageAudioClips)
    {
        int idx = 0;

        if (audioSource != null && stageAudioClips.Length > 0)
            idx = UnityEngine.Random.Range(0, stageAudioClips.Length);

        return idx;
    }
    #endregion

    #region 오디오 클립 재생 관련
    public void PlayRandomAudio(AudioClip[] clips)
    {
        PlayAudio(clips[SetRandomAudioIdx(clips)]);
    }

    public virtual void PlayAudio(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    #endregion
}
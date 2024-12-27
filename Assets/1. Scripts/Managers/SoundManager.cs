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
    }
    #endregion

    public AudioSource audioSource;

    public List<AudioSource> audioSources = new List<AudioSource>();
    public void SetVolume(float volumeScale)
    {
        GameManager.GM.SetVolumeScale(volumeScale);

        foreach (AudioSource source in audioSources)
        {
            source.volume = GameManager.GM.volumeScale;
        }
    }

    /// <summary>
    /// 랜덤 오디오 재생
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clips"></param>
    public void PlayRandomAudio(AudioSource source, AudioClip[] clips)
    {
        if (source == null || clips.Length <= 0)
            return;
        
        int idx = UnityEngine.Random.Range(0, clips.Length);

        PlayAudio(clips[idx]);
    }

    public void PlayRandomAudio(AudioClip[] clips)
    {
        if (audioSource == null || clips.Length <= 0)
            return;

        int idx = UnityEngine.Random.Range(0, clips.Length);

        PlayAudio(clips[idx]);
    }

    /// <summary>
    /// 해당 오디오 클립 재생
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    public virtual void PlayAudio(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null)
            return;

        source.PlayOneShot(clip);
    }

    public virtual void PlayAudio(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}

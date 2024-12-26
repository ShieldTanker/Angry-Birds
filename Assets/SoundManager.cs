using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ
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
    /// ·£´ý ¿Àµð¿ÀÀç»ý
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clips"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void PlayRandomAudio(AudioSource source, AudioClip[] clips, int min, int max)
    {
        if(max >= clips.Length)
            max = clips.Length - 1;
        
        int idx = Random.Range(min, max);

        PlayAudio(source, clips[idx]);
    }

    public virtual void PlayAudio(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}

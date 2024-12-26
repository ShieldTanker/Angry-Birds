using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityBase : MonoBehaviour
{
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public Collider2D col;

    [NonSerialized] public float prevMagnitude;
    
    [Header("VelocityBase"), Tooltip("����")]
    public int point;

    public float destroyTime;
    public float resistance;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        prevMagnitude = rb.velocity.magnitude;
    }
    public virtual void Die()
    {
        Debug.Log("Vellocity Base �� Die()");
        Destroy(gameObject, destroyTime);
    }

    /// <summary>
    /// ������ ����� ���� ���
    /// </summary>
    /// <param name="minIdx"></param>
    /// <param name="maxIdx"></param>
    public virtual void PlayRandomAudio(int minIdx, int maxIdx)
    {
        if(maxIdx >= int.MaxValue)
            maxIdx = int.MaxValue - 1;
        
        int idx = UnityEngine.Random.Range(minIdx, maxIdx + 1);
        audioSource.PlayOneShot(audioClips[idx]);
    } 
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class BirdBase : ObjectBase
{
    [Header("BirdBase")]
    public AudioClip flyAudioClip;
    public AudioClip selectedAudioClip;

    public float currentSpeed;
    public float power;
    public int idx;

    [Space]
    public float initTime;
    public float currentTime;
    [SerializeField] bool stopTime;

    [Space]
    public bool usedAbility;
    public float abilityTime;

    [SerializeField] bool isSetted;


    #region 기본 함수
    public virtual void Start()
    {
        usedAbility = false;
        isSetted = false;

        currentTime = 0;

        StageManager.SM.BirdCount++;
        StageManager.SM.birdIdxLength++;
    }

    public virtual void Update()
    {
        currentSpeed = rb.velocity.magnitude;

        // 발사체가 자신이 아니거나 이미 세팅을 했었다면 리턴
        if (SlingShot.SS.ShottedBird != this || isSetted)
            return;

        if (Hit == true)
        {
            stopTime = true;

            // 일정속도 이하이면 남은시간 이어 재생
            if (rb.velocity.magnitude <= resistance)
                stopTime = false;
        }

        if (!stopTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= initTime)
                SetBird();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Hit = true;

        float otherSpeed = 0;
        float speed = prevMagnitude;
        
        if (collision.gameObject.GetComponent<ObjectBase>())
        {
            ObjectBase otherRb = collision.gameObject.GetComponent<ObjectBase>();
            otherSpeed = otherRb.prevMagnitude;
        }

        if (speed >= soundResistance || otherSpeed >= soundResistance)
            SoundManager.SM.PlayRandomAudio(audioSource, hitAudioClips);
    }
    #endregion

    #region 만든 함수

    /// <summary>
    /// 새의 능력
    /// </summary>
    /// <param name="time"></param>
    public virtual void BirldAbility(float time)
    {
        usedAbility = true;
    }

    /// <summary>
    /// 세팅한적이 없으면 새총에 새를 세팅
    /// </summary>
    void SetBird()
    {
        if (!isSetted)
        {
            SlingShot.SS.SetBird();
            isSetted = true;
            stopTime = true;
        }
    }

    /// <summary>
    /// 사망시 호출
    /// </summary>
    public override void Die()
    {
        if (SlingShot.SS.ShottedBird == this && !isSetted)
        {
            Debug.Log("BirdBase 의 Die()");

            SetBird();
        }

        base.Die();
    }

    #endregion

    #region 코루틴

    /// <summary>
    /// 물리작용 활성
    /// </summary>
    public void EnAbleVellocity()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = true;
        StartCoroutine(TurnOffHit());
    }

    /// <summary>
    /// 물리작용 비활성
    /// </summary>
    public void DisAbleVellocity() => StartCoroutine(DisAbleColliderCoroutine());

    /// <summary>
    /// 물리효과 끄기
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisAbleColliderCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        StartCoroutine(TurnOffHit());
    }

    /// <summary>
    /// 0.1초뒤 Hit를 false 로 바꿈
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(0.1f);
        Hit = false;
    }
    #endregion
}
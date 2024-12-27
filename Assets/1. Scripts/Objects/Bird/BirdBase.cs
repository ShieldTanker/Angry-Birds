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
    public Sprite flySprite;
    public Sprite abilitiedImage;

    [Space]
    public AudioClip flyAudioClip;
    public AudioClip selectedAudioClip;
    public AudioClip abilityAudioClip;

    [Space]
    public float currentSpeed;
    public float power;
    public float abilityPower;
    public float abilityLength;
    public float abilityTime;
    public int idx;

    [Space]
    public float initTime;
    public float currentTime;
    [SerializeField] bool stopTime;

    [Space]
    public bool usedAbility;

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

        if (Hit)
        {
            stopTime = true;

            if (rb.velocity.magnitude <= resistance)
                SetBird();
        }

        if (!stopTime)
        {
            if (currentTime >= initTime)
                SetBird();

            currentTime += Time.deltaTime;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        // BirdBase 는 안쓰는 함수라서 빈코드로 덮어씌움
    }

    /*    public void OnCollisionEnter2D(Collision2D collision)
        {
            Hit = true;
            SetIdleImage();
            base.CalcCollision(collision);

            if (prevMagnitude >= soundResistance || otherSpeed >= soundResistance)
            {
                HitCoroutineStart();
                // SoundManager.SM.PlayRandomAudio(audioSource, hitAudioClips);
                SoundManager.SM.PlayRandomAudio(hitAudioClips);
            }
        }*/
    #endregion

    #region 만든 함수

    public void SetFlyImage()
    {
        spriteRenderer.sprite = flySprite;
    }

    /// <summary>
    /// 새의 능력
    /// </summary>
    /// <param name="time"></param>
    public virtual void BirldAbility()
    {
        if (usedAbility)
            return;

        usedAbility = true;

        if (abilitiedImage != null)
            spriteRenderer.sprite = abilitiedImage;

        SoundManager.SM.PlayAudio(abilityAudioClip);
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
        rb.simulated = true;

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
        rb.bodyType = RigidbodyType2D.Static;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.simulated = false;
        col.enabled = false;

        Hit = false;
        // StartCoroutine(TurnOffHit());
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
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class BirdBase : VelocityBase
{
    [Header("BirdBase")]
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
    [SerializeField] public bool Hit;

    [Space]
    public RectTransform txtEndPos;
    public TMP_Text pointTxt;
    public float txtSpeed;

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
        
        if ( Hit == true)
        {
            stopTime = true;

            // 일정속도 이하이면 남은시간 이어 재생
            if(rb.velocity.magnitude <= resistance)
                stopTime = false;
        }

        if (!stopTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= initTime)
                SetBird();   
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name}이 {collision.gameObject.name}과 부딫힘");
        Hit = true;
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
    /// 점수 텍스트 표시
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowPointTxt()
    {
        pointTxt.gameObject.SetActive(true);
        pointTxt.text = "" + point;

        StageManager.SM.currentScore += point;

        // 로컬 좌표를 기준으로 시작 위치와 목표 위치 설정
        Vector3 localTxtPos = pointTxt.rectTransform.localPosition;
        Vector3 localEndPos = txtEndPos.localPosition;

        // 초기 거리 계산
        float distance = Vector3.Distance(localTxtPos, localEndPos);

        while (distance > 0.05f)
        {
            // Lerp를 사용하여 점진적으로 이동
            pointTxt.rectTransform.localPosition =
                Vector3.Lerp(pointTxt.rectTransform.localPosition, localEndPos, txtSpeed * Time.deltaTime);

            // 거리 갱신
            distance = Vector3.Distance(pointTxt.rectTransform.localPosition, localEndPos);

            yield return null;
        }

        // 최종 위치 보정
        pointTxt.rectTransform.localPosition = localEndPos;

        // 이동이 끝난 후 1초 대기
        yield return new WaitForSeconds(1f);

        pointTxt.gameObject.SetActive(false);
    }


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
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ObjectBase : MonoBehaviour
{
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    [SerializeField] public bool Hit;

    [NonSerialized] public float prevMagnitude;
    
    [Header("VelocityBase"), Tooltip("점수")]
    public int point;

    public float destroyTime;
    public float resistance;
    public float soundResistance;

    [Space]
    public TMP_Text pointTxt;
    public float endDistance;
    public float txtSpeed;
    public float showTxtTime;

    public AudioSource audioSource;
    public AudioClip[] hitAudioClips;
    public AudioClip[] dieAudioClips;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {
        prevMagnitude = rb.velocity.magnitude;
    }

    /// <summary>
    /// 점수 텍스트 표시
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowPointTxt()
    {
        if (gameObject.layer == CameraManager.CM.targetLayer)
            CameraManager.CM.SetTarget(gameObject.transform);
        
        pointTxt.gameObject.SetActive(true);
        pointTxt.text = "" + point;

        StageManager.SM.currentScore += point;

        // 로컬 좌표를 기준으로 시작 위치와 목표 위치 설정
        pointTxt.transform.eulerAngles = Vector3.zero;
        Vector3 localTxtPos = pointTxt.rectTransform.localPosition;
        Vector3 localEndPos = localTxtPos + Vector3.up * endDistance;

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
       
        // 이동이 끝난 후 대기
        yield return new WaitForSeconds(showTxtTime);
        pointTxt.gameObject.SetActive(false);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        float otherSpeed = 0;
        float speed = prevMagnitude;

        if (collision.gameObject.GetComponent<ObjectBase>())
        {
            ObjectBase otherRb = collision.gameObject.GetComponent<ObjectBase>();
            otherSpeed = otherRb.prevMagnitude;
        }

        // 자신의 속도 or 상대의 속도가 제한속도 이상일때
        if (speed >= resistance || otherSpeed >= resistance)
            Die();
        else if (speed >= soundResistance || otherSpeed >= soundResistance)
        {
            SoundManager.SM.PlayRandomAudio(audioSource, hitAudioClips);
        }
    }

    public virtual void Die()
    {
        SoundManager.SM.PlayRandomAudio(audioSource, dieAudioClips);
        
        spriteRenderer.enabled = false;
        col.enabled = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;
        
        Destroy(gameObject, destroyTime);
    }
}
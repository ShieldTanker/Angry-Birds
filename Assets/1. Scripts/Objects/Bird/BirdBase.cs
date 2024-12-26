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

    #region �⺻ �Լ�
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

        // �߻�ü�� �ڽ��� �ƴϰų� �̹� ������ �߾��ٸ� ����
        if (SlingShot.SS.ShottedBird != this || isSetted)
            return;
        
        if ( Hit == true)
        {
            stopTime = true;

            // �����ӵ� �����̸� �����ð� �̾� ���
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
        Debug.Log($"{gameObject.name}�� {collision.gameObject.name}�� �΋H��");
        Hit = true;
    }
    #endregion

    #region ���� �Լ�

    /// <summary>
    /// ���� �ɷ�
    /// </summary>
    /// <param name="time"></param>
    public virtual void BirldAbility(float time)
    {
        usedAbility = true;
    }

    /// <summary>
    /// ���������� ������ ���ѿ� ���� ����
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
    /// ����� ȣ��
    /// </summary>
    public override void Die()
    {
        if (SlingShot.SS.ShottedBird == this && !isSetted)
        {
            Debug.Log("BirdBase �� Die()");

            SetBird();
        }
        
        base.Die();
    }

    #endregion

    #region �ڷ�ƾ
    /// <summary>
    /// ���� �ؽ�Ʈ ǥ��
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowPointTxt()
    {
        pointTxt.gameObject.SetActive(true);
        pointTxt.text = "" + point;

        StageManager.SM.currentScore += point;

        // ���� ��ǥ�� �������� ���� ��ġ�� ��ǥ ��ġ ����
        Vector3 localTxtPos = pointTxt.rectTransform.localPosition;
        Vector3 localEndPos = txtEndPos.localPosition;

        // �ʱ� �Ÿ� ���
        float distance = Vector3.Distance(localTxtPos, localEndPos);

        while (distance > 0.05f)
        {
            // Lerp�� ����Ͽ� ���������� �̵�
            pointTxt.rectTransform.localPosition =
                Vector3.Lerp(pointTxt.rectTransform.localPosition, localEndPos, txtSpeed * Time.deltaTime);

            // �Ÿ� ����
            distance = Vector3.Distance(pointTxt.rectTransform.localPosition, localEndPos);

            yield return null;
        }

        // ���� ��ġ ����
        pointTxt.rectTransform.localPosition = localEndPos;

        // �̵��� ���� �� 1�� ���
        yield return new WaitForSeconds(1f);

        pointTxt.gameObject.SetActive(false);
    }


    /// <summary>
    /// �����ۿ� Ȱ��
    /// </summary>
    public void EnAbleVellocity()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = true;
        StartCoroutine(TurnOffHit());
    }

    /// <summary>
    /// �����ۿ� ��Ȱ��
    /// </summary>
    public void DisAbleVellocity() => StartCoroutine(DisAbleColliderCoroutine());

    /// <summary>
    /// ����ȿ�� ����
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
    /// 0.1�ʵ� Hit�� false �� �ٲ�
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(0.1f);
        Hit = false;
    }
    #endregion
}
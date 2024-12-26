using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : VelocityBase
{
    [Header("ObjectBase")]
    public bool wasPointGiven = false;

    [Space]
    public int audioMinIdx;
    public int audioMaxIdx;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.SM.PlayRandomAudio(audioSource, audioClips, audioMinIdx, audioMaxIdx);
        float otherSpeed = 0;
        float speed = prevMagnitude;

        if (collision.gameObject.GetComponent<VelocityBase>())
        {
            VelocityBase otherRb = collision.gameObject.GetComponent<VelocityBase>();
            otherSpeed = otherRb.prevMagnitude;
        }

        // �ڽ��� �ӵ� or ����� �ӵ��� ���Ѽӵ� �̻��϶�
        if (speed >= resistance || otherSpeed >= resistance)
        {
            Die();
        }
    }

    public override void Die()
    {
        if (wasPointGiven)
            return;

        wasPointGiven = true;
        StageManager.SM.currentScore += point;

        Debug.Log("StageManager.CheckCount()");
        StageManager.SM.CheckCount();

        base.Die();
    }
}

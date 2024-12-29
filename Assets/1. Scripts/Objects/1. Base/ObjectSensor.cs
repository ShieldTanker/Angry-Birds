using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSensor : MonoBehaviour
{
    [NonSerialized] public float prevMagnitude;
    [SerializeField] ObjectBase parent;
    public LayerMask targetLayer;

    void FixedUpdate()
    {
        prevMagnitude = parent.rb.velocity.magnitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.Hit = true;
        parent.SetIdleImage();

        if (collision.gameObject.GetComponent<ObjectBase>())
        {
            ObjectBase otherRb = collision.gameObject.GetComponent<ObjectBase>();
            parent.otherSpeed = otherRb.prevMagnitude;
        }

        if (parent.prevMagnitude >= parent.soundResistance || parent.otherSpeed >= parent.soundResistance)
        {
            parent.HitCoroutineStart();
            SoundManager.SM.PlayRandomAudio(parent.hitAudioClips);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pig : ObjectBase
{
    private void Start()
    {
        StageManager.SM.PigCount++;
    }

    public override void Die()
    {
        StageManager.SM.PigCount--;
        SoundManager.SM.PlayAudio(audioSource, audioClips[0]);
        base.Die();
    }
}

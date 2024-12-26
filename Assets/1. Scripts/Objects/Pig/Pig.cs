using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pig : ObstacleBase
{
    public override void Start()
    {
        base.Start();
        StageManager.SM.PigCount++;
    }

    public override void Die()
    {
        if (wasPointGiven)
            return;

        StageManager.SM.PigCount--;
        SoundManager.SM.PlayRandomAudio(audioSource, dieAudioClips);
        base.Die();
    }
}

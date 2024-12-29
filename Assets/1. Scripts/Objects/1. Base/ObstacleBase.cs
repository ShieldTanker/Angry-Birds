using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : ObjectBase
{
    [Header("ObstacleBase")]
    public bool wasPointGiven = false;

    public virtual void Start()
    {
        Hit = true;
    }

    public override void Die()
    {
        if (wasPointGiven)
            return;
        
        wasPointGiven = true;

        StageManager.SM.currentScore += point;
        StageManager.SM.CheckCount();

        StartCoroutine(ShowPointTxt());

        base.Die();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBird : BirdBase
{
    public override void BirldAbility()
    {
        base.BirldAbility();
        rb.AddForce(rb.velocity * abilityPower, ForceMode2D.Impulse);
     }
}

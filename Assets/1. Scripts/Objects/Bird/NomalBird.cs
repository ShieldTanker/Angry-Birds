using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalBird : BirdBase
{
    public override void BirldAbility(float time)
    {
        base.BirldAbility(time);
        Debug.Log($"{gameObject.name}의 능력 발동"); 
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void Die()
    {
        Debug.Log("Nomal Bird 의 Die()");
        base.Die();
    }
}
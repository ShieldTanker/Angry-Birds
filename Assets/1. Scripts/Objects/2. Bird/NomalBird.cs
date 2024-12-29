using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalBird : BirdBase
{
    public override void BirldAbility()
    {
        base.BirldAbility();
        Debug.Log($"{gameObject.name}의 능력 발동"); 
    }
    public override void Die()
    {
        Debug.Log("Nomal Bird 의 Die()");
        base.Die();
    }
}
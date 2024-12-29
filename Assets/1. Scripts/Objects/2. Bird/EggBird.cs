using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggBird : BirdBase
{
    public GameObject egg;
    public float firePower;

    public override void BirldAbility()
    {
        base.BirldAbility();

        rb.AddForce(Vector2.up * firePower * 2, ForceMode2D.Impulse);
        GameObject firedEgg = Instantiate(egg, transform.position, Quaternion.identity);

        firedEgg.GetComponent<Rigidbody2D>().AddForce(Vector2.down * firePower, ForceMode2D.Impulse);
    }

    public override void Die()
    {
        Debug.Log("EggBird의 Die()");
        base.Die();
    }
}

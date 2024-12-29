using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BombBird : BirdBase
{
    public LayerMask targetLayer;
    public float initTimer;
    bool canUse = false;

    public override void Start()
    {
        base.Start();
        StartCoroutine(InitTimer());
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (canUse)
            StartCoroutine(IgnitionBomb());
    }

    public override void BirldAbility()
    {
        base.BirldAbility();
        canUse = false;
        Debug.Log("폭발 작동");

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, abilityLength, targetLayer);

        for (int i = 0; i < others.Length; i++)
        {
            Vector3 dir = others[i].transform.position - transform.position;

            ObjectBase go = others[i].gameObject.GetComponent<ObjectBase>();
            go.rb.AddForce(dir * abilityPower, ForceMode2D.Impulse);
        }

        spriteRenderer.sprite = abilitiedImage;
    }

    IEnumerator IgnitionBomb()
    {
        if (canUse)
        {
            canUse = false;
            yield return new WaitForSeconds(abilityTime);
            BirldAbility();

            yield return new WaitForSeconds(1);
            SetIdleImage();
        }
    }

    IEnumerator InitTimer()
    {
        yield return new WaitForSeconds(initTimer);
        canUse = true;
    }
}

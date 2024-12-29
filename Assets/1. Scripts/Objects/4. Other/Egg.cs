using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Egg : MonoBehaviour
{
    Rigidbody2D rb;
    public LayerMask targetLayer;

    [Space]
    public float eggPower;
    public float range;
    public float destroyTime;
    
    [Space]
    SpriteRenderer spriteRenderer;
    public Sprite sprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);

        for (int i = 0; i < others.Length; i++)
        {
            Vector3 dir = others[i].transform.position - transform.position;

            Rigidbody2D go = others[i].gameObject.GetComponent<Rigidbody2D>();
            go.AddForce(dir * eggPower, ForceMode2D.Impulse);
        }

        spriteRenderer.sprite = sprite;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        Destroy(gameObject, destroyTime);
    }
}

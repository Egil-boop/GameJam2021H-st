using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Projectile : NetworkBehaviour
{
    [HideInInspector] public float projectileVelocity;
    public float deathTimer = 15f;
    private List<GameObject> collidedEnemies = new List<GameObject>();
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private LayerMask lm;

    public int damage = 0;

    private void Awake()
    {
        if (IsLocalPlayer)
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            lm = gameObject.layer;

            Physics2D.IgnoreLayerCollision(lm, lm);

        }

        Destroy(gameObject, deathTimer);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            rb.velocity = transform.up * projectileVelocity;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerState enemy) && !collidedEnemies.Contains(enemy.gameObject))
        {
            enemy.TakeDamageClientRpc(damage);

            collidedEnemies.Add(enemy.gameObject);
        }

        //projectile death animation
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1f);
    }

    public SpriteRenderer GetSR()
    {
        return sr;
    }
}

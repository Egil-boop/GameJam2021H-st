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

        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        lm = gameObject.layer;

        Physics2D.IgnoreLayerCollision(lm, lm);



        Destroy(gameObject, deathTimer);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

  
    private void FixedUpdate()
    {
        Vector2 newVelocity = transform.up * projectileVelocity;
        rb.velocity = newVelocity;

        Vector2 position = transform.position;
        testServerRpc(position);

    }
    [ServerRpc(RequireOwnership = false)]
    private void testServerRpc(Vector2 newPosition)
    {
        testClientRpc(newPosition);
    }

    [ClientRpc]
    private void testClientRpc(Vector2 newPosition)
    {
        rb.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerState enemy) && !collidedEnemies.Contains(enemy.gameObject))
        {
            enemy.TakeDamageServerRpc(damage);

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

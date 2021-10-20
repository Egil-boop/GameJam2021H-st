using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;
    public Text healthUI;
    public Transform respawnPoint;

    public Color playerColor;
    public int startHealth = 1000;
    private int currentHealth;
    private Rigidbody2D rb;

    public int startLives = 3;
    private int currentLives;

    public float deathFreezeTimer = 2f;
    private float dieTimer;

    private void Start()
    {
        currentHealth = startHealth;
        currentLives = startLives;
        healthUI.color = playerColor;
        healthUI.text = "$" + startHealth;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //for dev use /August
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }
    }

    private void FixedUpdate()
    {
        if(dieTimer > 0f)
        {
            rb.velocity = Vector2.zero;
            dieTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagePopUp.Pop(damage);
        healthUI.text = "$" + currentHealth;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player died!");

        currentLives--;
        if(currentLives <= 0)
        {
            //game over
            Debug.LogWarning("TODO: implement GAME OVER!");
            return;
        }
        else
        {
            currentHealth = startHealth;
        }

        transform.position = respawnPoint.position;
        Debug.LogWarning("TODO: make player invulnerable upon death");

        dieTimer = deathFreezeTimer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;
    public Text healthUI;
    public List<GameObject> livesUI;
    public Transform respawnPoint;

    public Color playerColor;
    public int startHealth = 1000;
    private int currentHealth;
    private Rigidbody2D rb;
    private PlayerMovement pm;

    public int startLives = 3;
    private int currentLives;

    public float deathFreezeTimer = 2f;
    private float dieTimer;

    private void Start()
    {
        currentHealth = startHealth;
        currentLives = startLives;
        healthUI.color = playerColor;
        UpdateUI();

        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
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
        else
        {
            //will break things if used elsewhere /August
            pm.inputFreeze = false;
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

    private void UpdateUI()
    {
        healthUI.text = "$" + startHealth;
        livesUI.RemoveAt(livesUI.Count - 1);
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
            UpdateUI();
        }

        transform.position = respawnPoint.position;
        Debug.LogWarning("TODO: make player invulnerable upon death");

        dieTimer = deathFreezeTimer;
        pm.inputFreeze = true;
        Destroy(livesUI[livesUI.Count - 1]);
    }
}

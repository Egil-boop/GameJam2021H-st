using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;
    private SetPlayerInfo playerInfo;
    private Text healthUI;
    private List<Image> livesUI;
    public Transform respawnPoint;

    public Color playerColor;
    public int startHealth = 1000;
    private int currentHealth;
    private Rigidbody2D rb;
    private PlayerMovement pm;

    public float deathFreezeTimer = 2f;
    private float dieTimer;

    public int startLives = 3;

    [Header("ReadOnly")]
    public int currentLives;

    private void Start()
    {
        playerInfo = GetComponent<SpawnPlayerInfo>().infoInstance;
        healthUI = playerInfo.healthUI;
        livesUI = playerInfo.lives;

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
            TakeDamage(100);
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
    }

    public void Die()
    {
        Debug.Log("Player died!");

        currentLives--;

        //UI lives
        Destroy(livesUI[currentLives]);
        livesUI.RemoveAt(currentLives);

        if (currentLives <= 0)
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
    }
}

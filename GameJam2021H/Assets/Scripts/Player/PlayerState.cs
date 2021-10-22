using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PlayerState : NetworkBehaviour
{
    public DamagePopUp damagePopUp;
    private SetPlayerInfo playerInfo;
    private SpawnPlayerInfo spawnPlayerInfo;
    private Text healthUI;
    private List<Image> livesUI;


    public float respawnRange = 5f;


    public int startHealth = 1000;
    NetworkVariableInt currentHealth;
    private int _currentHealth;

    private Rigidbody2D rb;
    private PlayerMovement pm;

    public float deathFreezeTimer = 2f;


    public int startLives = 3;

    //---
    private float takeDamageTimer = 0.5f;
    private float timer;

    [Header("ReadOnly")]
    public NetworkVariableInt currentLives;
    private int _currentLives;
    public Color playerColor;
    public float dieTimer;

    private void Start()
    {
        // Local player? NOtes: Egil

        currentLives.CanClientWrite(GetComponent<NetworkObject>().OwnerClientId);
        currentHealth.CanClientWrite(GetComponent<NetworkObject>().OwnerClientId);

        spawnPlayerInfo = GetComponent<SpawnPlayerInfo>();
        playerInfo = spawnPlayerInfo.infoInstance;
        playerColor = spawnPlayerInfo.color;
        healthUI = playerInfo.healthUI;
        livesUI = playerInfo.lives;
        _currentHealth = startHealth;
        _currentLives = startLives;

        UpdateHealthServerRpc(_currentHealth);
        currentLives.Value = _currentLives;

        UpdateUIClientRpc();

        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();


    }

    private void Update()
    {

        // Local player? NOtes: Egil
        //for dev use /August
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {

                TakeDamageServerRpc(100);
            }
        }

    }

    private void FixedUpdate()
    {
        // Local player? NOtes: Egil
        if (IsLocalPlayer)
        {
            if (dieTimer > 0f)
            {
                rb.velocity = Vector2.zero;
                dieTimer -= Time.deltaTime;
            }
            else
            {
                //will break things if used elsewhere /August
                pm.inputFreeze = false;
            }

            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        TakeDamageClientRpc(damage);
    }

    [ClientRpc]
    public void TakeDamageClientRpc(int damage)
    {
        if(timer <= 0f)
        {
            if (dieTimer > 0f)
            {
                Debug.Log("Player is invulnerable!");
                return;
            }


            _currentHealth -= damage; // Network health så att man kan uppdatera variablen.
            UpdateHealthServerRpc(_currentHealth);
            timer = takeDamageTimer;
            damagePopUp.Pop(damage); // getComponent Skapar NullPointer.
            UpdateUiServerRpc();

            if (_currentHealth <= 0)
            {
                DieServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateUiServerRpc()
    {
        UpdateUIClientRpc();
    }

    [ClientRpc]
    private void UpdateUIClientRpc()
    {
        //int value = currentHealth.Value;
        int value = _currentHealth;
        healthUI.text = "$" + value;

    }

    [ServerRpc(RequireOwnership = false)]
    public void DieServerRpc()
    {
        DieClientRpc();
    }
    [ClientRpc]
    private void DieClientRpc()
    {
        Debug.Log("Player died!");

        _currentLives--;
        currentLives.Value = _currentLives;

        //UI lives
        Destroy(livesUI[currentLives.Value]);
        livesUI.RemoveAt(currentLives.Value);

        if (currentLives.Value <= 0)
        {
            //game over
            GetComponent<GameOver>().Eliminated();
            return;
        }
        else
        {
            _currentHealth = startHealth;
            UpdateHealthServerRpc(_currentHealth);
            UpdateUiServerRpc();
        }

        transform.position = CalculateRespawnPoint();

        dieTimer = deathFreezeTimer;
        pm.inputFreeze = true;

        // GetComponent<Weapon>().ShootClientRpc();  
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateHealthServerRpc(int health)
    {
        UpdateHealthClientRpc(health);
    }

    [ClientRpc]
    void UpdateHealthClientRpc(int health)
    {
        currentHealth.Value = _currentHealth;
    }

    private Vector3 CalculateRespawnPoint()
    {
        return new Vector3(Random.Range(-respawnRange, respawnRange), 2);
    }
}

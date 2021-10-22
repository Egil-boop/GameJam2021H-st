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


    NetworkVariableInt netWorkHealth = new NetworkVariableInt(1000);
    public int startHealth = 1000;
    private int currentHealth;

    private Rigidbody2D rb;
    private PlayerMovement pm;

    public float deathFreezeTimer = 2f;


    public int startLives = 3;

    [Header("ReadOnly")]
    public int currentLives;
    public Color playerColor;
    public float dieTimer;

    private void Start()
    {
        // Local player? NOtes: Egil

        spawnPlayerInfo = GetComponent<SpawnPlayerInfo>();
        playerInfo = spawnPlayerInfo.infoInstance;
        playerColor = spawnPlayerInfo.color;
        healthUI = playerInfo.healthUI;
        livesUI = playerInfo.lives;
        netWorkHealth.Value = startHealth;
        currentHealth = startHealth;
        currentLives = startLives;

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
        if (dieTimer > 0)
        {
            Debug.Log("Player is invulnerable!");
            return;
        }


        netWorkHealth.Value -= damage; // Network health s� att man kan uppdatera variablen.
        currentHealth = netWorkHealth.Value; // get currentHealth samma v�rde s� att det inte har s�nder resten av koden for now.
        damagePopUp.Pop(damage); // getComponent Skapar NullPointer.
        UpdateUiServerRpc();

        if (currentHealth <= 0)
        {
            DieServerRpc();
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
        healthUI.text = "$" + currentHealth;

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
            netWorkHealth.Value = startHealth;
            currentHealth = netWorkHealth.Value;
            UpdateUiServerRpc();
        }

        transform.position = CalculateRespawnPoint();

        dieTimer = deathFreezeTimer;
        pm.inputFreeze = true;

        // GetComponent<Weapon>().ShootClientRpc();  
    }

    private Vector3 CalculateRespawnPoint()
    {
        return new Vector3(Random.Range(-respawnRange, respawnRange), 2);
    }
}

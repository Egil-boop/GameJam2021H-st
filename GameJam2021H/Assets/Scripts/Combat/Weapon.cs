using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
public class Weapon : NetworkBehaviour
{
    public GameObject projectile;
    public Slider chargeSlider;

    public float projectileSpeed = 10f;
    public float projectileOffset = 2f;
    public float maxCharge = 50f;
    public float chargeRate = 10f;
    public float minCharge = 1f;

    private bool isCharging;
    private PlayerState state;
    private Color color;
    private float attackTimer;
    private float attackSpeed = 1f;

    private LineRenderer lr;
    private Vector2 mousePos;

    [Header("Read Only")]
    public float currentCharge = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            lr = GetComponent<LineRenderer>();
            lr.enabled = false;
            chargeSlider.maxValue = maxCharge;
            state = GetComponent<PlayerState>();
            color = GetComponent<SpawnPlayerInfo>().color;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (attackTimer <= 0f && state.dieTimer <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    isCharging = true;
                    lr.enabled = true;
                }
                else if (Input.GetButtonUp("Fire1") && isCharging)
                {
                    shootServerRpc();
                    ResetShotServerRpc();
                }
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (isCharging)
            {
                DrawSightServerRpc();
            }
        }

      
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            if (isCharging && currentCharge < maxCharge)
            {
                float increment = Time.deltaTime * chargeRate;
                currentCharge += increment;
                chargeSlider.value = currentCharge;
            }

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
       
    }

    [ServerRpc]
    private void shootServerRpc()
    {
        shootServerRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
        Vector3 lookDir = (Vector3)mousePos - transform.localPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion projectileAngle = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        Vector3 shootPosition = transform.localPosition + lookDir;
        shootPosition = transform.localPosition + Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), projectileOffset);

        Projectile instance = Instantiate(projectile, shootPosition, projectileAngle).GetComponent<Projectile>();

        if(currentCharge < minCharge)
        {
            currentCharge = minCharge;
        }

        int damage = Mathf.RoundToInt(currentCharge);
        instance.damage = damage * 2;
        instance.projectileVelocity = projectileSpeed;
        instance.GetSR().color = state.playerColor;

        state.TakeDamageClientRpc(damage);
    }


    [ServerRpc]
    private void ResetShotServerRpc()
    {
        ResetShotClientRpc();
    }

    [ClientRpc]
    public void ResetShotClientRpc()
    {
        isCharging = false;
        currentCharge = 0;
        chargeSlider.value = 0;
        attackTimer = attackSpeed;
        lr.enabled = false;
    }

    [ServerRpc]
    private void DrawSightServerRpc()
    {
        DrawSightClientRpc();
    }


    [ClientRpc]
    private void DrawSightClientRpc()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, mousePos);
    }
}

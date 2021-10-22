using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class Gun : NetworkBehaviour
{
    public TrailRenderer bulletTrail;

    public float projectileOffset = 2f;

    //-----
    // public GameObject projectile;
    public Transform weapon;
    private Vector2 mousePos;
    private Vector3 lookDir;

    public float maxCharge = 50f;
    public float chargeRate = 10f;
    public float minCharge = 1f;

    public float projectileSpeed = 10f;

    private PlayerState state;

    [Header("Read Only")]
    public float currentCharge;

    private void Start()
    {
        state = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            MousePosServerRpc(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            WeaponPosServerRpc();

            //shoot
            if (Input.GetButtonDown("Fire1"))
            {
                //shoot and tell server
                ShootServerRpc();

            }
        }
    }

    //client -> server
    [ServerRpc]
    void ShootServerRpc()
    {
        ShootClientRpc();
    }

    //server -> client
    [ClientRpc]
    void ShootClientRpc()
    {

        

        var bullet = Instantiate(bulletTrail, weapon.position, transform.rotation);
        bullet.AddPosition(weapon.position);

        RaycastHit2D hit = Physics2D.Raycast(weapon.position, lookDir);

        if(hit.collider != null)
        {
            bullet.transform.position = hit.point;
            if(hit.collider.gameObject.TryGetComponent(out PlayerState enemy))
            {
                enemy.TakeDamageServerRpc(10);
            }
        }
        else
        {
            bullet.transform.position = weapon.localPosition + (Vector3)mousePos * 200f;
        }

        /*
        Vector3 lookDir = (Vector3)mousePos - transform.localPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion projectileAngle = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        Vector3 shootPosition = transform.localPosition + lookDir;
        shootPosition = transform.localPosition + Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), projectileOffset);


        Projectile instance = Instantiate(projectile, shootPosition, projectileAngle).GetComponent<Projectile>();


        if (currentCharge < minCharge)
        {
            currentCharge = minCharge;
        }

        int damage = Mathf.RoundToInt(currentCharge);


        instance.damage = damage * 2;
        instance.projectileVelocity = projectileSpeed;
        instance.GetSR().color = state.playerColor;




        state.TakeDamageServerRpc(damage);*/
    }

    [ServerRpc]
    void MousePosServerRpc(Vector2 mouse)
    {
        MousePosClientRpc(mouse);
    }

    [ClientRpc]
    void MousePosClientRpc(Vector2 mouse)
    {
        mousePos = mouse;
    }

    [ServerRpc]
    void WeaponPosServerRpc()
    {
        WeaponPosClientRpc();
    }

    [ClientRpc]
    void WeaponPosClientRpc()
    {
        lookDir = (Vector3)mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion projectileAngle = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        Vector3 shootPosition = transform.position + lookDir;
        shootPosition = transform.position + Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), projectileOffset);

        weapon.position = shootPosition;
    }

    /*
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



        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        chargeSlider.maxValue = maxCharge;
        state = GetComponent<PlayerState>();
        color = GetComponent<SpawnPlayerInfo>().color;









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
                    ShootServerRpc();
                    ResetShotServerRpc();
                }
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (isCharging)
            {
                DrawSight();
            }

        }




    }

    private void FixedUpdate()
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

  
    [ServerRpc]
    public void ShootServerRpc()
    {
        ShootClientRpc();
    }


   [ClientRpc]
    public void ShootClientRpc()
    {
        Vector3 lookDir = (Vector3)mousePos - transform.localPosition;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion projectileAngle = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        Vector3 shootPosition = transform.localPosition + lookDir;
        shootPosition = transform.localPosition + Vector3.ClampMagnitude(new Vector3(lookDir.x, lookDir.y), projectileOffset);

        
        Projectile instance = Instantiate(projectile, shootPosition, projectileAngle).GetComponent<Projectile>();
       

        if (currentCharge < minCharge)
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
        ResetClientRpc();
    }

    [ClientRpc]
    public void ResetClientRpc()
    {
        isCharging = false;
        currentCharge = 0;
        chargeSlider.value = 0;
        attackTimer = attackSpeed;
        lr.enabled = false;
    }





    private void DrawSight()
    {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, mousePos);
      
       
    }*/
}

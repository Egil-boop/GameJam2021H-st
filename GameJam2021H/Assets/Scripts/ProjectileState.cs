using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ProjectileState : NetworkBehaviour
{

    public float damage = 0;
    public Rigidbody2D rb;
    public Vector2 velocity = new Vector2(3, 0);


    private void Awake()
    {
        if (IsLocalPlayer)
        {
            spawnBallServerRpc();

        }
        Destroy(gameObject, 2);

    }

    [ServerRpc]
    private void spawnBallServerRpc()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        
    }

}

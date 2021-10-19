using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileState : MonoBehaviour
{
    
    public float damage = 0;
    public Rigidbody2D rb;
    public Vector2 velocity = new Vector2(3, 0);


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        Destroy(gameObject, 2);
    }


}

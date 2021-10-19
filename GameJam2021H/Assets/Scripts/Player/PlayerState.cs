using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;

    public Color playerColor;
    public int startHealth = 1000;
    private int currentHealth;

    private void Start()
    {
        currentHealth = startHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagePopUp.Pop(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;

    public Color playerColor;
    public int startHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagePopUp.Pop(damage);
    }
}

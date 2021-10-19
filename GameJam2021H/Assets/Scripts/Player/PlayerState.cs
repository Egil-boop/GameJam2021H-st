using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public DamagePopUp damagePopUp;
    public Text healthUI;

    public Color playerColor;
    public int startHealth = 1000;
    private int currentHealth;

    private void Start()
    {
        currentHealth = startHealth;
        healthUI.color = playerColor;
        healthUI.text = "$" + startHealth;
    }

    private void Update()
    {
        //for dev use /August
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagePopUp.Pop(damage);
        healthUI.text = "$" + currentHealth;
    }
}

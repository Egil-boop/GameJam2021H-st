using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text eliminatedText;
    private PlayerState state;
    public float eliminatedTimer = 1f;
    private float timer;

    private void Start()
    {
        state = GetComponent<PlayerState>();
    }

    private void FixedUpdate()
    {
        if(timer > 0)
        {
            eliminatedText.enabled = true;
            timer -= Time.deltaTime;
            Time.timeScale = 0.2f;
        }
        else
        {
            eliminatedText.enabled = false;
            Time.timeScale = 1f;
        }
    }

    public void Eliminated()
    {
        if(state.currentLives <= 0)
        {
            eliminatedText.color = state.playerColor;
            timer = eliminatedTimer;

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class GameOver : MonoBehaviour
{
    public Text eliminatedText;
    public Light2D pointLight;
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
        if(state.currentLives.Value <= 0)
        {
            eliminatedText.color = state.playerColor;
            timer = eliminatedTimer;

            GetComponent<Gun>().weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            pointLight.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerState player))
        {
            if(player.currentLives.Value > 0)
            {
                player.DieServerRpc();
                AudioSource source = GetComponent<AudioSource>();
                source.PlayOneShot(source.clip);
            }
        }
    }
}

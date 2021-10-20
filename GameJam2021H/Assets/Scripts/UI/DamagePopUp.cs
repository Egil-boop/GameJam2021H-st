using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class DamagePopUp : NetworkBehaviour
{

    public PlayerState state;
    public Text[] damagePopUps;

    public float speed = 1f;

    [Range(1, 500)]
    public float duration = 100f;

    private void Start()
    {
        foreach (Text text in damagePopUps)
        {
            text.enabled = false;
        }
    }

    private void FixedUpdate()
    {
      
            foreach (Text text in damagePopUps)
            {
                if (text.enabled)
                {
                    Color disableColor = new Color(text.color.r, text.color.g, text.color.r, 0);

                    text.gameObject.transform.position += new Vector3(0, speed);
                    text.color = Color.Lerp(text.color, disableColor, 1f / duration);

                    //Slow but logical /August
                    if (text.color == disableColor)
                    {
                        text.enabled = false;
                    }
                }
            }
        
    }

    public void Pop(int damage)
    {
        Text textToPop = null;

        Text lowestAlpha = null;

        for (int i = 0; i < damagePopUps.Length; i++)
        {
            if (!damagePopUps[i].enabled)
            {
                textToPop = damagePopUps[i];
                break;
            }

            if (lowestAlpha == null || damagePopUps[i].color.a < lowestAlpha.color.a)
            {
                lowestAlpha = damagePopUps[i];
            }
        }

        if (textToPop == null)
        {
            textToPop = lowestAlpha;
            Debug.Log("Damage Pop Ups are being recycled");
        }

        Vector3 position = Camera.main.WorldToScreenPoint(state.gameObject.transform.position) + new Vector3(Random.Range(-50, 50), 0, 0);

        textToPop.enabled = true;
        textToPop.gameObject.transform.position = position;
        textToPop.text = "-$" + damage;
    }
}

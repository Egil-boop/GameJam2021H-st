using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{

	public PlayerState state;
	public Text[] damagePopUps;

	public float speed = 0.1f;
	public float duration = 1f;
	public Vector3 startPosition;

    private void FixedUpdate()
    {
        foreach(Text text in damagePopUps)
        {
            if (text.enabled)
            {
				Color disableColor = new Color(text.color.r, text.color.g, text.color.r, 0);

				text.gameObject.transform.position += new Vector3(0, speed);
				text.color = Color.Lerp(text.color, disableColor, duration);
            }
        }
    }

    public void Pop(int damage)
	{
		Text textToPop = null;

		for (int i = 0; i < damagePopUps.Length; i++)
		{
			if (!damagePopUps[i].enabled)
			{
				textToPop = damagePopUps[i];
			}
		}

		if(textToPop == null)
        {
			textToPop = damagePopUps[0];
			Debug.Log("Damage Pop Ups are being recycled");
        }

		textToPop.enabled = true;
		textToPop.gameObject.transform.position = startPosition;
		textToPop.color = state.playerColor;
		textToPop.text = "-$" + damage;
	}
}

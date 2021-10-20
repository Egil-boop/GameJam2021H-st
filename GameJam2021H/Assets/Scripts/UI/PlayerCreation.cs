using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreation : MonoBehaviour
{
    public InputField input;

    [Header("Read Only")]
    public string playerName;
    public Color selectedColor = Color.white;

    private void SetPlayerName()
    {
        playerName = input.text.ToUpper();
    }

    public void Red()
    {
        selectedColor = Color.red;
    }

    public void Green()
    {
        selectedColor = Color.green;
    }

    public void Blue()
    {
        selectedColor = Color.blue;
    }

    public void Yellow()
    {
        selectedColor = Color.yellow;
    }

    public void SavePlayer()
    {
        SetPlayerName();

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("PlayerColor", ColorToHex(selectedColor));

        PlayerPrefs.Save();
    }

    private string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}

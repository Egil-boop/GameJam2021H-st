using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayerInfo : MonoBehaviour
{
    public GameObject playerUI;

    [Header("Read Only")]
    public SetPlayerInfo infoInstance;
    public Color color;

    private void Awake()
    {
        infoInstance = Instantiate(playerUI, GameObject.Find("PlayerTextUI").transform).GetComponent<SetPlayerInfo>();

        //infoInstance.playerName.text = PlayerPrefs.GetString("PlayerName");

        // color = HexToColor(PlayerPrefs.GetString("PlayerColor"));

        infoInstance.playerName.color = new Color(Random.Range(0f, 1f),Random.Range(0f,1f), Random.Range(0f,1f));
        //infoInstance.healthUI.color = color;

        foreach (Image i in infoInstance.lives)
        {
            i.color = Color.red;
        }
    }

    public Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}

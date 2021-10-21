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

        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //infoInstance.playerName.text = PlayerPrefs.GetString("PlayerName");

        infoInstance.healthUI.color = color;
        infoInstance.playerName.color = color;
        GetComponent<SpriteRenderer>().color = color;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;

        foreach (Image i in infoInstance.lives)
        {
            i.color = color;
        }
    }
}

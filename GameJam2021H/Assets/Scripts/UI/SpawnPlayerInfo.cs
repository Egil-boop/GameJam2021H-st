using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayerInfo : MonoBehaviour
{
    public GameObject playerUI;

    private PlayerState state;

    [Header("Read Only")]
    public SetPlayerInfo infoInstance;

    private void Awake()
    {
        state = GetComponent<PlayerState>();

        infoInstance = Instantiate(playerUI, GameObject.Find("PlayerTextUI").transform).GetComponent<SetPlayerInfo>();

        infoInstance.playerName.text = "placeholder name";
        infoInstance.playerName.color = state.playerColor;
        infoInstance.healthUI.color = state.playerColor;

        foreach (Image i in infoInstance.lives)
        {
            i.color = state.playerColor;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using MLAPI;
using System.Text;
using System;

public class PasswordNetworkManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private GameObject passwordEntryUI;
    [SerializeField] private GameObject leaveButton;

    private void Start()
    {

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconected;

    }

    private void OnDestroy()
    {

        if (NetworkManager.Singleton == null)
        {
            return;
        }
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconected;
    }

    public void Host()
    {

        NetworkManager.Singleton.ConnectionApprovalCallback += approvalCheck;
        //Man kan skicka in positon till host vart den ska spawna, en vector 3;
        NetworkManager.Singleton.StartHost();

    }


    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();


    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.ConnectionApprovalCallback -= approvalCheck;
        } else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
        changeUIButtonActive(true, false);
      
    }

    private void approvalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {

        string password = Encoding.ASCII.GetString(connectionData);

        bool approveConnection = password == passwordInputField.text;

        callback(true, null, approveConnection, null, null);
    }


    private void HandleClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            changeUIButtonActive(false, true);
        
        }

    }

    private void HandleClientDisconected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            changeUIButtonActive(true, false);
        }
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void changeUIButtonActive(bool passwordEntryUi, bool leaveButton)
    {
        passwordEntryUI.SetActive(passwordEntryUi);
        this.leaveButton.SetActive(leaveButton);
    }
}

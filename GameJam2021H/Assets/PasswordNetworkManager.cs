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

    public void Host()
    {

        NetworkManager.Singleton.ConnectionApprovalCallback += approvalCheck;
        NetworkManager.Singleton.StartHost();

    }


    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();


    }

    public void Leave()
    {

    }

    private void approvalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {

        string password = Encoding.ASCII.GetString(connectionData);

        bool approveConnection = password == passwordInputField.text;

        callback(true, null,approveConnection, null, null);
    }

}

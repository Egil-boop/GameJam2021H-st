using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

                spawnWeaponServerRpc();


            }

        }



    }
    [ServerRpc]
    private void spawnWeaponServerRpc()
    {
        NetworkObject ballInstance = Instantiate(weapon, transform.position, transform.rotation).GetComponent<NetworkObject>();

        ballInstance.SpawnWithOwnership(OwnerClientId);
    }



}

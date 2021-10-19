using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ProjectileState projectileInstance = Instantiate(weapon, transform.position, transform.rotation).GetComponent<ProjectileState>();

        }
        


    }



}

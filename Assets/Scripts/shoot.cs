using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject bulletPref;
    public Transform spawnPoint;
    public Transform bulletRotation;



    public float launchForce;
    public float rateOfFire;




    void Start()
    {
        
    }

    void Update()
    {
        


        if (Input.GetButtonDown("Fire1"))
        {
            GameObject go = Instantiate(bulletPref, spawnPoint.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(-transform.forward * launchForce, ForceMode.Impulse);
            Debug.Log(bulletRotation.transform.rotation.y + "+" + Mathf.Rad2Deg * bulletRotation.transform.rotation.y);
        }
    }
}

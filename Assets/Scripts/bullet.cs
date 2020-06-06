using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Transform bulletRotation;
    void Start()
    {
        bulletRotation = FindObjectOfType<Camera>().gameObject.transform;
        Vector3 relativePos = bulletRotation.position;

        transform.rotation = Quaternion.Slerp(
        transform.rotation,
        Quaternion.LookRotation(relativePos),
        Time.deltaTime * 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

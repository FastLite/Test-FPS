using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float Sensativity;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }
    void CameraRotation()
    {
        yaw += Sensativity * Input.GetAxis("Mouse X");
        pitch -= Sensativity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -80, 100);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
}

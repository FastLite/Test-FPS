using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;
    public float Sensativity;
    public float jumpForce;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    public Rigidbody rigid;

    void Start()
    {
         rigid = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        CameraRotation();
        Jump();
    }

    void CameraRotation()
    {
        yaw += Sensativity * Input.GetAxis("Mouse X");
        pitch -= Sensativity * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
    void Jump()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce( 0,jumpForce, 0, ForceMode.Impulse);
            Debug.Log("игрок должен был прыгнуть");


        }
    }
}

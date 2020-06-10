using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Sensativity;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float walkSpeed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float runSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    void Start()
    {
         
        controller = GetComponentInParent<CharacterController>();

    }

    void Update()
    {
        CameraRotation();
        Move();
    }

    void CameraRotation()
    {
        yaw += Sensativity * Input.GetAxis("Mouse X");
        pitch -= Sensativity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -80, 100);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
    void Move()
    {
        if (controller.isGrounded)
        {
            
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= walkSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }
    
}

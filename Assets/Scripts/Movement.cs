using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float chrouch = 1.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    public bool canMove = true;
    public bool isCrouched = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {

        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;

            moveDirection = (forward * curSpeedX);
            moveDirection =  (right * curSpeedY);


            if (Input.GetButton("Jump") )
            {
                moveDirection.y =  jumpSpeed;
            }
        }
        else if (characterController.isGrounded == false) // Here I independently allow for both X and Z movement. 

        {
            moveDirection.x = Input.GetAxis("Horizontal") * speed;
            moveDirection.z = Input.GetAxis("Vertical") * speed;
            moveDirection = transform.TransformDirection(moveDirection);// Then reassign the current transform to the Vector 3.
        }

        if (Input.GetButton("Crouch"))
        {

            crouch();

        }
        else
        {
            stopCrouching();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveDirection.y = -chrouch;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
    void crouch()
    {
        this.GetComponent(CapsuleCollider).size -= Vector3(0, crouchDeltaHeight, 0);
        this.GetComponent(CapsuleCollider).center -= Vector3(0, crouchDeltaHeight / 2, 0);
        isCrouched = true;
    }
    void stopCrouching()
    {
        isCrouched = false;
        this.GetComponent(BoxCollider).size += Vector3(0, crouchDeltaHeight, 0);
        this.GetComponent(BoxCollider).center += Vector3(0, crouchDeltaHeight / 2, 0);
    }
}
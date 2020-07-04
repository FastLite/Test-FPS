using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject maincamera;

    public GameObject wholeObj;

    public float walkSpeed = 6.0f;

    public float runSpeed = 11.0f;

    [Tooltip("If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster.")]
    [SerializeField]
    public bool limitDiagonalSpeed = true;

    [Tooltip("If checked, the run key toggles between running and walking. Otherwise player runs if the key is held down.")]
    [SerializeField]
    private bool toggleRun = false;

    public float jumpSpeed = 8.0f;

    public float gravity = 20.0f;

    [Tooltip("Units that player can fall before a falling function is run. To disable, type \"infinity\" in the inspector.")]
    [SerializeField]
    private float fallingThreshold = 10.0f;



    [Tooltip("If checked, then the player can change direction while in the air.")]
    [SerializeField]
    private bool airControl = false;


    [Tooltip("Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping.")]
    [SerializeField]
    private int antiBunnyHopFactor = 1;

    public float sensativity;
    private float yaw = 0.0f;
    private float pitch = 0.0f;


    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;
    private CharacterController controller;
    private Transform transf;
    private float speed;
    private float fallStartLevel;
    private bool falling;
    private bool playerControl = false;
    private int jumpTimer;

    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 groundNormal;

    private void Start()
    {

        // Saving component references to improve performance.
        transf = GetComponent<Transform>();
        controller = GetComponentInParent<CharacterController>();

        // Setting initial values.
        speed = walkSpeed;
        jumpTimer = antiBunnyHopFactor;
    }


    private void Update()
    {

        // If the run button is set to toggle, then switch between walk/run speed. (We use Update for this...
        // FixedUpdate is a poor place to use GetButtonDown, since it doesn't necessarily run every frame and can miss the event)
        if (toggleRun && grounded && Input.GetButtonDown("Run"))
        {
            speed = (speed == walkSpeed ? runSpeed : walkSpeed);
        }
    }


    private void FixedUpdate()
    {
        CameraRotation();
        movementFunc();


        if (grounded)
        {

            playerControl = true;

            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (falling)
            {
                falling = false;
                if (transf.position.y < fallStartLevel - fallingThreshold)
                {
                    OnFell(fallStartLevel - transf.position.y);
                }
            }

            // If running isn't on a toggle, then use the appropriate speed depending on whether the run button is down
            if (!toggleRun)
            {
                speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            }





            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
            if (!Input.GetButton("Jump"))
            {
                jumpTimer++;
            }
            else if (jumpTimer >= antiBunnyHopFactor)
            {
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
            }
        }
        else
        {
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!falling)
            {
                falling = true;
                fallStartLevel = transf.position.y;
            }

            
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }


    // This is the place to apply things like fall damage. You can give the player hitpoints and remove some
    // of them based on the distance fallen, play sound effects, etc.
    private void OnFell(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }

    void movementFunc()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //This is all the vector directions from the camera
        camForward = maincamera.transform.forward;
        camRight = maincamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        //This is where the magic happens
        moveDirection = inputY * camForward + inputX * camRight;

        moveDirection = transform.InverseTransformDirection(moveDirection);
        moveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);

        //Sprint system below
        //isInAir is a boolean for the grounded logic
        if (!airControl && Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection *= 5f;
            
            moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);
            
        }
        else
        {
            moveDirection *= 0.5f;
            
            moveDirection.z = Mathf.Clamp(moveDirection.z, -.5f, .5f);
            
        }


        //This is for extra rotation controllable from variables above
        

    }


    void CameraRotation()
    {
        yaw += sensativity * Input.GetAxis("Mouse X");
        pitch -= sensativity * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -80, 100);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
}

//Eric Haines (Eric5h5)
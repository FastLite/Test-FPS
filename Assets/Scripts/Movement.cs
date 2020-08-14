using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    public bool canMove = true;


    CharacterController characterController;
    
    
    [Header("Stance")]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float cameraHeightRatio = 0.9f;
    [Tooltip("Height of character when standing")]
    public float capsuleHeightStanding = 1.8f;
    [Tooltip("Height of character when crouching")]
    public float capsuleHeightCrouching = 0.9f;
    [Tooltip("Speed of crouching transitions")]
    public float crouchingSharpness = 10f;
    
    public UnityAction<bool> onStanceChanged;
    
    public float chrouchSpeedModifier = 0.75f;


    public float defaultSpeed = 7.5f;
    public float gravity = 20.0f;
    public bool isCrouched = false;
    public bool isSliding = false;
    public float jumpSpeed = 8.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    public Camera playerCamera;
    Vector2 rotation = Vector2.zero;
    public float sprintModifier = 1.55f;
    public float crouchModifier = 0.75f;

    public float standardCamHeight;
    public float walkSpeed = 7.5f;
    
    float m_TargetCharacterHeight;
    
    PlayerInputHandler m_InputHandler;
    Actor m_Actor;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        m_Actor = GetComponent<Actor>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        
        walkSpeed = defaultSpeed;
        
        rotation.y = transform.eulerAngles.y;
        

        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);
    }

    void Update()
    {
        HandleCharacterMovement();
    }

    void HandleCharacterMovement()
    {
        bool isSprinting = m_InputHandler.GetSprintInputHeld();

        if (isSprinting)
        {
            isSprinting = SetCrouchingState(false, false);
        }
        float speedModifier = isSprinting ? sprintModifier : 1f;

        if (isSprinting && m_InputHandler.GetCrouchInputDown())
        {
            isSliding = true;
            
        }


        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? walkSpeed * speedModifier * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? walkSpeed* speedModifier * Input.GetAxis("Horizontal") : 0;

            moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed;
            moveDirection.z = Input.GetAxis("Vertical") * walkSpeed;
            moveDirection = transform.TransformDirection(moveDirection);


            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else if (!characterController.isGrounded) // Here I independently allow for both X and Z movement. 

        {
            moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed* speedModifier;
            moveDirection.z = Input.GetAxis("Vertical") * walkSpeed* speedModifier;
            moveDirection =
                transform.TransformDirection(moveDirection); // Then reassign the current transform to the Vector 3.
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

    

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - characterController.radius));
    }


    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            characterController.height = m_TargetCharacterHeight;
            characterController.center = Vector3.up * characterController.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * cameraHeightRatio;
            m_Actor.aimPoint.transform.localPosition = characterController.center;
        }
        // Update smooth height
        else if (characterController.height != m_TargetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            characterController.height = Mathf.Lerp(characterController.height, m_TargetCharacterHeight,
                crouchingSharpness * Time.deltaTime);
            characterController.center = Vector3.up * characterController.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition,
                Vector3.up * m_TargetCharacterHeight * cameraHeightRatio, crouchingSharpness * Time.deltaTime);
            m_Actor.aimPoint.transform.localPosition = characterController.center;
        }
    }

    bool SetCrouchingState(bool crouched, bool ignoreObstructions)
    {
        // set appropriate heights
        if (crouched)
        {
            m_TargetCharacterHeight = capsuleHeightCrouching;
        }
        else
        {
            // Detect obstructions
            if (!ignoreObstructions)
            {
                Collider[] standingOverlaps = Physics.OverlapCapsule(
                    GetCapsuleBottomHemisphere(),
                    GetCapsuleTopHemisphere(capsuleHeightStanding),
                    characterController.radius,
                    -1,
                    QueryTriggerInteraction.Ignore);
                foreach (Collider c in standingOverlaps)
                {
                    if (c != characterController)
                    {
                        return false;
                    }
                }
            }

            m_TargetCharacterHeight = capsuleHeightStanding;
        }

        if (onStanceChanged != null)
        {
            onStanceChanged.Invoke(crouched);
        }

        isCrouched = crouched;
        return true;
    }
    
}
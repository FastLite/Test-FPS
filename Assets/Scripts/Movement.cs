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
    [Tooltip("Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float movementSharpnessOnGround = 15;
    [Tooltip("Acceleration speed when in the air")]
    public float accelerationSpeedInAir = 25f;
    
    public UnityAction<bool> onStanceChanged;
    
    public float chrouchSpeedModifier = 0.75f;

    public Vector3 characterVelocity;
    
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
    public float sprintSpeedModifier = 1.55f;
    public float crouchSpeedModifier = 0.75f;

    public float standardCamHeight;
    public float walkSpeed = 7.5f;
    
    float m_TargetCharacterHeight;
    
    PlayerInputHandler m_InputHandler;
    Actor m_Actor;
    private Vector3 m_LatestImpactSpeed;


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
        
        
        if (m_InputHandler.GetCrouchInputDown())
        {
            
            SetCrouchingState(!isCrouched, false);
            
        }
        UpdateCharacterHeight(false);
        HandleCharacterMovement();
    }

    void HandleCharacterMovement()
    {
        bool isSprinting = m_InputHandler.GetSprintInputHeld();

        if (isSprinting)
        {
            isSprinting = SetCrouchingState(false, false);
        }
        float speedModifier = isSprinting ? sprintSpeedModifier : 1f;

        if (isSprinting && m_InputHandler.GetCrouchInputDown())
        {
            isSliding = true;
            
        }
        
        Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

        if (characterController.isGrounded)
        {
            Vector3 targetVelocity = worldspaceMoveInput * walkSpeed * speedModifier;
            // reduce speed if crouching by crouch speed ratio
            if (isCrouched)
                targetVelocity *= crouchSpeedModifier;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, transform.up) * targetVelocity.magnitude;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);



            if ( m_InputHandler.GetJumpInputDown())
            {
                if (SetCrouchingState(false, false))
                {
                    // start by canceling out the vertical component of our velocity
                    characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                    // then, add the jumpSpeed value upwards
                    characterVelocity += Vector3.up * jumpSpeed;
                  
                }
            }
        }
        else 

        {
            characterVelocity += worldspaceMoveInput * (accelerationSpeedInAir * Time.deltaTime);

            // limit air speed to a maximum, but only horizontally
            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, walkSpeed * speedModifier);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            // apply the gravity to the velocity
            characterVelocity += Vector3.down * gravity * Time.deltaTime;
        }

       
        characterController.Move(characterVelocity * Time.deltaTime);

        Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(characterController.height);
        characterController.Move(characterVelocity * Time.deltaTime);

        m_LatestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, characterController.radius, characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
        {
            // We remember the last impact speed because the fall damage logic might need it
            m_LatestImpactSpeed = characterVelocity;

            characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
        }

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
                    if (c != characterController && !c.CompareTag("weapon"))
                    {
                        return false;
                    }
                }
            }

            m_TargetCharacterHeight = capsuleHeightStanding;
        }

        if (onStanceChanged != null)
        {
            Debug.Log("Chrouch on stance" + crouched );
            onStanceChanged.Invoke(crouched);
        }

        isCrouched = crouched;
        Debug.Log("Chrouch " + isCrouched );
        return true;
        
    }
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }
    
}
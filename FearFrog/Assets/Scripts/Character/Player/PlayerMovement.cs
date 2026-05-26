using System;
using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement member variables
    [SerializeField] private float m_walkAcceleration = 30f;
    private float m_maxWalkAirVelocity = 1.2f;
    [SerializeField] private float m_sprintAcceleration = 45f;
    private float m_maxSprintAirVelocity = 6f;
    [SerializeField] private float m_crouchAcceleration = 18f;
    private float m_currMoveAcceleration;
    private float m_currMaxAirVelocity;
    
    // Look member variables
    [SerializeField] private float m_cameraSensitivity = 100f;
    private float m_xOritation = 0; // Record of player look direction
    private float m_yOritation = 0;
    
    // Jump member variables
    [SerializeField] private float m_jumpAcceleration = 320f;
    
    // Crouch height change member variables
    private float m_stepUpHeight = 0.8f;
    private float m_standCameraHeight = 1.75f;
    private float m_crouchCameraHeight = 0.05f;
    private float m_crouchShrinkRatio = 0.4f;
    

    // Start
    void Start()
    {
        // Variable initialization
        m_currMoveAcceleration = m_walkAcceleration;
        m_currMaxAirVelocity = m_maxWalkAirVelocity;
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Link jump, sprint, and crouch functionality
        InputController.Instance.Input.Player.Jump.performed += Jump;
        InputController.Instance.Input.Player.Sprint.performed += ToggleSprint;
        InputController.Instance.Input.Player.Crouch.performed += ToggleCrouch;
    }
    
    // Update
    void Update()
    {
        // Player Look
        PlayerLook();
    }
    
    // FixedUpdate
    private void FixedUpdate()
    {
        // Sprint stop check
        SprintStopCheck();
        
        // Player move
        PlayerMove();
        if (!PlayerController.Instance.IsGrounded)
        {
            VelocityControl();
        }
    }
    
    
    // Handle player looking around
    private void PlayerLook()
    {
        Vector2 lookDirection = InputController.Instance.Input.Player.Look.ReadValue<Vector2>();
        m_xOritation += lookDirection.x * m_cameraSensitivity * Time.deltaTime;
        m_yOritation += lookDirection.y * m_cameraSensitivity * Time.deltaTime;
        m_yOritation = Math.Clamp(m_yOritation, -78f, 85f);
        
        PlayerController.Instance.CameraContainer.rotation = Quaternion.Euler(-m_yOritation, m_xOritation, 0f);
        PlayerController.Instance.PlayerEntityContainer.rotation = Quaternion.Euler(0f, m_xOritation, 0f);
    }
    
    // Handle player movement
    private void PlayerMove()
    {
        // Calculate new input move direction
        Vector2 input = InputController.Instance.Input.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        moveDirection = (Quaternion.Euler(0f, m_xOritation, 0f) * moveDirection).normalized;

        // Modify input move direction to be parallel to the ground if player is grounded
        float cos;
        if (PlayerController.Instance.IsGrounded)
        {
            cos = Vector3.Dot(moveDirection, PlayerController.Instance.GroundHit.normal);       // Cosine of angle between new move direction and normal of surface
            float degTheta = Mathf.Acos(cos) * Mathf.Rad2Deg;       // Angle between new move direction and normal of surface in degrees
            degTheta -= 90;
            
            moveDirection = Quaternion.AngleAxis(degTheta, Vector3.Cross(moveDirection, Vector3.up)) * moveDirection;
            moveDirection = moveDirection.normalized;
        }
        
        PlayerController.Instance.PlayerRb.AddForce(moveDirection * m_currMoveAcceleration, ForceMode.Acceleration);
    }

    // Contorl player's horizontal velocity when in the air
    private void VelocityControl()
    {
        Vector3 currHorVelocity = PlayerController.Instance.PlayerRb.linearVelocity;
        currHorVelocity.y = 0f;
        if (currHorVelocity.magnitude > m_currMaxAirVelocity)
        {
            currHorVelocity = currHorVelocity.normalized * m_currMaxAirVelocity;
            // Player's fall down speed should not be affected by velocity control
            currHorVelocity.y = PlayerController.Instance.PlayerRb.linearVelocity.y;
            PlayerController.Instance.PlayerRb.linearVelocity = currHorVelocity;
        }
    }
    
    
    // Perform player jump action
    private void Jump(InputAction.CallbackContext ctx)
    {
        // Perform jump is player is grounded
        if (PlayerController.Instance.IsGrounded && !PlayerController.Instance.IsJumping)
        {
            StopCrouching();
            PlayerController.Instance.PlayerRb.AddForce(new Vector3(0f, m_jumpAcceleration, 0f), ForceMode.Acceleration);
            PlayerController.Instance.IsGrounded = false;
            // Update jumping state
            PlayerController.Instance.IsJumping = true;
            StartCoroutine(ResetJumping(0.04f));
        }
    }
    
    private IEnumerator ResetJumping(float targetTime)      // Reset player jumping state after a short time
    {
        float timer = 0f;
        while (timer < targetTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayerController.Instance.IsJumping = false;
    }
    
    
    // Toggle player sprint
    private void ToggleSprint(InputAction.CallbackContext ctx)
    {
        if (PlayerController.Instance.IsGrounded)   // Only allow toggling when player's grounded
        {
            if (PlayerController.Instance.IsSprinting)      // Stop sprinting
            {
                StopSprinting();
            }
            else                                        // Start sprinting
            {
                StopCrouching();
                StartSprinting();
            }
        }
    }

    private void StartSprinting()   // Start sprinting
    {
        if (!PlayerController.Instance.IsSprinting)
        {
            PlayerController.Instance.IsSprinting = true;
            m_currMoveAcceleration = m_sprintAcceleration;
            m_currMaxAirVelocity = m_maxSprintAirVelocity;
            // Invoke event
            PlayerController.Instance.TriggerOnStartSprinting();
        }
    }

    private void StopSprinting()    // Stop sprinting
    {
        if (PlayerController.Instance.IsSprinting)
        {
            PlayerController.Instance.IsSprinting = false;
            m_currMoveAcceleration = m_walkAcceleration;
            m_currMaxAirVelocity = m_maxWalkAirVelocity;
            // Invoke event
            PlayerController.Instance.TriggerOnBackToWalking();
        }
    }
    
    // Exit sprint if player stops moving
    private void SprintStopCheck()
    {
        if (PlayerController.Instance.IsSprinting && PlayerController.Instance.PlayerRb.linearVelocity.magnitude < 0.0001f)
        {
            StopSprinting();
            // Invoke event
            PlayerController.Instance.TriggerOnBackToWalking();
        }
    }
    
    
    // Toggle player crouch
    private void ToggleCrouch(InputAction.CallbackContext ctx)
    {
        if (PlayerController.Instance.IsGrounded) // Only allow toggling when player's grounded
        {
            if (PlayerController.Instance.IsCrouching)      // Stop crouching
            {
                StopCrouching();
            }
            else                    // Start crouching
            {
                StopSprinting();
                StartCrouching();
            }
        }
    }

    private void StartCrouching()   // Start crouching
    {
        if (!PlayerController.Instance.IsCrouching)
        {
            PlayerController.Instance.IsCrouching = true;
            m_currMoveAcceleration = m_crouchAcceleration;
            // Update player entity and camera
            StartCoroutine(CrounchCameraChange(new Vector3(0f, m_crouchCameraHeight, 0f)));
            float localScaleY = (2f * (1f - m_crouchShrinkRatio) - m_stepUpHeight) / 2f;
            float localPosY = m_stepUpHeight + localScaleY - 1f;
            PlayerController.Instance.PlayerEntity.localScale = new Vector3(1f, localScaleY, 1f);
            PlayerController.Instance.PlayerEntity.localPosition = new Vector3(0f, localPosY, 0f);
            
            // Invoke event
            PlayerController.Instance.TriggerOnStartCrouching();
        }
    }

    private void StopCrouching()    // Stop crouching
    {
        if (PlayerController.Instance.IsCrouching)
        {
            PlayerController.Instance.IsCrouching = false;
            m_currMoveAcceleration = m_walkAcceleration;
            // Update player entity and camera
            StartCoroutine(CrounchCameraChange(new Vector3(0f, m_standCameraHeight, 0f)));
            PlayerController.Instance.PlayerEntity.localScale = new Vector3(1f, 1f - m_stepUpHeight / 2f, 1f);
            PlayerController.Instance.PlayerEntity.localPosition = new Vector3(0f, m_stepUpHeight / 2f, 0f);
            
            // Invoke event
            PlayerController.Instance.TriggerOnBackToWalking();
        }
    }

    // Smooth transition of main camera when toggling crounch
    private IEnumerator CrounchCameraChange(Vector3 targetPos)
    {
        while (Vector3.Distance(PlayerController.Instance.CameraContainer.localPosition, targetPos) > 0.01f)
        {
            PlayerController.Instance.CameraContainer.localPosition =
                Vector3.Lerp(PlayerController.Instance.CameraContainer.localPosition, targetPos, 0.12f);
            yield return null;
        }
    }
}

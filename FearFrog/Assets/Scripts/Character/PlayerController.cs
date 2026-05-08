using System;
using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement member variables
    [SerializeField] private float m_walkAcceleration = 30f;
    [SerializeField] private float m_maxWalkAirVelocity = 1.5f;
    [SerializeField] private float m_sprintAcceleration = 45f;
    [SerializeField] private float m_maxSprintAirVelocity = 3.5f;
    [SerializeField] private float m_crouchAcceleration = 20f;
    private float m_currMoveAcceleration;
    private float m_currMaxAirVelocity;
    
    // Look member variables
    [SerializeField] private float m_cameraSensitivity = 100f;
    private float m_xOritation = 0; // Record of player look direction
    private float m_yOritation = 0;
    
    // Jump member variables
    [SerializeField] private float m_jumpAcceleration = 250f;
    
    // Crouch height change member variables
    private float m_standCameraHeight = 0.85f;
    private float m_crouchCameraHeight = -0.1f;
    private float m_crouchShrinkRatio = 0.5f;
    

    // Start
    void Start()
    {
        // Default movement speed to walk
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
        if (!PlayerStatus.Instance.IsGrounded)
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
        m_yOritation = Math.Clamp(m_yOritation, -90f, 90f);
        
        PlayerStatus.Instance.Camera.rotation = Quaternion.Euler(-m_yOritation, m_xOritation, 0f);
        PlayerStatus.Instance.PlayerEntity.rotation = Quaternion.Euler(0f, m_xOritation, 0f);
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
        if (PlayerStatus.Instance.IsGrounded)
        {
            cos = Vector3.Dot(moveDirection, PlayerStatus.Instance.GroundHit.normal);       // Cosine of angle between new move direction and normal of surface
            float degTheta = Mathf.Acos(cos) * Mathf.Rad2Deg;       // Angle between new move direction and normal of surface in degrees
            degTheta -= 90;
            
            moveDirection = Quaternion.AngleAxis(degTheta, Vector3.Cross(moveDirection, Vector3.up)) * moveDirection;
            moveDirection = moveDirection.normalized;
        }
        
        PlayerStatus.Instance.PlayerRb.AddForce(moveDirection * m_currMoveAcceleration, ForceMode.Acceleration);
    }

    // Contorl player's horizontal velocity when in the air
    private void VelocityControl()
    {
        Vector3 currHorVelocity = PlayerStatus.Instance.PlayerRb.linearVelocity;
        currHorVelocity.y = 0f;
        if (currHorVelocity.magnitude > m_currMaxAirVelocity)
        {
            currHorVelocity = currHorVelocity.normalized * m_currMaxAirVelocity;
            // Player's fall down speed should not be affected by velocity control
            currHorVelocity.y = PlayerStatus.Instance.PlayerRb.linearVelocity.y;
            PlayerStatus.Instance.PlayerRb.linearVelocity = currHorVelocity;
        }
    }
    
    
    // Perform player jump action
    private void Jump(InputAction.CallbackContext ctx)
    {
        // Perform jump is player is grounded
        if (PlayerStatus.Instance.IsGrounded && !PlayerStatus.Instance.IsJumping)
        {
            // StopCrouching();
            PlayerStatus.Instance.PlayerRb.AddForce(new Vector3(0f, m_jumpAcceleration, 0f), ForceMode.Acceleration);
            PlayerStatus.Instance.IsGrounded = false;
            // Update jumping state
            PlayerStatus.Instance.IsJumping = true;
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
        PlayerStatus.Instance.IsJumping = false;
    }
    
    
    // Toggle player sprint
    private void ToggleSprint(InputAction.CallbackContext ctx)
    {
        if (PlayerStatus.Instance.IsGrounded)   // Only allow toggling when player's grounded
        {
            if (PlayerStatus.Instance.IsSprinting)      // Stop sprinting
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
        if (!PlayerStatus.Instance.IsSprinting)
        {
            PlayerStatus.Instance.IsSprinting = true;
            m_currMoveAcceleration = m_sprintAcceleration;
            m_currMaxAirVelocity = m_maxSprintAirVelocity;
        }
    }

    private void StopSprinting()    // Stop sprinting
    {
        if (PlayerStatus.Instance.IsSprinting)
        {
            PlayerStatus.Instance.IsSprinting = false;
            m_currMoveAcceleration = m_walkAcceleration;
            m_currMaxAirVelocity = m_maxWalkAirVelocity;   
        }
    }
    
    // Exit sprint if player stops moving
    private void SprintStopCheck()
    {
        if (PlayerStatus.Instance.IsSprinting && PlayerStatus.Instance.PlayerRb.linearVelocity.magnitude < 0.0001f)
        {
            StopSprinting();
        }
    }
    
    
    // Toggle player crouch
    private void ToggleCrouch(InputAction.CallbackContext ctx)
    {
        if (PlayerStatus.Instance.IsGrounded) // Only allow toggling when player's grounded
        {
            if (PlayerStatus.Instance.IsCrounching)      // Stop crouching
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
        if (!PlayerStatus.Instance.IsCrounching)
        {
            PlayerStatus.Instance.IsCrounching = true;
            m_currMoveAcceleration = m_crouchAcceleration;
            // Update player entity and camera
            // StartCoroutine(CrounchCameraChange(new Vector3(0f, m_crouchCameraHeight, 0f)));
            // m_playerEntity.localScale = new Vector3(1f, m_crouchShrinkRatio, 1f);
            // m_playerEntity.localPosition = new Vector3(0f, m_crouchShrinkRatio - 1f, 0f);
        }
    }

    private void StopCrouching()    // Stop crouching
    {
        if (PlayerStatus.Instance.IsCrounching)
        {
            PlayerStatus.Instance.IsCrounching = false;
            m_currMoveAcceleration = m_walkAcceleration;
            // Update player entity and camera
            // StartCoroutine(CrounchCameraChange(new Vector3(0f, m_standCameraHeight, 0f)));
            // m_playerEntity.localScale = new Vector3(1f, 1f, 1f);
            // m_playerEntity.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    // Smooth transition of main camera when toggling crounch
    private IEnumerator CrounchCameraChange(Vector3 targetPos)
    {
        yield return null;
    }
}

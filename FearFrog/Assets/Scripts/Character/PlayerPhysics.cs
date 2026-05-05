using System;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // Grounded check variables
    private float m_gcRadius = 0.35f;       // Grounndeed check shpere radius
    [SerializeField] private float m_groundDrag = 5f;
    [SerializeField] private float m_airDrag = 0f;
    
    // Gravity & Dynamic friction member variables
    private float m_playerWeight;
    [SerializeField] private float m_gravity = 9.81f;
    private float m_dfCoef = 0.4f;          // Dynamic friction coefficient
    [SerializeField] private float m_stepUpHeight = 0.4f;       // Player floating height
    [SerializeField] private float m_stepDownHeight = 0.2f;     // Player step down distance
    
    
    // Start
    private void Start()
    {
        // Initilization
        m_playerWeight = PlayerStatus.Instance.PlayerRb.mass;
        
        // Set player entity scale and position
        PlayerStatus.Instance.PlayerEntity.localScale = new Vector3(1f, 1 - m_stepUpHeight / 2f, 1f);
        PlayerStatus.Instance.PlayerEntity.localPosition = new Vector3(0f, m_stepUpHeight / 2f, 0f);
    }
    
    // FixedUpdate
    private void FixedUpdate()
    {
        // Ground check
        GroundCheck();
        
        // Player physics
        if (PlayerStatus.Instance.IsGrounded)       // Player on ground
        {
            // Apply dynamic friction
            if (PlayerStatus.Instance.PlayerRb.linearVelocity.magnitude > 0.001f)
            {
                ApplyGroundFriction();
            }

            // Maintain player floating
            MaintainFloat();
        }
        else                                        // Player in air
        {
            // Apply gravity force
            ApplyGravity();
        }
    }
    
    
    // Check whether the player is on the ground
    private void GroundCheck()
    {
        PlayerStatus.Instance.IsGroundedPrev = PlayerStatus.Instance.IsGrounded;
        PlayerStatus.Instance.IsGrounded = Physics.SphereCast(transform.position, m_gcRadius, 
            Vector3.down, out PlayerStatus.Instance.GroundHit, -PlayerStatus.Instance.FootPos.localPosition.y);
        
        PlayerStatus.Instance.PlayerRb.linearDamping = PlayerStatus.Instance.IsGrounded ? m_groundDrag : m_airDrag;
        // Update vertical velocity to 0 when just landed on ground
        if (PlayerStatus.Instance.IsGrounded && !PlayerStatus.Instance.IsGroundedPrev)
        {
            Vector3 newVelocity = PlayerStatus.Instance.PlayerRb.linearVelocity;
            newVelocity.y = 0f;
            PlayerStatus.Instance.PlayerRb.linearVelocity = newVelocity;
        }
        
        // Prevent player from free falling from edges of low heights
        if (!PlayerStatus.Instance.IsGrounded && PlayerStatus.Instance.IsGroundedPrev)
        {
            if (!PlayerStatus.Instance.IsJumping)
            {
                // Second ground check
                PlayerStatus.Instance.IsGrounded = Physics.SphereCast(transform.position, m_gcRadius, 
                    Vector3.down, out PlayerStatus.Instance.GroundHit, 
                    -PlayerStatus.Instance.FootPos.localPosition.y + m_stepDownHeight);
                
                PlayerStatus.Instance.PlayerRb.linearDamping = PlayerStatus.Instance.IsGrounded ? m_groundDrag : m_airDrag;
            }
        }
    }
    
    // Apply gravity manually
    private void ApplyGravity()
    {
        PlayerStatus.Instance.PlayerRb.AddForce(m_gravity * Vector3.down, ForceMode.Acceleration);
    }
    
    // Apply a friction while the player is moving on ground
    private void ApplyGroundFriction()
    {
        // Calculate the normal force applied by the ground
        float cos = Vector3.Dot(-PlayerStatus.Instance.GroundHit.normal, Vector3.down);    // Cosine of angle between gravity force and normal force
        Vector3 gravityNormal = cos * m_playerWeight * m_gravity * (-PlayerStatus.Instance.GroundHit.normal);
        Vector3 frictionForce = m_dfCoef * gravityNormal.magnitude * -PlayerStatus.Instance.PlayerRb.linearVelocity.normalized;
        
        PlayerStatus.Instance.PlayerRb.AddForce(frictionForce, ForceMode.Acceleration);
    }
    
    // Maintain player entity's floating status; also for player stair up functionality
    private void MaintainFloat()
    {
        // Readjust player height
        Vector3 playerCenter = transform.position;
        float stepUpAmount = PlayerStatus.Instance.GroundHit.point.y + 1 - transform.position.y;
        // Adjustment for ground point NOT exactly below the player's center
        playerCenter.y = PlayerStatus.Instance.GroundHit.point.y;
        float d = Vector3.Distance(PlayerStatus.Instance.GroundHit.point, playerCenter);
        float r = m_gcRadius;
        float adjust = r - Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(d, 2));
        stepUpAmount -= adjust;
        if (Mathf.Abs(stepUpAmount) >= 0.001f)
        {
            transform.Translate(stepUpAmount * Vector3.up);   
        }
    }
    
    
    // DEBUG USE
    private void OnDrawGizmos()
    {
        // Ground check
        Gizmos.color = Color.blue;
        Vector3 drawPos = transform.position;
        drawPos.y += PlayerStatus.Instance.FootPos.localPosition.y;
        Gizmos.DrawSphere(drawPos, m_gcRadius);

        // Grounded point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(PlayerStatus.Instance.GroundHit.point, 0.07f);
    }
}
















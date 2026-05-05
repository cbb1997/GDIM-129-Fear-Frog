using System;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // Member variables
    private float m_playerWeight;
    [SerializeField] private float m_gravity = 9.81f;
    private float m_dfCoef = 0.4f; // Dynamic friction coefficient
    [SerializeField] private float m_stepHeight = 0.4f;
    
    
    // Start
    private void Start()
    {
        // Initilization
        m_playerWeight = PlayerStatus.Instance.PlayerRb.mass;
        
        // Set player entity scale and position
        PlayerStatus.Instance.PlayerEntity.localScale = new Vector3(1f, 1 - m_stepHeight / 2f, 1f);
        PlayerStatus.Instance.PlayerEntity.localPosition = new Vector3(0f, m_stepHeight / 2f, 0f);
    }
    
    // FixedUpdate
    private void FixedUpdate()
    {
        // Check to apply gravity and friction forces
        if (PlayerStatus.Instance.IsGrounded)       // Player on ground
        {
            // Apply dynamic friction
            if (PlayerStatus.Instance.PlayerRb.linearVelocity.magnitude > 0.001f)
            {
                ApplyGroundFriction();
            }
            
            // Readjust player height
            Vector3 playerCenter = transform.position;
            float stepUpAmount = PlayerStatus.Instance.GroundHit.point.y + 1 - transform.position.y;
            // Adjustment for ground point NOT exactly below the player's center
            playerCenter.y = PlayerStatus.Instance.GroundHit.point.y;
            float d = Vector3.Distance(PlayerStatus.Instance.GroundHit.point, playerCenter);
            float r = PlayerStatus.Instance.GcRadius;
            float adjust = r - Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(d, 2));
            stepUpAmount -= adjust;
            if (stepUpAmount >= 0.001f)
            {
                Debug.Log("Adjust applied");
                transform.Translate(stepUpAmount * Vector3.up);   
            }
        }
        else                                        // Player in air
        {
            ApplyGravity();
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
        float cos = Vector3.Dot(-PlayerStatus.Instance.GroundNormal, Vector3.down);    // Cosine of angle between gravity force and normal force
        Vector3 gravityNormal = cos * m_playerWeight * m_gravity * (-PlayerStatus.Instance.GroundNormal);
        Vector3 frictionForce = m_dfCoef * gravityNormal.magnitude * -PlayerStatus.Instance.PlayerRb.linearVelocity.normalized;
        
        PlayerStatus.Instance.PlayerRb.AddForce(frictionForce, ForceMode.Acceleration);
    }
}

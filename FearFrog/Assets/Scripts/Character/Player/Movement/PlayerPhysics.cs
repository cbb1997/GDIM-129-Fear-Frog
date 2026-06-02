using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerPhysics : MonoBehaviour
{
    // Grounded check variables
    private float m_gcRadius = 0.3f;       // Grounded check shpere radius
    private float m_groundDrag = 5f;
    private float m_airDrag = 0f;
    // [SerializeField] [Range(0f, 90f)] private float m_maxGroundAngle = 45f;
    
    // Gravity & Dynamic friction member variables
    private float m_playerWeight;
    private float m_playerScaleY;
    private float m_gravity = 9.81f * 2.5f;
    private float m_dfCoef = 0.4f;          // Dynamic friction coefficient
    private float m_stepUpHeight;       // Player floating height
    private float m_stepDownHeight;    // Player step down distance
    
    
    // Start
    private void Start()
    {
        // Initilization
        m_playerWeight = PlayerController.Instance.PlayerRb.mass;
        m_playerScaleY = PlayerController.Instance.PlayerEntity.localScale.y;

        m_gcRadius = 0.3f * PlayerController.Instance.PlayerScale;
        m_stepUpHeight = 0.7f;
        m_stepDownHeight = 0.35f;
        PlayerController.Instance.FootPos.localPosition = 
            new Vector3(0f, -1.6f * PlayerController.Instance.PlayerScale - 0.05f + m_gcRadius, 0f);
        
        // Set player entity scale and position
        PlayerController.Instance.PlayerEntity.localScale = 
            new Vector3(1f, m_playerScaleY - m_stepUpHeight / 2f, 1f);
        PlayerController.Instance.PlayerEntity.localPosition = 
            new Vector3(0f, m_stepUpHeight / 2f, 0f);
    }
    
    // FixedUpdate
    private void FixedUpdate()
    {
        // Ground check
        GroundCheck();
        
        // Player physics
        if (PlayerController.Instance.IsGrounded)       // Player on ground
        {
            // Apply dynamic friction
            if (PlayerController.Instance.PlayerRb.linearVelocity.magnitude > 0.1f)
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
    private Vector3 footStepPos;
    private RaycastHit testHit;
    private void GroundCheck()
    {
        RaycastHit temp = PlayerController.Instance.GroundHit;
        
        // Save previous grounded status
        PlayerController.Instance.IsGroundedPrev = PlayerController.Instance.IsGrounded;
        
        // Ground check
        RaycastHit[] hitArr;
        if (PlayerController.Instance.IsJumping)
        {
            // Prevent failing to jump up
            PlayerController.Instance.IsGrounded = false;
        }
        else
        {
            float gcHeightAdjust = (PlayerController.Instance.IsGroundedPrev) ? m_stepDownHeight : 0f;
            int mask = LayerMask.NameToLayer("Player");     // Exclude player layer from checking
            mask = ~(1 << mask);
            hitArr = Physics.SphereCastAll(transform.position, m_gcRadius, Vector3.down, 
                -PlayerController.Instance.FootPos.localPosition.y + gcHeightAdjust, mask);
            hitArr = hitArr.OrderBy(x => x.distance).ToArray();
            
            PlayerController.Instance.IsGrounded = false;   // Set player grounded to false by default for the case no ground is found
            for (int i = 0; i < hitArr.Length; i++)
            {
                // Check ground besides isTrigger objects
                if (!hitArr[i].collider.isTrigger)
                {
                    PlayerController.Instance.GroundHit = hitArr[i];
                    PlayerController.Instance.IsGrounded = true;
                    break;
                }
            }
        }
        
        // Update status
        PlayerController.Instance.PlayerRb.linearDamping = PlayerController.Instance.IsGrounded ? m_groundDrag : m_airDrag;
        // Update vertical velocity to 0 when just landed on ground
        if (PlayerController.Instance.IsGrounded && !PlayerController.Instance.IsGroundedPrev)
        {
            Vector3 newVelocity = PlayerController.Instance.PlayerRb.linearVelocity;
            newVelocity.y = 0f;
            PlayerController.Instance.PlayerRb.linearVelocity = newVelocity;
        }
    }
    
    // Calculate the angle between the given vector and the horizontal plane
    private float GetGroundAngle(Vector3 groundNormal)
    {
        Vector3 horizontal = groundNormal;
        horizontal.y = 0f;
        horizontal = horizontal.normalized;

        float cos = Vector3.Dot(groundNormal, horizontal);
        return (90f - Mathf.Acos(cos) * Mathf.Rad2Deg);
    }
    
    // Apply gravity manually
    private void ApplyGravity()
    {
        PlayerController.Instance.PlayerRb.AddForce(m_gravity * Vector3.down, ForceMode.Acceleration);
    }
    
    // Apply a friction while the player is moving on ground
    private void ApplyGroundFriction()
    {
        // Calculate the normal force applied by the ground
        float cos = Vector3.Dot(-PlayerController.Instance.GroundHit.normal, Vector3.down);    // Cosine of angle between gravity force and normal force
        Vector3 gravityNormal = cos * m_playerWeight * m_gravity * (-PlayerController.Instance.GroundHit.normal);
        Vector3 frictionForce = m_dfCoef * gravityNormal.magnitude * -PlayerController.Instance.PlayerRb.linearVelocity.normalized;
        
        PlayerController.Instance.PlayerRb.AddForce(frictionForce, ForceMode.Acceleration);
    }
    
    // Maintain player entity's floating status; also for player stair up functionality
    private void MaintainFloat()
    {
        // Readjust player height
        Vector3 playerCenter = transform.position;
        float stepUpAmount = PlayerController.Instance.GroundHit.point.y + m_playerScaleY * PlayerController.Instance.PlayerScale - transform.position.y;
        // Adjustment for ground point NOT exactly below the player's center
        playerCenter.y = PlayerController.Instance.GroundHit.point.y;
        float d = Vector3.Distance(PlayerController.Instance.GroundHit.point, playerCenter);
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
        if (PlayerController.Instance != null)
        {
            // Ground check
            Gizmos.color = Color.blue;
            Vector3 drawPos = transform.position;
            drawPos.y += PlayerController.Instance.FootPos.localPosition.y;
            Gizmos.DrawSphere(drawPos, m_gcRadius);
            
            // Grounded point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(PlayerController.Instance.GroundHit.point, 0.05f);
        }
    }
}
















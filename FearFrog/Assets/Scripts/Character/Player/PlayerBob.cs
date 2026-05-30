using UnityEngine;

public class PlayerBob : MonoBehaviour
{
    // Statistic member variables
    [SerializeField] protected bool m_bobEnabled = true;
    private float m_currFrequency;
    private float m_currMagModifier;
    private float m_walkFrequency = 8f;        // Bob effect frequency
    private float m_sprintFrequency = 16f;
    private float m_crouchFrequency = 5f;
    private float m_walkMagModifier = 0.2f;   // Modifier for maginitude of bob effect
    private float m_sprintMagModifier = 0.6f;
    private float m_crouchMagModifier = 0.25f;
    private float m_amplitude = 0.001f;
    private float m_toggleSpeed = 0.3f;     // Speed threshold for whether apply bob effect
    
    private Vector3 m_startPos;
    private float m_timer = 0f;
    
    // Inheritance member variables
    [SerializeField] protected Transform m_bobObj;
    [SerializeField] protected float m_xMagnitude;
    [SerializeField] protected float m_yMagnitude;
    
    
    // Start
    void Start()
    {
        // Varialbe initialization
        m_startPos = m_bobObj.localPosition;
        m_currFrequency = m_walkFrequency;
        m_currMagModifier = m_walkMagModifier;
        
        // Link events
        PlayerController.Instance.OnStartSprinting += SetSprintBob;
        PlayerController.Instance.OnStartCrouching += SetCrouchBob;
        PlayerController.Instance.OnBackToWalking += SetWalkBob;
    }

    // Update
    protected virtual void Update()
    {
        if (!m_bobEnabled) return;

        // Check to play camera bob effect
        Vector3 horiVelocity = PlayerController.Instance.PlayerRb.linearVelocity;
        horiVelocity.y = 0f;
        if (!PlayerController.Instance.IsGrounded || horiVelocity.magnitude < m_toggleSpeed)
        {
            StopBob();
        }
        else
        {
            PerformBob();
        }
    }

    // Play bob motion on the camera
    private void PerformBob()
    {
        // Calculate and perform bob offset
        m_timer += Time.deltaTime;
        Vector3 offset = new Vector3();
        offset.x = (m_xMagnitude * m_amplitude * m_currMagModifier) * Mathf.Cos((m_currFrequency / 2f) * m_timer);
        offset.y = (-m_yMagnitude * m_amplitude * m_currMagModifier) * 0.25f *
                   (3 * Mathf.Sin(m_currFrequency * m_timer) * Mathf.Pow(1f - Mathf.Cos(m_currFrequency * m_timer), 2));
        
        m_bobObj.localPosition += offset;
    }
    
    // Change back to walking bob settings
    private void SetWalkBob()
    {
        // Update setting
        m_currFrequency = m_walkFrequency;
        m_currMagModifier = m_walkMagModifier;
        ResetCamera();
    }

    // Change to sprinting bob settings
    private void SetSprintBob()
    {
        // Update setting
        m_currFrequency = m_sprintFrequency;
        m_currMagModifier = m_sprintMagModifier;
        ResetCamera();
    }
    
    // Change to crouching bob settings
    private void SetCrouchBob()
    {
        // Update setting
        m_currFrequency = m_crouchFrequency;
        m_currMagModifier = m_crouchMagModifier;
        ResetCamera();
    }

    // Check to reset camera location back to start position
    // on player movement state change
    private void ResetCamera()
    {
        Vector3 horiVelocity = PlayerController.Instance.PlayerRb.linearVelocity;
        horiVelocity.y = 0f;
        if (horiVelocity.magnitude >= m_toggleSpeed)
        {
            m_bobObj.localPosition = m_startPos;
            m_timer = 0f;
        }
    }

    // Stop bob motion and move camera back to start position
    private void StopBob()
    {
        m_timer = 0f;
        if (m_bobObj.localPosition == m_startPos) return;
        m_bobObj.localPosition = Vector3.Lerp(m_bobObj.localPosition, m_startPos, 7f * Time.deltaTime);
    }
}

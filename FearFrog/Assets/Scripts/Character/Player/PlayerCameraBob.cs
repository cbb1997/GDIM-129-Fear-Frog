using UnityEngine;

public class PlayerCameraBob : MonoBehaviour
{
    // Member variables
    [SerializeField] private bool m_bobEnabled = true;
    private float m_walkFrequency = 8f;        // Bob effect frequency
    private float m_sprintFrequency = 16f;
    private float m_crouchFrequency = 5f;
    private float m_walkMagModifier = 0.2f;   // Modifier for maginitude of bob effect
    private float m_sprintMagModifier = 0.6f;
    private float m_crouchMagModifier = 0.25f;
    private float m_amplitude = 0.001f;
    private float m_toggleSpeed = 0.3f;     // Speed threshold for whether apply bob effect
    private Vector3 m_startPos;
    private float timer = 0f;
    
    
    // Start
    void Start()
    {
        // Varialbe initialization
        m_startPos = PlayerStatus.Instance.Camera.localPosition;
    }

    // Update
    void Update()
    {
        if (!m_bobEnabled) return;

        // Check to play camera bob effect
        Vector3 horiVelocity = PlayerStatus.Instance.PlayerRb.linearVelocity;
        horiVelocity.y = 0f;
        if (!PlayerStatus.Instance.IsGrounded || horiVelocity.magnitude < m_toggleSpeed)
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
        // Check which frequency and magnitude modifier to use
        float frequency, magModifier;
        if (PlayerStatus.Instance.IsSprinting)
        {
            frequency = m_sprintFrequency;
            magModifier = m_sprintMagModifier;
        }
        else if (PlayerStatus.Instance.IsCrouching)
        {
            frequency = m_crouchFrequency;
            magModifier = m_crouchMagModifier;
        }
        else
        {
            frequency = m_walkFrequency;
            magModifier = m_walkMagModifier;
        }
        
        // Calculate and perform bob offset
        timer += Time.deltaTime;
        Vector3 offset = new Vector3();
        offset.x = (0.6f * m_amplitude * magModifier) * Mathf.Cos((frequency / 2f) * timer);
        offset.y = (-1.2f * m_amplitude * magModifier) * Mathf.Sin(frequency * timer);
        
        PlayerStatus.Instance.Camera.localPosition += offset;
    }

    // Stop bob motion and move camera back to start position
    private void StopBob()
    {
        timer = 0f;
        if (PlayerStatus.Instance.Camera.localPosition == m_startPos) return;
        PlayerStatus.Instance.Camera.localPosition = Vector3.Lerp(PlayerStatus.Instance.Camera.localPosition, m_startPos, 7f * Time.deltaTime);
    }
}

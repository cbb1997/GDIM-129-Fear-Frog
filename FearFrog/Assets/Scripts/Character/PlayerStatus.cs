using System;
using UnityEngine;

// Singleton class for player related statistics
public class PlayerStatus : MonoBehaviour
{
    // Singleton variables
    private static PlayerStatus m_instance;
    public static PlayerStatus Instance { get { return m_instance; } }
    
    // Player status
    private bool m_isGrounded = true;
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }
    private bool m_isGroundedPrev = true;
    public bool IsGroundedPrev { get { return m_isGroundedPrev; } set { m_isGroundedPrev = value; } }
    private bool m_isSprinting = false;
    public bool IsSprinting { get { return m_isSprinting; } set { m_isSprinting = value; } }
    private bool m_isCrouching = false;
    public bool IsCrounching { get { return m_isCrouching; } set { m_isCrouching = value; } }
    private RaycastHit m_groundHit;
    public RaycastHit GroundHit { get { return m_groundHit; } set { m_groundHit = value; } }
    
    private Vector3 m_groundNormal = new Vector3();         // Normal of the ground the player is standing on
    public Vector3 GroundNormal { get { return m_groundNormal; } }
    
    // Reference to other player gameObjects/components
    [SerializeField] private Transform m_playerEntity;
    public Transform PlayerEntity { get { return m_playerEntity; } }
    [SerializeField] private Transform m_cameraContainer;
    public Transform CameraContainer { get { return m_cameraContainer; } }
    [SerializeField] private Transform m_camera;
    public Transform Camera { get { return m_camera; } }
    [SerializeField] private Transform m_footPos;
    public Transform FootPos { get { return m_footPos; } }
	private Rigidbody m_playerRb;
    public Rigidbody PlayerRb { get { return m_playerRb; } }
    
    // Grounded check variables
    private float m_gcRadius = 0.35f;       // Grounndeed check shpere radius
    public float GcRadius { get { return m_gcRadius; } }
    [SerializeField] private float m_groundDrag = 5f;
    [SerializeField] private float m_airDrag = 0f;
    [SerializeField] private float m_stepDownDistance = 0.2f;
    

    // Awake
    void Awake()
    {
        // Singleton
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        m_instance = this;
    }
    
    // Start
    void Start()
    {
        // Initialization
        m_playerRb = this.GetComponent<Rigidbody>();
    }

    // FixedUpdate
    void FixedUpdate()
    {
        GroundCheck();
    }


    // Check whether the player is on the ground
    private void GroundCheck()
    {
        m_isGroundedPrev = m_isGrounded;
        m_isGrounded = Physics.SphereCast(transform.position, m_gcRadius, Vector3.down, out m_groundHit,
            -m_footPos.localPosition.y);
        
        // Update variables
        m_playerRb.linearDamping = m_isGrounded ? m_groundDrag : m_airDrag;
        m_groundNormal = m_groundHit.normal;
        
        // Update vertical velocity to 0 when just landed on ground
        if (m_isGrounded && !m_isGroundedPrev)
        {
            Vector3 newVelocity = m_playerRb.linearVelocity;
            newVelocity.y = 0f;
            m_playerRb.linearVelocity = newVelocity;   
        }
    }
    
    // Apply a second ground check with slightly longer detection distance
    public void SecondGroundCheck()
    {
        m_isGrounded = Physics.SphereCast(transform.position, m_gcRadius, Vector3.down, out m_groundHit,
            -m_footPos.localPosition.y + m_stepDownDistance);
        
        // Update variables
        m_playerRb.linearDamping = m_isGrounded ? m_groundDrag : m_airDrag;
        m_groundNormal = m_groundHit.normal;
    }
    
    
    private void OnDrawGizmos()
    {
        // Ground check
        Gizmos.color = Color.blue;
        Vector3 drawPos = transform.position;
        drawPos.y += m_footPos.localPosition.y;
        Gizmos.DrawSphere(drawPos, m_gcRadius);

        // Grounded point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_groundHit.point, 0.1f);
    }
}

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
    private bool m_isJumping = false;
    public bool IsJumping { get { return m_isJumping; } set { m_isJumping = value; } }
    
    private bool m_isSprinting = false;
    public bool IsSprinting { get { return m_isSprinting; } set { m_isSprinting = value; } }
    private bool m_isCrouching = false;
    public bool IsCrouching { get { return m_isCrouching; } set { m_isCrouching = value; } }
    private RaycastHit m_groundHit;
    public ref RaycastHit GroundHit { get { return ref m_groundHit; } }
    
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
}

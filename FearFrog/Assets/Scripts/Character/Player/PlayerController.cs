using System;
using UnityEngine;

// Singleton class for player related statistics
public class PlayerController : MonoBehaviour
{
    // Singleton variables
    private static PlayerController m_instance;
    public static PlayerController Instance { get { return m_instance; } }
    
    // Player events
    public delegate void MovementChange();
    public delegate void Fire();
    public delegate void RecoilComp(float comp);
    
    public event MovementChange OnStartSprinting;
    public void TriggerOnStartSprinting() { OnStartSprinting?.Invoke(); }
    public event MovementChange OnStartCrouching;
    public void TriggerOnStartCrouching() { OnStartCrouching?.Invoke(); }
    public event MovementChange OnBackToWalking;
    public void TriggerOnBackToWalking() { OnBackToWalking?.Invoke(); }
    public event Fire OnFire;
    public void TriggerOnFire() { OnFire?.Invoke(); }
    public event RecoilComp OnRecoilCompensation;
    public void TriggerOnRecoilCompensation(float comp) { OnRecoilCompensation?.Invoke(comp); }
    
    // Player respawn
    private Vector3 m_respawnPos;   // Ground point
    public Vector3 RespawnPos { set { m_respawnPos = value; } }
    
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
    private float m_playerScale = 1f;
    public float PlayerScale { get { return m_playerScale; } }
    
    // Reference to other player gameObjects/components
    [SerializeField] private Transform m_playerEntityContainer;
    public Transform PlayerEntityContainer { get { return m_playerEntityContainer; } }
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
    
    [SerializeField] private Animator animatorArmL;
    public static Animator staticAnimatorArmL;

    
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
        staticAnimatorArmL = animatorArmL;
    }
    
    // Start
    void Start()
    {
        // Initialization
        m_playerRb = this.GetComponent<Rigidbody>();
        m_playerScale = transform.localScale.x;
    }
    
    // Player respawn
    public void Respawn()
    {
        Vector3 pos = m_respawnPos;
        pos.y += 0.5f;
        
        transform.position = pos;
    }
}

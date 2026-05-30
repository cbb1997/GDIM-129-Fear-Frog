using System;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    // Member variables
    [SerializeField] private float m_recoilMag = 15f;
    [SerializeField] private float m_randOffset = 2f;
    [SerializeField] private float m_smooth = 7f;
    private Transform m_camera;
    private Quaternion m_initRot;
    private Quaternion m_currRot;
    private Quaternion m_weaponAddRotY;
    private Quaternion m_weaponAddRotX;
    
    // Reference to other components
    [SerializeField] private Transform m_rightArm;
    [SerializeField] private PlayerMovement m_playerMovement;

    
    // Start
    private void Start()
    {
        // Initialization
        m_camera = PlayerController.Instance.Camera;
        m_initRot = m_camera.localRotation;
        m_currRot = Quaternion.identity;
            
        PlayerController.Instance.OnFire += PerformRecoil;
    }
    
    // Update
    private void Update()
    {
        if (Quaternion.Angle(m_currRot, m_initRot) > 0.01f)
        {
            // Recoil compensation
            float comp = InputController.Instance.Input.Player.Look.ReadValue<Vector2>().y * 50f * Time.deltaTime;
            if (comp < 0f)
            {
                if (comp < (m_currRot.eulerAngles.x - 360f))
                {
                    comp = m_currRot.eulerAngles.x - 360f;
                }
                Quaternion compRot = Quaternion.AngleAxis(-comp, Vector3.right);
                m_currRot = m_currRot * compRot;
                PlayerController.Instance.TriggerOnRecoilCompensation(comp);
            }
            
            // Recoil
            m_currRot = Quaternion.Slerp(m_currRot, m_initRot, m_smooth * Time.deltaTime);
            m_camera.localRotation = m_initRot * m_currRot;
            m_rightArm.localRotation = m_initRot * m_currRot;
        }
    }

    // Recoil for firing
    private void PerformRecoil()
    {
        float angle = m_recoilMag + UnityEngine.Random.Range(-m_randOffset, m_randOffset);
        m_currRot = Quaternion.AngleAxis(-angle, Vector3.right) * m_currRot;
    }
}

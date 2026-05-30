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
            m_currRot = Quaternion.Slerp(m_currRot, m_initRot, m_smooth * Time.deltaTime);
            m_camera.localRotation = m_initRot * m_currRot;
            
            // m_weaponAddRotY = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 5f), Vector3.right);
            // m_weaponAddRotX = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 2f), Vector3.forward);
            // m_currRot = Quaternion.Slerp(m_currRot, m_currRot * m_weaponAddRotY * m_weaponAddRotX,
            //     m_smooth * Time.deltaTime);
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

using System;
using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    // Member variables
    [SerializeField] private Transform m_rightArm;
    [SerializeField] private float m_magnitude = 0.7f;
    [SerializeField] private float m_smooth = 5f;
    private float m_currAngleX = 0f;
    private float m_currAngleY = 0f;
    
    // Update
    private void Update()
    {
        // Weapon swing
        Vector2 mouseInput = InputController.Instance.Input.Player.Look.ReadValue<Vector2>();
        float targetAngleX = -m_magnitude * mouseInput.x;
        float targetAngleY = m_magnitude * mouseInput.y;
        targetAngleX = Mathf.Lerp(m_currAngleX, targetAngleX, m_smooth * Time.deltaTime);
        targetAngleY = Mathf.Lerp(m_currAngleY, targetAngleY, m_smooth * Time.deltaTime);
        
        float angleOffsetX = targetAngleX - m_currAngleX;
        float angleOffsetY = targetAngleY - m_currAngleY;

        Quaternion rotationX = Quaternion.AngleAxis(angleOffsetX, Vector3.forward);
        Quaternion rotationY = Quaternion.AngleAxis(angleOffsetY, Vector3.right);
        m_rightArm.localRotation = rotationX * rotationY * m_rightArm.localRotation;

        m_currAngleX = targetAngleX;
        m_currAngleY = targetAngleY;
    }
}

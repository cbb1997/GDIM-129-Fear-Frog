using System;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // Member variables
    [SerializeField] private Transform m_rightArm;
    [SerializeField] private float m_magnitude = 0.8f;
    [SerializeField] private float m_smooth = 5f;
    private float m_maxXDeg = 15f;
    private float m_maxYDeg = 12f;
    private Quaternion m_initRot;
    private Quaternion m_prevRot;
    
    
    // Start
    private void Start()
    {
        m_initRot = m_rightArm.localRotation;
        m_prevRot = quaternion.identity;
    }

    // Weapon sway
    private void Update()
    {
        // Calculate target quaternion
        Vector2 mouseInput = InputController.Instance.Input.Player.Look.ReadValue<Vector2>();
        float degX = Mathf.Clamp(-mouseInput.x * m_magnitude, -m_maxXDeg, m_maxXDeg);
        float degY = Mathf.Clamp(mouseInput.y * m_magnitude, -m_maxYDeg, m_maxYDeg);

        Quaternion rotX = Quaternion.AngleAxis(degX, -Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(degY, Vector3.forward);
        Quaternion rotSway = Quaternion.Slerp(m_prevRot, rotY * rotX, m_smooth * Time.deltaTime);
        m_prevRot = rotSway;

        // Apply rotation (first sway rotation => localRotation)
        m_rightArm.localRotation = m_initRot * rotSway;
    }
}

using System;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // Member variables
    [SerializeField] private Transform m_rightArm;
    [SerializeField] private float m_magnitude = 10f;
    [SerializeField] private float m_smooth = 5f;
    private Quaternion m_initRot;
    
    
    // Start
    private void Start()
    {
        m_initRot = m_rightArm.localRotation;
    }

    // Weapon sway
    private void Update()
    {
        // Calculate target quaternion
        Vector2 mouseInput = InputController.Instance.Input.Player.Look.ReadValue<Vector2>();
        float degX = -mouseInput.x;
        float degY = mouseInput.y;

        Quaternion rotX = Quaternion.AngleAxis(degX, -Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(degY, Vector3.forward);
        Quaternion rotTarget = m_initRot * rotY * rotX;
        
        // Apply rotation (first sway rotation => localRotation)
        m_rightArm.localRotation = Quaternion.Slerp(m_rightArm.localRotation, rotTarget, m_smooth * Time.deltaTime);
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    // Member variables
    private int m_ammoLeft = 100;
    private float m_maxBulletDist = 200f;
    private float m_defaultCoolDown = 2f;
    private float m_currCoolDown = 0f;
    
    
    // Start
    private void Start()
    {
        // Link player interaction functionality
        InputController.Instance.Input.Player.Fire.performed += Fire;
    }

    // Update
    private void Update()
    {
        // Update firing cooldown
        if (m_currCoolDown > 0f)
        {
            m_currCoolDown -= Time.deltaTime;
        }
    }

    // Player pistol fire
    private void Fire(InputAction.CallbackContext ctx)
    {
        if (m_currCoolDown <= 0f && m_ammoLeft > 0)
        {
            // Fire
            PlayerController.Instance.TriggerOnFire();

            SoundManager.PlaySound(SoundType.SHOOTING, 0.8f);
            
            m_ammoLeft -= 1;
            
            Ray ray = PlayerController.Instance.Camera.GetComponent<Camera>().ScreenPointToRay(
                new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, m_maxBulletDist))
            {
                // Hit!
            }
            
            // Set coolDown
            m_currCoolDown = m_defaultCoolDown;
        }
    }
}

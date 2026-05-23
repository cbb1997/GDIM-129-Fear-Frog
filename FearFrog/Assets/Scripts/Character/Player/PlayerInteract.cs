using System;
using System.Collections;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Member variables
    [SerializeField] private float m_maxDistance = 1.2f;
    private Vector3 m_screenCenter;
    
    
    // Start
    private void Start()
    {
        m_screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    // Update
    private void Update()
    {
        // Check for interactable objects
        Ray ray = PlayerController.Instance.Camera.GetComponent<Camera>().ScreenPointToRay(m_screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance))
        {
            IInteractable interactableObj = hit.collider.GetComponent<IInteractable>();
            if (interactableObj != null)
            {
                interactableObj.OnInteract();
            }   
        }
    }
}

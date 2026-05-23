using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // Member variables
    [SerializeField] private float m_maxDistance = 1.7f;
    private Vector3 m_screenCenter;
    private IInteractable interactableObj;
    
    
    // Start
    private void Start()
    {
        m_screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        
        // Link player interaction functionality
        InputController.Instance.Input.Player.Interact.performed += Interact;
    }
    
    // Update
    private void Update()
    {
        // Check for interactable objects
        Ray ray = PlayerController.Instance.Camera.GetComponent<Camera>().ScreenPointToRay(m_screenCenter);
        interactableObj = (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance)) 
            ? hit.collider.GetComponent<IInteractable>() : null;
        Debug.DrawLine(ray.origin, ray.origin + m_maxDistance * ray.direction);
        // Update UI prompt
        // if (interactableObj != null)
        // {
        //     SomeEvent?.Invoke();
        // }  
    }

    // Perform player interaction
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (interactableObj != null && interactableObj.CanInteract)
        {
            interactableObj.OnInteract();
        }   
    }
}

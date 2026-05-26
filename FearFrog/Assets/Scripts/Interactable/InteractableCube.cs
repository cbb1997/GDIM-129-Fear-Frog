using System;
using UnityEngine;

public class InteractableCube : MonoBehaviour, IInteractable
{
    // Interactable interface member variables
    private bool m_canInteract = true;
    public bool CanInteract { get { return m_canInteract; } set { m_canInteract = value; } }
    [SerializeField] private string m_promptMessage = "";
    public string PromptMessage { get { return m_promptMessage; } }
    

    // OnInteract() override
    public void OnInteract()
    {
        Debugger.Log(m_promptMessage);
    }
}

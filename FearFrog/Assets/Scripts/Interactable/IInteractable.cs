using UnityEngine;

// Interactable object interface
public interface IInteractable
{
    // Member variable
    bool CanInteract { get; set; }
    string PromptMessage { get; }

    // Logic when interacted with
    void OnInteract();
}

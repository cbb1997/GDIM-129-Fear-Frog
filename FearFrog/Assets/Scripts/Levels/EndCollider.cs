using UnityEngine;
using System;

public class EndCollider : MonoBehaviour
{
    public static Action OnPlayerWon;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnPlayerWon?.Invoke();
        }
    }
   
}

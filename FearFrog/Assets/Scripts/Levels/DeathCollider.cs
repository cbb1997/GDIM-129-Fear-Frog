using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        //Debugger.Log(other.tag);

        switch (other.tag)
        {
            case "Player":
                //PlayerController player = other.GetComponent<PlayerController>();
                //player.Kill();
                break;
            case "Enemy":
                other.GetComponent<EnemyController>().Respawn();
                break;
            default:
                Destroy(other.gameObject);
                break;
        }
    }
}

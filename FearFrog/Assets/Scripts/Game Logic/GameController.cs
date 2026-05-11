using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);

        Debugger.Log("Engine initialized...");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene("mrFrog");
    }
}

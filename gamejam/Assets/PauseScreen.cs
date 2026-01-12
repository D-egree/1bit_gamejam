using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    
    void Update()
    { 
    if (Input.GetKeyDown(KeyCode.Escape))
        { 
        if (GameIsPaused)
            {
                Resume();
            }
        else
            {
                Pause();
            }
        }
    }
    
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
    public void Resume()
    { 
    
    }
    
    public void RestartButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}

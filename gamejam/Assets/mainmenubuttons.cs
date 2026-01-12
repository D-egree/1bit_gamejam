using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenubuttons : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("UI Panels")]
    [SerializeField] private GameObject creditsPanel;

    // =========================
    // BUTTON FUNCTIONS
    // =========================

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ToggleCredits(bool show)
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(show);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

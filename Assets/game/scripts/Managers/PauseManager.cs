using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Drag in your HUD Panel here (contains the Pause button)")]
    public GameObject hudPanel;

    [Header("Drag in your Pause Menu Panel here (contains Quit/Resume buttons)")]
    public GameObject pauseMenuPanel;

    bool isPaused = false;

    void Start()
    {
        // Ensure we start unpaused, HUD visible, menu hidden
        Time.timeScale = 1f;
        hudPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        // hide HUD, show Pause Menu
        hudPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        // show HUD, hide Pause Menu
        pauseMenuPanel.SetActive(false);
        hudPanel.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Load the first scene (build index 0) and reset time scale.
    /// </summary>
    public void QuitGame()
    {
        // Make sure time is running again
        Time.timeScale = 1f;

        // Load the very first scene in your Build Settings
        SceneManager.LoadScene(0);
    }
}
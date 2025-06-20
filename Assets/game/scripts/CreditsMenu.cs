using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    private int returnToIndex;

    void Start()
    {
        // Only allow this to be loaded from Title screen (build index 0)
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.LogWarning("Credits menu can only be accessed from Title screen.");
            returnToIndex = 0; // fallback just in case
        }
        else
        {
            returnToIndex = 0;
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene(returnToIndex);
    }
}

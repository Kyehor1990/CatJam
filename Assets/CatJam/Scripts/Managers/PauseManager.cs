using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        // ESC tuşuna yalnızca oyun sahnesindeysek tepki ver
        if (Input.GetKeyDown(KeyCode.Escape) && IsInGameScene())
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    bool IsInGameScene()
    {
        // MainMenu veya Credits gibi menü sahneleri değilse oyun sahnesindeyiz diyelim
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName != "MainMenu" && sceneName != "CreditsScene";
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

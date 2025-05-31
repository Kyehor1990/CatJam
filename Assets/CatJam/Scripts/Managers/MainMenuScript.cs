using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "Boss1Scene"; // İlk sahne adı
    [SerializeField] private GameObject settingsPanel; // Ayarlar paneli

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Çıkış yapıldı.");
        Application.Quit();
    }
}

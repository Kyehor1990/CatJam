using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "Boss1Scene"; // İlk sahne adı
    [SerializeField] private GameObject settingsPanel; // Ayarlar paneli

    public static MainMenu Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Eğer ayarlar paneli açıksa ESC kapatsın
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                // Ayarlar paneli kapalıysa, ESC ile ana menüde farklı bir işlem yapmak istersen buraya yazabilirsin.
                // Örnek: Oyundan çıkış için onay paneli açmak vs.
                Debug.Log("ESC basıldı, ama ayar paneli kapalı.");
            }
        }
    }

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

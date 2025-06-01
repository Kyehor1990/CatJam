using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "Boss1Scene"; // İlk sahne adı
    [SerializeField] private GameObject settingsPanel; // Ayarlar paneli
    

    public static MainMenu Instance;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // İkinci GameManager sahneye girerse yok et
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
    }



    void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeSelf)
    {
        CloseSettings();
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

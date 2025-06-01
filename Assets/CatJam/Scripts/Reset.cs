using UnityEngine;

public class MainMenuReset : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
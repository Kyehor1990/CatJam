using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource; // Arka plan müziği

    private void Awake()
    {
        // Singleton sistemi
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince yok olmasın
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
            bgmSource.volume = volume;
    }

    public void MuteBGM(bool mute)
    {
        if (bgmSource != null)
            bgmSource.mute = mute;
    }

    public void PlayBGM()
    {
        if (!bgmSource.isPlaying)
            bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }
}

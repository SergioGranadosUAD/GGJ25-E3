using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Fuente para música de fondo
    public AudioSource sfxSource;     // Fuente para efectos de sonido

    [Header("Configuración de Audio")]
    [Range(0, 1)] public float musicVolume = 1.0f;
    [Range(0, 1)] public float sfxVolume = 1.0f;

    private void Awake()
    {
        // Implementar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }

    /// <summary>
    /// Reproduce una pista de música.
    /// </summary>
    /// <param name="clip">Clip de audio para la música.</param>
    /// <param name="loop">Si debe repetirse en bucle.</param>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    /// <summary>
    /// Reproduce un efecto de sonido.
    /// </summary>
    /// <param name="clip">Clip de audio para el efecto.</param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    /// <summary>
    /// Detiene la música.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Cambia el volumen de la música.
    /// </summary>
    /// <param name="volume">Nuevo volumen (0.0 - 1.0).</param>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    /// <summary>
    /// Cambia el volumen de los efectos de sonido.
    /// </summary>
    /// <param name="volume">Nuevo volumen (0.0 - 1.0).</param>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
